using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

/// <summary>
/// Управляет врагами
/// </summary>
public class EnemuManager : MonoBehaviour
{
    [SerializeField] private float allSpeedMultipler, defaultSpeedMultipler;
    [SerializeField] private float updateTime;

    [SerializeField] private Transform spawnParent;

    [SerializeField] private List<EnemuVariant> enemuVariants = new List<EnemuVariant>();
    [SerializeField] private List<Enemu> activeEnemies;
    [SerializeField] private List<DynamicMoveData> dynamicDatas;
    [SerializeField] private NativeArray<MoveEnemuData> moveDatas;
    [SerializeField] private NativeArray<Vector3> allPositions;


    private List<Transform> activeEnemiesTransforms;
    private List<Spawner> spawners;
    private List<int> spawnersIndex;

    [SerializeField] private float pose;
    [SerializeField] private int groupCount;

    private EnemuMover enemuMover;
    private HitsRotator hitsRotator;
    //private JobHandle moveHandle, collisionHandle, hitsHandle;

    public int GroupCount { get => groupCount; set => groupCount = value; }
    public float Pose { get => pose; set => pose = value; }


    [System.Serializable]
    public struct EnemuVariant
    {
        public Enemu variant;
        public int batchCount;
    }

    public ref List<Enemu> GetAllActiveEnemies()
    {
        return ref activeEnemies;
    }

    public int ActiveEnemiesCount => activeEnemies.Count;

    [SerializeField] private int distanceCheckFrames;

    public int CheckPositionTime => distanceCheckFrames; // Время проверки на дсиатнцию


