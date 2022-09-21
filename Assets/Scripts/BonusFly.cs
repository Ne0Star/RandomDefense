using UnityEngine;

public class BonusFly : MonoBehaviour
{
    public DynamicData rocketType;
    [SerializeField] private int current = 0;
    [SerializeField] private Transform target;
    [SerializeField] TrajectorySystem trajectory;
    [SerializeField] private GroundSpawner[] trajectories;
    [SerializeField] private StaticEntityTrigger trigger;
    [SerializeField] private bool blocker = false;
    [SerializeField] private int attackIndex;
    [SerializeField] private Patron[] patrons;
    private void Start()
    {
        trajectory = FindObjectOfType<TrajectorySystem>();
    }
    private Patron GetPatron()
    {
        for (int i = 0; i < patrons.Length; i++)
        {
            Patron p = patrons[i];
            if (p)
            {
                if (!p.gameObject.activeInHierarchy)
                    return p;
            }
        }
        return null;
    }
    public void Fly()
    {
        if (!trajectory || !target || blocker) return;
        current = 0;
        GroundSpawner s = trajectories[Random.Range(0, trajectories.Length - 1)];
        if (!s.GetTarget(current)) return;
        blocker = true;
        target.gameObject.SetActive(true);

        CreateFly(s);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fly();
        }
    }
    private Vector3 tt;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(tt, 5f);
    }
    private void CreateFly(GroundSpawner s)
    {
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
        //GroundSpawner s = trajectories[Random.Range(0, trajectories.Length - 1)];

        trajectory.CreatePatron(dt, target, s.GetTarget(current), (p, t) =>
        {
            if (current == attackIndex)
            {
                tt = target.transform.position;
                Patron pt = GetPatron();
                if (pt != null)
                {
                    pt.gameObject.SetActive(true);
                    pt.transform.position = tt;
                    pt.Explose(() =>
                    {
                        GameUtils.DealSplashDamage(target.transform.position, trigger.GetAllRadius().ToArray(), trigger.TriggerRadius / 2, 50);
                        pt.Explose(() =>
                        {
                            GameUtils.DealSplashDamage(target.transform.position, trigger.GetAllRadius().ToArray(), trigger.TriggerRadius, 50);
                            pt.gameObject.SetActive(false);
                        });
                    });

                }

            }
            if (current < s.GetPath().Length)
            {
                CreateFly(s);
                current++;
            }
            else
            {
                blocker = false;
                target.gameObject.SetActive(false);
            }
        });
    }
}
