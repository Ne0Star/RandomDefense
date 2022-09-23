using System.Collections;
using UnityEngine;

public class MultiRocketTurret : ShellTurret
{
    [SerializeField] private MultiplerValue splashRadius;

    [SerializeField] private RocketSpawner[] rocketSpawners;
    [SerializeField] private AttackEffect attackEffect;
    [SerializeField] private DynamicData rocketType;

    [SerializeField] private float attackDamage;
    [SerializeField] private int attackCount;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float reloadSpeed;

    protected override void OnAttack()
    {
        StartCoroutine(Attack_Animation());
    }
    private void OnDisable()
    {
        foreach(Patron p in patrons)
        {
            if(p)
            Destroy(p.gameObject);
        }
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


                    CreateRocket(result);
                    if (spawner.vfxAttack)
                        spawner.vfxAttack.gameObject.SetActive(true);
                    yield return new WaitForSeconds((attackSpeed / LevelManager.Instance.GameSpeed) / 2f);
                    if (spawner.vfxAttack)
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
        StopAllCoroutines();
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
                if (target)
                {
                    StartCoroutine(PatronComplete((MultiRocket)result, target));
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

    private IEnumerator PatronComplete(MultiRocket patron, EntityUnit target)
    {
        patron.Explose(() =>
        {
            patron.gameObject.SetActive(false);
            patron.transform.position = transform.position;
        });

        patron.gameObject.transform.parent = transform;
        patron.destroyParticle.transform.SetParent(null);
        patron.DisableRendering();
        patron.destroyParticle.SetActive(true);
        if (splashRadius.GetCurrentValue() != 0)
        {
            GameUtils.DealSplashDamage(patron.transform.position, LevelManager.Instance.EnemuManager.GetAllEnemies().ToArray(), splashRadius.GetCurrentValue(), attackDamage);
        }
        else
        {
            if (target) target.TakeDamage(attackDamage, 0.1f, patron.transform.position);
        }
        yield return new WaitForSeconds(patron.destroyParticleLife / LevelManager.Instance.GameSpeed);
        patron.destroyParticle.SetActive(false);
        patron.destroyParticle.transform.SetParent(patron.transform);
        patron.destroyParticle.transform.localPosition = Vector3.zero;
    }
}
