using System.Collections;
using UnityEngine;

/// <summary>
/// Выпускает диски для атаки
/// </summary>
public class DiscTurret : ShellTurret
{
    [SerializeField] private DynamicData moveType;
    [SerializeField] private Transform[] spawners;
    [SerializeField] private Transform radiusTarget;

    [SerializeField] private int attackSpeed;
    [SerializeField] private int currentTick;



    public override void ShowRadius()
    {
        radius.Open();
    }
    public override void HideRadius()
    {
        radius.Close();
    }

    public override void Tick()
    {
        currentTick++;
        if (currentTick >= attackSpeed)
        {
            Attack();
            currentTick = 0;
        }
    }

    private void Attack()
    {
        if (!mainTrigger || !mainTrigger.gameObject) return;
        if (target != null && target.gameObject.activeInHierarchy)
        {
            if (mainTrigger && !mainTrigger.CheckRadius(target))
            {
                target = null;
            }
        }
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            if(mainTrigger && mainTrigger.gameObject)
            {
            target = mainTrigger.GetOneNear(false);
            if (!target || !target.gameObject.activeInHierarchy)
                target = mainTrigger.GetOneNear(true);
            }
        }
        if (target == null || !target.gameObject.activeInHierarchy) return;
        Patron result = GetFreePatron();
        if (result)
        {
            result.transform.position = transform.position;
            result.gameObject.SetActive(true);
            Disc res = (Disc)result;
            res.startPos = transform.position;
            res.turretRadius = mainTrigger.TriggerRadius;
            CreateRocket(res);
        }
        //GameUtils.LookAt2D(transform, target.transform, rotateOffset, 0.2f, () =>
        //{

        //});
    }


    private void CreateRocket(Patron result)
    {
        if (spawners != null)
        {
            int index = Random.Range(0, spawners.Length);
            if (index <= spawners.Length - 1)
                if (spawners[index])
                {
                    result.transform.position = spawners[index].position;
                }
        }

        DynamicData dt = moveType;
        dt = new DynamicData()
        {
            source = transform,
            rotateOffset = moveType.rotateOffset,
            distanceToCompleted = moveType.distanceToCompleted,
            currentOffsetMultipler = moveType.currentOffsetMultipler,
            currentSpeed = moveType.currentSpeed,
            currentTime = moveType.currentTime,
            endPosition = Vector3.zero,
            missilesType = moveType.missilesType,
            pursue = moveType.pursue,
            searchRadius = moveType.searchRadius,
            rotateToTarget = moveType.rotateToTarget
        };
        if (target)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Vector3 center = transform.position + (dir * mainTrigger.TriggerRadius);
            center = new Vector3(center.x, center.y, 0);
            radiusTarget.transform.position = center;
            trajectory.CreatePatron(dt, result.transform, radiusTarget.transform, (p, t) =>
            {
                radiusTarget.transform.position = transform.position;
                result.gameObject.SetActive(false);
                //if (target)
                //{
                //    StartCoroutine(PatronComplete(result, target));
                //}
                //else
                //{
                //    result.gameObject.SetActive(false);
                //}

            });
        }
        else
        {
            result.gameObject.SetActive(false);
        }
    }

    private IEnumerator PatronComplete(Patron patron, EntityUnit target)
    {

        patron.gameObject.SetActive(false);
        patron.transform.position = transform.position;
        yield break;
    }
}
