using System.Collections;
using UnityEngine;

[System.Serializable]
public struct MissilesType
{
    [SerializeField] private bool useRandomCurve;
    [SerializeField] private float randomMinX, randomMaxX;
    [SerializeField] private float randomMinY, randomMaxY;
    public float RandomMinX { get => randomMinX; set => randomMinX = value; }
    public float RandomMaxX { get => randomMaxX; set => randomMaxX = value; }
    public float RandomMinY { get => randomMinY; set => randomMinY = value; }
    public float RandomMaxY { get => randomMaxY; set => randomMaxY = value; }
    public bool UseRandomCurve { get => useRandomCurve; set => useRandomCurve = value; }



    public void SetRandomKeys()
    {
        SetRandom(ref yOffset, randomMinY, randomMaxY);
        SetRandom(ref xOffset, randomMinX, randomMaxX);
    }

    private void SetRandom(ref AnimationCurve curve, float min, float max)
    {
        AnimationCurve result = new AnimationCurve();
        Keyframe[] keys = curve.keys;
        for (int I = 0; I < keys.Length; I++)
        {
            if (I > 0 && I < keys.Length - 1)
            {
                Keyframe key = keys[I];
                key.value = Random.Range(key.value - min, key.value + max) * -1f;
                keys[I] = key;
            }
        }
        curve.keys = keys;
    }

    public AnimationCurve SetRandomKeys(AnimationCurve origin, float minValue, float maxValue)
    {
        AnimationCurve result = new AnimationCurve();
        Keyframe[] keys = origin.keys;
        for (int I = 0; I < keys.Length; I++)
        {
            if (I > 0 && I < keys.Length - 1)
            {
                Keyframe key = keys[I];
                key.value = Random.Range(key.value - minValue, key.value + maxValue) * -1f;
                keys[I] = key;
            }
        }
        result.keys = keys;
        return result;
    }

    [SerializeField] private AnimationCurve yOffset, xOffset, speedInterpolator;
    public AnimationCurve YOffset { get => yOffset; set => yOffset = value; }
    public AnimationCurve XOffset { get => xOffset; set => xOffset = value; }
    public AnimationCurve SpeedInterpolator { get => speedInterpolator; set => speedInterpolator = value; }
}

public class RocketTurret : ShellTurret
{
    [SerializeField] private MultiplerValue splashRadius;

    [SerializeField] private RocketSpawner[] rocketSpawners;
    [SerializeField] private DynamicData rocketType;

    [SerializeField] private float attackDamage;
    [SerializeField] private int attackCount;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float reloadSpeed;
    private void OnDisable()
    {
        if (patrons != null)
            foreach (Patron p in patrons)
            {
                if (p)
                {
                    if (p.destroyParticle)
                    {
                        p.destroyParticle.gameObject.SetActive(false);
                    }
                    p.gameObject.SetActive(false);
                }

            }
        attack = false;
    }
    protected override void OnAttack()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(Attack_Animation());
    }
    public override void UpdateTurret(int currentWave, float minDamage, float maxDamage)
    {
        attackDamage += Random.Range(minDamage, maxDamage);
    }
    private IEnumerator Attack_Animation()
    {


        for (int i = 0; i < attackCount; i++)
        {
            if (!target || !target.gameObject.activeInHierarchy) break;
            if (mainTrigger && !mainTrigger.CheckRadius(target))
            {
                target = null;
                break;
            }
            GameUtils.LookAt2DSmooth(transform, target.transform.position, rotateOffset, rotateSpeed, 0.1f);


            foreach (RocketSpawner spawner in rocketSpawners)
            {
                Patron result = GetFreePatron();
                if (result)
                {
                    result.transform.position = spawner.transform.position;
                    result.gameObject.SetActive(true);

                    if (!result) break;
                    CreateRocket(result);
                    spawner.vfxAttack.gameObject.SetActive(true);
                    yield return new WaitForSeconds((attackSpeed / LevelManager.Instance.GameSpeed) / 2f);
                    spawner.vfxAttack.gameObject.SetActive(false);
                    yield return new WaitForSeconds((attackSpeed / LevelManager.Instance.GameSpeed));
                }
            }

        }
        attack = false;
        rotated = false;
        yield break;
    }

    private void OnDestroy()
    {
        try
        {
            foreach (RocketSpawner spawner in rocketSpawners)
            {
                if (spawner)
                {
                    Destroy(spawner.gameObject);
                }
            }
            foreach (Patron patron in patrons)
            {
                if (patron)
                {
                    if (patron.destroyParticle)
                    {
                        Destroy(patron.destroyParticle.gameObject);
                    }
                    Destroy(patron.gameObject);
                }
            }
        }
        catch
        {

        }

    }

    private void CreateRocket(Patron result)
    {

        if (rocketSpawners != null)
        {
            int index = Random.Range(0, rocketSpawners.Length);
            if (index <= rocketSpawners.Length - 1)
                if (rocketSpawners[index])
                {
                    result.transform.position = rocketSpawners[index].transform.position;
                }
        }
        DynamicData dt = rocketType;
        if (!target)
        {

            if (result)
                result.gameObject.SetActive(false);

            return;
        }
        dt = new DynamicData()
        {
            source = transform,
            rotateOffset = rocketType.rotateOffset,
            distanceToCompleted = rocketType.distanceToCompleted,
            currentOffsetMultipler = rocketType.currentOffsetMultipler * Vector2.Distance(transform.position, target.transform.position),
            currentSpeed = rocketType.currentSpeed * LevelManager.Instance.GameSpeed,
            currentTime = rocketType.currentTime,
            endPosition = Vector3.zero,
            missilesType = rocketType.missilesType,
            pursue = rocketType.pursue,
            searchRadius = rocketType.searchRadius,
            rotateToTarget = rocketType.rotateToTarget
        };
        result.updateParticle.SetActive(true);
        result.EnableRendering();
        //result.SetDestroyParticleScale(new Vector3(splashRadius.GetCurrentValue() * 2, splashRadius.GetCurrentValue() * 2, splashRadius.GetCurrentValue() * 2));
        if (target)
        {
            trajectory.CreatePatron(dt, result.transform, target.transform, (p, t) =>
            {
                if (target && gameObject.activeInHierarchy)
                {
                    StartCoroutine(PatronComplete(result, target));
                }
                else
                {
                    result.gameObject.SetActive(false);
                }

            });
        }
        else
        {
            result.gameObject.SetActive(false);
        }
    }

    private IEnumerator PatronComplete(Patron patron, EntityUnit target)
    {

        patron.destroyParticle.transform.SetParent(null);
        patron.DisableRendering();
        patron.destroyParticle.SetActive(true);
        if (splashRadius.GetCurrentValue() != 0)
        {
            GameUtils.DealSplashDamage(patron.transform.position, mainTrigger.GetAllEntities(), splashRadius.GetCurrentValue(), attackDamage);
        }
        else
        {
            if (target) target.TakeDamage(attackDamage, 0.1f, patron.transform.position);
        }
        yield return new WaitForSeconds(patron.destroyParticleLife / LevelManager.Instance.GameSpeed);
        patron.destroyParticle.SetActive(false);
        patron.destroyParticle.transform.SetParent(patron.transform);
        patron.destroyParticle.transform.localPosition = Vector3.zero;
        patron.gameObject.SetActive(false);
        patron.transform.position = transform.position;
    }

}