    public IEnumerator SpawnIntellectually(int count, int currentWave, float maxHealth, Spawner spawner, float minHp, float maxHp, float minDmg, float maxDmg)
    {
        List<Enemu> variants = new List<Enemu>();
        for (int i = 0; i < enemuVariants.Count; i++)
        {
            if (enemuVariants[i].variant != null)
            {
                if (enemuVariants[i].variant.Hit.GetMaxHealth() < maxHealth)
                {
                    //Debug.Log(enemuVariants[i].variant.Hit.GetMaxHealth() + " " + maxHealth);
                    variants.Add(enemuVariants[i].variant);
                }
            }
        }
        variants.Distinct();
        if (variants.Count > 0)
        {

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, variants.Count - 1);// .Range();
                bool spawn = Random.Range(0, 100) < variants[randomIndex].ReachCost.Chance;
                if (spawn)
                {
                    Enemu result = null;
                    result = (Enemu)variants[randomIndex].Spawn(spawner.transform.position, Quaternion.identity, spawner, null, () =>
                    {
                        if (result)
                            result.EnemuUpdate(currentWave, minHp, maxHp, minDmg, maxDmg);
                    });
                }
                yield return new WaitForSeconds(0.005f);
            }
        }
        yield break;
    }

    public Enemu SpawnEnemu<T>(Vector3 position, Spawner spawner)
    {
        //if (!isEmpty) return null;
        Enemu result = null;
        for (int i = 0; i < dynamicDatas.Count; i++)
        {
            result = dynamicDatas[i].enemu;
            // Если в пуле есть свободная сущность данного типа
            if (!result.gameObject.activeInHierarchy && result is T)
            {
                if (spawner != null)
                {
                    if (!spawners.Contains(spawner))
                    {
                        spawners.Add(spawner);
                        spawnersIndex.Add(1);
                    }
                    else
                    {
                        spawnersIndex[spawners.IndexOf(spawner)]++;
                    }
                }


                result.CurrentSpawner = spawner;
                result.transform.position = spawner.transform.position;
                result.gameObject.SetActive(true);
                result.OnDied_.AddListener((e) => DiedEnemu((Enemu)e, false));
                result.PositionIndex = 0;
                MoveEnemuData md = moveDatas[i];
                md.NextPosition = result.transform.position;
                md.Speed = result.Speed;
                md.ColliderRadius = result.Agent.radius;
                md.RotateOffset = result.RotateOffset;
                md.RotateSpeed = result.RotateSpeed;
                md.DistanceToComplete = spawner.ReachDistance;
                md.pose = pose;
                md.currentFramesCount = 0;
                md.scalePersent = spawner.ReachDistance / 2f;
                md.groupIndex = spawnersIndex[spawners.IndexOf(spawner)];
                md.maxFramesCountToSaveLastDistance = CheckPositionTime;
                md.noWaitRotate = false;
                //md.groupID = maxID;
                //md.groupIndex = currentIndex;
                moveDatas[i] = md;
                dynamicDatas[i].enemu = result;
                activeEnemies.Add(result);
                activeEnemiesTransforms.Add(result.transform);
                break;
            }
        }
        //array.SetTransforms(activeEnemiesTransforms.ToArray()); // = new TransformAccessArray(activeEnemiesTransforms.ToArray());
        return result;
    }
    [Range(1f, 100f)]
    [SerializeField] private float enemuToFramePersent;
    [SerializeField] private int enemuToFrame; // Количество индексов которые будут проходится в 1 фрейм
    [SerializeField] private int enemuLast = 0;
    [SerializeField] private int enemuCurrent = 0;


    public void SwitchSpeedMultipler(float multipler)
    {
        allSpeedMultipler = multipler;
    }
    public void RemoveSpeedMultipler()
    {
        allSpeedMultipler = defaultSpeedMultipler;
    }
    private void Update()
    {
        if (activeEnemies.Count > 0)
        {

            enemuToFrame = dynamicDatas.Count == 0 ? 0 : Mathf.Clamp(Mathf.RoundToInt(dynamicDatas.Count / 100f * (enemuToFramePersent * LevelManager.Instance.GameSpeed)), 1, 100) + 5;

            int t = enemuLast;
            int r = Mathf.Clamp(t + enemuToFrame, 0, dynamicDatas.Count);
            for (int i = t; i < r; i++)
            {

                if (dynamicDatas[i] != null && dynamicDatas[i].enemu.gameObject.activeInHierarchy)
                {
                    MoveEnemuData md = moveDatas[i];



                    int max = dynamicDatas[i].enemu.CurrentSpawner.GetPath().Length;
                    //dynamicDatas[i].enemu.SetDistionation
                    md.RotateSpeed = dynamicDatas[i].enemu.RotateSpeed / LevelManager.Instance.GameSpeed;
                    bool chance = Random.Range(0, 100) < 5;
                    md.RotateOffset = chance ? dynamicDatas[i].enemu.RotateOffset + Random.Range(-10, 10) : dynamicDatas[i].enemu.RotateOffset;
                    md.noWaitRotate = false;
                    md.blockJobMove = dynamicDatas[i].enemu.blockJobMove;

                    if (dynamicDatas[i].enemu.target && dynamicDatas[i].enemu.target.gameObject.activeInHierarchy)
                    {
                        md.targetPosition = dynamicDatas[i].enemu.target.transform.position;
                    }
                    else
                    {
                        md.targetPosition = Vector3.zero;
                    }

                    Vector3 nextPosition = dynamicDatas[i].enemu.CurrentSpawner.GetPath()[Mathf.Clamp(dynamicDatas[i].enemu.PositionIndex, 0, dynamicDatas[i].enemu.CurrentSpawner.GetPath().Length - 1)];
                    float currentDistance = Vector2.Distance(dynamicDatas[i].enemu.transform.position, nextPosition);
                    bool bug = false;

                    float speed = (dynamicDatas[i].enemu.Speed * Random.Range(0.5f, 1f)) * LevelManager.Instance.GameSpeed * allSpeedMultipler;

                    md.Speed = speed;
                    if (md.currentFramesCount >= md.maxFramesCountToSaveLastDistance)
                    {
                        // ???????? ?? ?????????
                        if (math.distance(dynamicDatas[i].enemu.transform.position, md.lastPosition) < (md.Speed * 0.0001f) && dynamicDatas[i].enemu.PositionIndex < max)// (md.Speed * 0.0001f))
                        {
                            dynamicDatas[i].enemu.Agent.avoidancePriority--;
                            bug = true;
                            md.noWaitRotate = true;
                            nextPosition = dynamicDatas[i].enemu.CurrentSpawner.GetPath()[Mathf.Clamp(dynamicDatas[i].enemu.PositionIndex, 0, dynamicDatas[i].enemu.CurrentSpawner.GetPath().Length - 1)];
                        }
                        md.lastPosition = dynamicDatas[i].enemu.transform.position;
                        md.currentFramesCount = 0;
                    }
                    if (currentDistance <= md.DistanceToComplete && !bug)
                    {
                        if (dynamicDatas[i].enemu.PositionIndex == max)
                        {
                            DiedEnemu(dynamicDatas[i].enemu, true);
                            continue;
                        }
                        else
                        {
                            dynamicDatas[i].enemu.PositionIndex = Mathf.Clamp(dynamicDatas[i].enemu.PositionIndex + 1, 0, max);
                        }
                    }
                    md.NextPosition = nextPosition;

                    md.ColliderRadius = dynamicDatas[i].enemu.Agent.radius;
                    md.pose = pose;
                    md.currentFramesCount++;

                    allPositions[i] = dynamicDatas[i].enemu.transform.position;
                    moveDatas[i] = md;
                    dynamicDatas[i].enemu.Tick();
                }
                t++;
            }
            enemuLast = t;
            if (enemuLast >= dynamicDatas.Count)
            {
                enemuLast = 0;
            }
            enemuMover = new EnemuMover()
            {
                moveData = moveDatas
            };
            hitsRotator = new HitsRotator()
            {
                target = Camera.main.ScreenToWorldPoint(rotateTARGET.position)
            };
            JobHandle rotateHandle = hitsRotator.Schedule(hitsArray);
            JobHandle moveHandle = enemuMover.Schedule(array);
            moveHandle.Complete();
            rotateHandle.Complete();
        }
    }

    public List<Enemu> GetAllEnemies()
    {
        List<Enemu> enemies = new List<Enemu>();
        foreach (DynamicMoveData d in dynamicDatas)
        {
            enemies.Add(d.enemu);
        }
        return enemies;
    }

    private TransformAccessArray array, hitsArray;

    private void OnDestroy()
    {
        if (allPositions.IsCreated)
            allPositions.Dispose();
        if (moveDatas.IsCreated)
            moveDatas.Dispose();
        if (array.isCreated)
            array.Dispose();
        if (hitsArray.isCreated)
            hitsArray.Dispose();
    }


    public void DiedEnemu(Enemu enemu, bool reach)
    {
        if (enemu)
        {
            if (enemu.CurrentSpawner != null)
            {
                int index = spawners.IndexOf(enemu.CurrentSpawner);
                spawnersIndex[index]--;
            }

            if (activeEnemies.Contains(enemu))
            {
                activeEnemies.Remove(enemu);
            }
            enemu.Kill(reach);
        }
    }
    private void OnDrawGizmos()
    {
        defaultSpeedMultipler = allSpeedMultipler;
    }
    private void Awake()
    {

        spawners = new List<Spawner>();
        spawnersIndex = new List<int>();
        activeEnemiesTransforms = new List<Transform>();
        List<Transform> temp = new List<Transform>();
        List<Transform> enemuHits = new List<Transform>();
        for (int i = 0; i < enemuVariants.Count; i++)
        {
            EnemuVariant enemuVariant = enemuVariants[i];
            if (enemuVariant.variant)
            {
                GameObject p = new GameObject("Thread: " + i);
                p.transform.SetParent(spawnParent);
                for (int j = 0; j < enemuVariant.batchCount; j++)
                {
                    Enemu result = Instantiate(enemuVariant.variant, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, p.transform);
                    result.gameObject.SetActive(false);
                    temp.Add(result.transform);
                    enemuHits.Add(result.Hit.transform);
                    dynamicDatas.Add(new DynamicMoveData()
                    {

                        enemu = result

                    });
                }
            }
        }
        array = new TransformAccessArray(temp.ToArray());
        hitsArray = new TransformAccessArray(enemuHits.ToArray());
        moveDatas = new NativeArray<MoveEnemuData>(dynamicDatas.Count, Allocator.Persistent);
        allPositions = new NativeArray<Vector3>(dynamicDatas.Count, Allocator.Persistent);
        //StartCoroutine(Life());
    }
    public Transform rotateTARGET;
}


