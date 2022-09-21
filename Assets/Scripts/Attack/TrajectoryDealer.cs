using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

/// <summary>
/// Эта структура будет хранить и передавать данные из главного потока
/// </summary>
[System.Serializable]
public class DynamicData
{
    public Transform source;
    public MissilesType missilesType;


    /// <summary>
    /// Время которое пройцдено для снаряда
    /// </summary>
    public float currentTime;
    /// <summary>
    /// Скорость с которым снаряд проходит время
    /// </summary>
    public float currentSpeed;
    /// <summary>
    /// Множитель смещения траектории
    /// </summary>
    public float currentOffsetMultipler;
    /// <summary>
    /// В этом радиусе будет искаться новый враг если текущий враг умрёт
    /// </summary>
    public float searchRadius;
    /// <summary>
    /// Вращение будет происходить с этим отклонением
    /// </summary>
    public float rotateOffset;
    /// <summary>
    /// Когда дистанция будет меньше или равна этой патрон прилетит к цели я
    /// </summary>
    public float distanceToCompleted;
    public Vector3 endPosition;
    public bool pursue, rotateToTarget;
}
public class TrajectoryDealer : MonoBehaviour
{
    [Header("Количество трансформов на одного ребёнка")]
    public int oneDealerPatrons;
    [SerializeField]
    private int maxPatrons;
    [SerializeField]
    private int currentPatrons;
    public bool UseJob;
    public Allocator allocator;
    public JobHandle handles;
    public Mover moveJob;
    public TransformAccessArray accessArray;

    public bool AllowBuild()
    {
        if (currentPatrons == maxPatrons) return false; else return true;
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        DisposeAllTemp();
    }
    /// <summary>
    /// Эта структура отправляется в задачу
    /// </summary>
    [System.Serializable]
    public struct Data
    {
        public bool rotateToTarget;
        public float time;
        public float speed;
        public Vector3 offset;
        public float offsetMultipler;
        public float rotateOffset;
        public Vector3 endPosition;

    }
    // будет ли преследовать, если нет то вместо его трансформа создать пустышку на его месте и сделать её целью
    void Awake()
    {
        //maxPatrons = transform.childCount * oneDealerPatrons;
        m_Targets = new Transform[maxPatrons];
        m_Patrons = new Transform[maxPatrons];
        m_OnReached = new System.Action<Transform, Transform>[maxPatrons];
        m_DynamicData = new DynamicData[maxPatrons];
    }

    public Transform[] m_Targets;
    public Transform[] m_Patrons;
    /// <summary>
    /// Патрон, цель, источник
    /// </summary>
    public System.Action<Transform, Transform>[] m_OnReached;
    public DynamicData[] m_DynamicData;
    public NativeArray<Data> tempData;

    public void DisposeAllTemp()
    {
        if (accessArray.isCreated)
            accessArray.Dispose();
        if (tempData.IsCreated)
            tempData.Dispose();
        isCreated = false;
    }
    public void CreateAllTemp()
    {
        accessArray = new TransformAccessArray(m_Patrons);
        tempData = new NativeArray<Data>(m_Patrons.Length, allocator);
        isCreated = true;
    }
    public void RemoveAllList(int i, bool andDispose)
    {
        //Debug.Log(m_Patrons[i] + " " + i);
        //currentPatrons--;

        if (m_DynamicData[i].source)
        {
            m_OnReached[i].Invoke(m_Patrons[i], m_Targets[i]);
        }
        m_OnReached[i] = null;
        m_DynamicData[i] = new DynamicData();
        m_Targets[i] = null;
        m_Patrons[i] = null;
        DisposeAllTemp();
        //
        //currentPatrons--;

    }

    public void CreateAllList(DynamicData d, Transform patron, Transform target, System.Action<Transform, Transform> onComplete)
    {
        if (!AllowBuild()) return;
        for (int i = 0; i < m_Patrons.Length; i++)
        {
            if (!m_Patrons[i])
            {
                StartCoroutine(PatronLife(i));
                m_OnReached[i] = onComplete;
                m_DynamicData[i] = d;
                m_Patrons[i] = patron;
                m_Targets[i] = target;
                currentPatrons++;
                return;
            }
        }
        //
    }

    private IEnumerator PatronLife(int i)
    {
        yield return new WaitUntil(() => (!m_Patrons[i] || !m_Patrons[i].gameObject.activeInHierarchy));
        currentPatrons--;
    }

