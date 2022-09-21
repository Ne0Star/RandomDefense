using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRocket : Patron
{
    [SerializeField] private MultiplerValue splashRadius;
    [SerializeField] private float attackDamage;
    [SerializeField] private StaticEntityTrigger trigger;
    [SerializeField] private int batchCount;
    [SerializeField] private int miniRocketCount;
    [SerializeField] private Patron patronPrefab;
    [SerializeField] private List<Patron> patrons;
    [SerializeField] private DynamicData rocketType;
    [SerializeField] TrajectorySystem trajectory;
    private System.Action onComplete;
    private IEnumerator Start()
    {
        while (!trajectory)
        {
            trajectory = FindObjectOfType<TrajectorySystem>();
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < batchCount; i++)
        {
            Patron mr = Instantiate(patronPrefab, transform.position, Quaternion.identity);
            mr.gameObject.SetActive(false);
            patrons.Add(mr);
        }
    }

    private Patron GetPatron()
    {
        Patron result = null;
        for (int i = 0; i < patrons.Count; i++)
        {
            if (patrons[i])
                if (!patrons[i].gameObject.activeInHierarchy)
                    result = patrons[i];
        }
        return result;
    }

    private bool AllPatronComlete()
    {
        bool result = true;
        for (int i = 0; i < patrons.Count; i++)
        {
            if (patrons[i])
                if (patrons[i].gameObject.activeInHierarchy)
                    result = false;
        }
        return result;
    }

    public override void Explose(System.Action onComplete)
    {
        this.onComplete = onComplete;
        DisableRendering();
        for (int i = 0; i < miniRocketCount; i++)
        {
            CreatePatron();
        }
        StartCoroutine(WaitComplete());
    }

    private IEnumerator WaitComplete()
    {
        //destroyParticle.gameObject.SetActive(true);
        //yield return new WaitForSeconds(destroyParticleLife);
        //destroyParticle.gameObject.SetActive(false);

        yield return new WaitUntil(() => AllPatronComlete());
        onComplete();
        Debug.Log("Все патроны готовы");
    }

    private void CreatePatron()
    {
        EntityUnit entity = trigger.GetOneRandom();
        Patron result = GetPatron();
        if (!entity || !result) return;
        result.gameObject.SetActive(true);
        result.transform.parent = null;
        result.EnableRendering();
        DynamicData dt = rocketType;
        dt = new DynamicData()
        {
            source = transform,
            rotateOffset = rocketType.rotateOffset,
            distanceToCompleted = rocketType.distanceToCompleted,
            currentOffsetMultipler = rocketType.currentOffsetMultipler * Vector2.Distance(transform.position, entity.transform.position),
            currentSpeed = rocketType.currentSpeed * LevelManager.Instance.GameSpeed,
            currentTime = rocketType.currentTime,
            endPosition = Vector3.zero,
            missilesType = rocketType.missilesType,
            pursue = rocketType.pursue,
            searchRadius = rocketType.searchRadius,
            rotateToTarget = rocketType.rotateToTarget
        };
        trajectory.CreatePatron(dt, result.transform, entity.transform, (p, t) =>
        {
            if (entity)
            {
                StartCoroutine(PatronComplete(result, entity));
            }
            else
            {
                result.gameObject.SetActive(false);
            }
        });
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        foreach (Patron p in patrons)
        {
            if (p)
                Destroy(p.gameObject);
        }
    }
    private IEnumerator PatronComplete(Patron patron, EntityUnit target)
    {
        patron.destroyParticle.transform.SetParent(null);
        patron.DisableRendering();
        patron.destroyParticle.SetActive(true);
        if (target) GameUtils.DealSplashDamage(target.transform.position, LevelManager.Instance.EnemuManager.GetAllEnemies().ToArray(), splashRadius.GetCurrentValue(), attackDamage);// target.TakeDamage(attackDamage, 0.1f, patron.transform.position);
        yield return new WaitForSeconds(patron.destroyParticleLife / LevelManager.Instance.GameSpeed);
        patron.destroyParticle.SetActive(false);
        patron.destroyParticle.transform.SetParent(patron.transform);
        patron.destroyParticle.transform.localPosition = Vector3.zero;
        patron.gameObject.SetActive(false);
        patron.transform.parent = transform;
        patron.transform.localPosition = Vector3.zero;
    }

}