[System.Serializable]
public class DynamicMoveData
{
    public Enemu enemu;
}

[System.Serializable]
public struct MoveEnemuData
{
    public float pose;
    public float scalePersent;
    public int groupID, groupIndex;

    public float ColliderRadius;

    public float RotateOffset;
    public float RotateSpeed;
    public float Speed;


    public bool blockJobMove;
    public bool noWaitRotate;


    public Vector3 targetPosition;
    public Vector3 NextPosition;
    public Vector3 lastPosition;
    public int maxFramesCountToSaveLastDistance;
    public int currentFramesCount;
    public float DistanceToComplete;
}

public struct HitsRotator : IJobParallelForTransform
{
    public Vector3 target;
    public void Execute(int index, TransformAccess transform)
    {
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg + 90;//
        angle = math.round(angle);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
    }
}


[BurstCompile]
public struct EnemuMover : IJobParallelForTransform
{
    [ReadOnly]
    public NativeArray<MoveEnemuData> moveData;

    public void Execute(int index, TransformAccess transform)
    {




        float currentDistance = math.distance(transform.position, moveData[index].NextPosition);
        float lerpStep = 0.005f / currentDistance;
        //float offset = moveData[index].Speed * 0.3f;
        float moveSpeed = moveData[index].Speed * lerpStep; // + Random.Range(, moveData[index].Speed / 4);
        //Vector3 nextPostion = Vector3.Lerp(transform.position, moveData[index].NextPosition, (moveData[index].Speed * lerpStep) + 0.01f);
        Vector3 result = moveData[index].NextPosition;

        if (!moveData[index].noWaitRotate)
        {
            result = moveData[index].NextPosition + TransformCorrector(moveData[index].groupIndex, moveData[index].pose, moveData[index].scalePersent);
        }

        Quaternion total = transform.rotation;
        bool fastRotate = false;
        bool next = false;
        if (moveData[index].targetPosition != Vector3.zero)
        {
            fastRotate = true;
            result = moveData[index].targetPosition;
        }





        float angle = Mathf.Atan2(result.y - transform.position.y, result.x - transform.position.x) * Mathf.Rad2Deg - moveData[index].RotateOffset;
        angle = math.round(angle);
        total = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
        if (!fastRotate)
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, total, moveData[index].RotateSpeed);
        else transform.rotation = total;