    private void FixedUpdate()
    {
        //if(currentPatrons == maxPatrons) currentPatrons = 0;
        if (UseJob)
        {
            if (!isCreated)
            {
                return;
            }
            for (int i = 0; i < m_DynamicData.Length; i++)
            {
                if (m_Patrons[i])
                {
                    DynamicData dd = m_DynamicData[i];
                    Data td = tempData[i];

                    // check last data
                    if (Vector2.Distance(m_Patrons[i].position, m_DynamicData[i].endPosition) <= dd.distanceToCompleted || !m_Patrons[i])
                    {
                        RemoveAllList(i, true);
                        CreateAllTemp();
                        return;
                    }
                    // set/update dynamic


                    dd.currentTime += dd.currentSpeed;
                    // set temp
                    td.rotateOffset = dd.rotateOffset;
                    td.rotateToTarget = dd.rotateToTarget;

                    //if (m_Targets[i] && m_Patrons[i])
                    //{
                    //    dd.endPosition = m_Targets[i].position;
                    //}
                    //if (!m_Targets[i] && m_Patrons[i])
                    //{
                    //    dd.endPosition = m_Patrons[i].position;
                    //}

                    if (Vector2.Distance(dd.endPosition, Vector3.zero) < 10f)
                    {
                        if (m_Targets[i] && dd.pursue)
                        {
                            dd.endPosition = m_Targets[i].position;
                        }
                        else if (dd.endPosition == Vector3.zero && !dd.pursue && m_Targets[i])
                        {
                            dd.endPosition = m_Targets[i].position;
                        }
                        else
                        {
                            dd.endPosition = m_Patrons[i].position;
                        }

                    } else
                    {
                        if (m_Targets[i] && dd.pursue)
                        {
                            dd.endPosition = m_Targets[i].position;
                        }
                    }

                    td.endPosition = dd.endPosition;
                    td.offset = new Vector3(dd.missilesType.XOffset.Evaluate(dd.currentTime * dd.currentSpeed), dd.missilesType.YOffset.Evaluate(dd.currentTime * dd.currentSpeed), 0);
                    td.speed = dd.missilesType.SpeedInterpolator.Evaluate(dd.currentTime * dd.currentSpeed);
                    td.time = dd.currentTime;
                    td.offsetMultipler = dd.currentOffsetMultipler;


                    // apply
                    tempData[i] = td; // Данные которые отправятся
                    m_DynamicData[i] = dd;

                }
            }

            moveJob = new Mover
            {
                data = tempData
            };

            handles = moveJob.Schedule(accessArray);
            handles.Complete();
        }
    }


    public void CreatePatron(DynamicData data, Transform patron, Transform target, System.Action<Transform, Transform> onComplete)
    {
        if (!AllowBuild()) return;
        if (data.missilesType.UseRandomCurve)
        {
            data.missilesType.XOffset = data.missilesType.SetRandomKeys(data.missilesType.XOffset, data.missilesType.RandomMinX, data.missilesType.RandomMaxX);
            data.missilesType.YOffset = data.missilesType.SetRandomKeys(data.missilesType.YOffset, data.missilesType.RandomMinY, data.missilesType.RandomMaxY);
        }
        if (UseJob)
        {
            CreateAllList(data, patron, target, onComplete);
            StartCoroutine(ConfigureArray());
        }
        else
        {
            CreateAllList(data, patron, target, onComplete);
        }
    }

    [SerializeField]
    private bool isCreated = false;
    private IEnumerator ConfigureArray()
    {
        if (isCreated)
        {
            DisposeAllTemp();
        }
        CreateAllTemp();
        yield return new WaitUntil(() => accessArray.isCreated);
    }
    [BurstCompile]
    public struct Mover : IJobParallelForTransform
    {
        public NativeArray<Data> data;
        public void Execute(int i, TransformAccess transform)
        {
            Vector2 target = (data[i].endPosition + (data[i].offset * data[i].offsetMultipler));
            float tempSpeed = 0f;

            if (Vector2.Distance(transform.position, data[i].endPosition) < 1f) tempSpeed += 0.01f;
            Vector3 nextPosition = Vector3.Lerp(transform.position, target, data[i].speed + tempSpeed);
            if (data[i].rotateToTarget)
            {
                GameUtils.LookAt2D(transform, nextPosition, data[i].rotateOffset);
                //Debug.Log(data[i].rotateOffset + "");
            }

            transform.position = nextPosition;
        }
    }
}

