using System.Collections;
using UnityEngine;

public enum DroneActionType
{
    Health,
    Attack
}

/// <summary>
/// Выпускает дронов
/// </summary>
public class DroneTurret : Turret
{
    [SerializeField] private int tickToDamage;
    [SerializeField] private int currentTick;
    [SerializeField] private bool blocker = false;
    [SerializeField] private MultiplerValue actionValue;
    [SerializeField] private MultiplerValue actionDuration;
    [SerializeField] private MultiplerValue allDuration;
    [SerializeField] private DroneActionType droneBehaviour;
    [SerializeField] private StaticEntityTrigger mainTrigger;
    [SerializeField] private TrajectorySystem droneSystem;
    [SerializeField] private DynamicData moveType;
    [SerializeField] private Drone[] drones;

    private void Start()
    {
        drones = gameObject.GetComponentsInChildren<Drone>();
        blocker = false;
    }

    private void OnEnable()
    {
        droneSystem = FindObjectOfType<TrajectorySystem>();
    }
    public override float GetTarretRadius()
    {
        return mainTrigger.TriggerRadius;
    }
    public override void HideRadius()
    {
        radius.Close();
    }

    public override void ShowRadius()
    {
        radius.Open(mainTrigger.TriggerRadius);
    }
    private Drone GetDrone()
    {
        Drone resut = null;
        foreach (Drone dron in drones)
        {
            if (dron.isComplete)
            {
                resut = dron;
            }
        }
        return resut;
    }
    public override void Tick()
    {
        if (target)
        {
            if (!target.gameObject.activeInHierarchy || !mainTrigger.CheckRadius(target))
            {
                target = null;
               // blocker = false;
            }
        }
        currentTick++;
        if (currentTick >= tickToDamage)
        {
            if (!blocker)
                StartCoroutine(Attack());
            currentTick = 0;
        }
    }

    private IEnumerator Attack()
    {

        blocker = true;
        targets = mainTrigger.GetAllRadius();
        target = mainTrigger.GetOneRandom();
        if (target && target.gameObject.activeInHierarchy)
            foreach (EntityUnit target in targets)
            {
                if (target && target.gameObject.activeInHierarchy)
                {
                    Drone drone = GetDrone();
                    if (drone)
                    {
                        switch (droneBehaviour)
                        {
                            case DroneActionType.Attack:
                                drone.isComplete = false;
                                drone.SetViewTarget(target.transform.position);
                                MoveDrone(false, drone, target.transform, (p, t) => StartCoroutine(OnComplete(p, t)));
                                break;
                            case DroneActionType.Health:
                                //Debug.Log(target);
                                if (!target.Hit.isFull())
                                {
                                    drone.isComplete = false;
                                    MoveDrone(false, drone, target.transform, (p, t) => StartCoroutine(OnComplete(p, t)));
                                }
                                break;
                        }

                    }
                }
                yield return new WaitForSeconds(actionDuration.GetCurrentValue() / LevelManager.Instance.GameSpeed);
            }
        blocker = false;
    }

    private void MoveDrone(bool itsHome, Drone drone, Transform target, System.Action<Transform, Transform> complete)
    {
        DynamicData dt = moveType;
        dt = new DynamicData()
        {
            source = transform,
            rotateOffset = dt.rotateOffset,
            distanceToCompleted = dt.distanceToCompleted,
            currentOffsetMultipler = dt.currentOffsetMultipler,
            currentSpeed = dt.currentSpeed * LevelManager.Instance.GameSpeed,
            currentTime = dt.currentTime,
            endPosition = Vector3.zero,
            missilesType = dt.missilesType,
            pursue = dt.pursue,
            searchRadius = dt.searchRadius,
            rotateToTarget = dt.rotateToTarget
        };
        droneSystem.CreatePatron(dt, drone.transform, target, complete);
    }
    private IEnumerator OnComplete(Transform patron, Transform target)
    {
        //yield return new WaitForSeconds(actionDuration.GetCurrentValue() / LevelManager.Instance.GameSpeed);
        if (target && patron)
        {
            Drone drone = patron.GetComponent<Drone>();
            EntityUnit t = target.GetComponent<EntityUnit>();
            if (drone && t)
            {
                drone.ApplyAction(droneBehaviour, t, actionValue.GetCurrentValue());
                yield return new WaitForSeconds(drone.SFXDuration);
                drone.CloseSFX();
                drone.RemoveViewTarget();
                MoveDrone(true, drone, drone.station, (p, v) => CompleteHome(drone));
            }
        }
        else if (patron && !target)
        {
            Drone drone = patron.GetComponent<Drone>();
            MoveDrone(true, drone, drone.station, (p, v) => CompleteHome(drone));
        }
    }

    private void CompleteHome(Drone drone)
    {
        drone.MoveToStartPosition();
    }

    private void OnDrawGizmos()
    {
        if (radius)
        {
            radius.transform.localScale = new Vector3(mainTrigger.TriggerRadius, mainTrigger.TriggerRadius, 0);
        }
    }

}