        if (math.distance(total.z, transform.rotation.z) <= 0.1f)
        {
            next = true;
        }

        if (moveData[index].blockJobMove) return;

        if (!moveData[index].noWaitRotate)
        {
            transform.position = Vector3.Lerp(transform.position, result, moveSpeed);
        }
        else
        {
            if (next)
            {

                transform.position = Vector3.Lerp(transform.position, result, moveSpeed);

                // Лягушачие прыжки
                // transform.position = Vector3.Lerp(transform.position, result, moveData[index].Speed / lerpStep);
                // Телепортация
                //  transform.position = Vector3.Lerp(transform.position, result, lerpStep / moveData[index].Speed);
            }
            else
            {
                //transform.position = Vector3.Lerp(transform.position, result, moveData[index].Speed / 2f);
            }
        }

    }

    /// <summary>
    /// Двигает группу юнитов к цели
    /// </summary>
    /// <param name="center"></param>
    /// <param name="array"></param>
    public Vector3 TransformCorrector(int index, float pose, float scalePersent)
    {
        Vector3 result = Vector3.zero;

        //center = Vector3.ClampMagnitude(center, index);
        for (int i = 0; i < index; i++)
        {
            float theta = i * Mathf.Rad2Deg * (pose % 5f);//Poses[Random.Range(0, Poses.Length)];
            float r = scalePersent;// * Mathf.Sqrt(i);
            result = new Vector3(math.clamp(Mathf.Cos(theta) * r, 0, r), math.clamp(Mathf.Sin(theta) * r, 0, r));

        }
        return result;
    }

    /// <summary>
    /// Двигает группу юнитов к цели
    /// </summary>
    /// <param name="center"></param>
    /// <param name="array"></param>
    public void TransformCorrector(Vector3 center, EntityUnit[] array, float pose, float scalePersent)
    {
        if (array == null)
            return;
        int length = array.Length;
        if (length <= 0)
            return;
        //float pose = 0.01465452f + Random.Range(0.001f, 0.01f);// Mathf.Sin(Random.Range(0.00f, 360.00f)) * Mathf.Cos(Random.Range(0.00f, 360.00f));
        for (int i = 0; i < length; i++)
        {
            float theta = i * Mathf.Rad2Deg * (pose % 5f);//Poses[Random.Range(0, Poses.Length)];
            float r = scalePersent * Mathf.Sqrt(i);
            Vector3 result = center + new Vector3(Mathf.Cos(theta) * r, Mathf.Sin(theta) * r, 0);
            EntityUnit entity = array[i];
            if (entity)
            {
                entity.transform.position = new Vector3(result.x, result.y, 0);
                // entity.currentMovePosition = result;
            }

        }
    }

}