//using System;
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public struct MultiValue
{
    public float maxValue;
    public float multipler;
}
[System.Serializable]
public class AttackEffect
{
    [SerializeField] private Effect effect;
    [SerializeField] private float power;
    [SerializeField] private float duration;

    public Effect CurrentEffect { get => effect; }
    public float Power { get => power; }
    public float Duration { get => duration; }

    public enum Effect
    {
        None,
        Freze
    }
}

[System.Serializable]
public struct CostData
{

    [SerializeField] private Rares raresType;
    [SerializeField] private float gold, energy, component, chance;

    public float Component { get => component; set => component = value; }
    public float Energy { get => energy; set => energy = value; }
    public float Gold { get => gold; set => gold = value; }
    public Rares RaresType { get => raresType; }
    public float Chance { get => chance; set => chance = value; }

    public enum Rares
    {
        Ненужный,
        Обычный,
        Редкий,
        Легендарный,
        Эпический
    }

}

/// <summary>
/// Статичная сущность, как правило неразрушаемые объекты, или объекты связщанные с другими сущностями
/// </summary>
public abstract class EntityStatic : Entity
{

}
/// <summary>
/// Любая сущность которая может умирать, имеет хп
/// </summary>
public abstract class EntityUnit : Entity
{
    [SerializeField] protected bool hideHit = true;
    [SerializeField] private HitBar hit;
    public HitBar Hit { get => hit; }
    //[SerializeField] private DiedCallback diedCallback = DiedCallback.Other;
    //private enum DiedCallback
    //{
    //    Enemu,
    //    Other
    //}
    [SerializeField] protected Renderer[] renderers;
    private void Awake()
    {
        if (hit)
            if (hit.gameObject)
            {
                if (hideHit)
                    hit.gameObject.SetActive(false);
            }
        if (renderers == null)
            renderers = gameObject.GetComponentsInChildren<Renderer>();
        AwakeUnit();
    }

    protected virtual void AwakeUnit()
    {

    }

    // Эта сущность получит урон
    public void TakeDamage(float damage, float attackDuration, Vector3 patronPosition)
    {
        if (attackDuration < 0) attackDuration = 0.2f;

        if (hit) hit.TakeDamage(damage, attackDuration, () => CheckDied());
        //if (!blockHit && !hideHit)
        //{
        //    StartCoroutine(WaitHitBar());
        //}
    }

    private void CheckDied()
    {
        if (this as Enemu)
        {
            if (this != null)
                LevelManager.Instance.EnemuManager.DiedEnemu((Enemu)this, false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Приняет эффект к данной сущности
    /// </summary>
    /// <param name="effect"></param>
    public virtual void TakeEffect(AttackEffect effect)
    {

    }

    /// <summary>
    /// Восстанавливает хп сущности
    /// </summary>
    /// <param name="value"></param>
    /// <param name="attackDuration"></param>
    public void TakeHeal(float value, float attackDuration)
    {
        if (attackDuration < 0) attackDuration = 0.2f;

        if (hit) hit.TakeHealth(value, attackDuration);
        if (!blockHit && !hideHit)
        {
            StartCoroutine(WaitHitBar());
        }
    }

    private bool blockHit = false;
    private IEnumerator WaitHitBar()
    {
        blockHit = true;
        hit.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        hit.gameObject.SetActive(false);
        blockHit = false;
    }
}
// Awake
// Spawn
// Died
// Destroy


public abstract class Entity : MonoBehaviour
{

    private void OnValidate()
    {
        if(LevelManager.Instance)
        {
            LevelManager.Instance.ByuManager.Roll();
        }
    }

    #region Events
    [System.Serializable]
    public class OnDied : UnityEngine.Events.UnityEvent<Entity> { }
    [System.Serializable]
    public class OnSpawn : UnityEngine.Events.UnityEvent { }


    [SerializeField] private OnDied onDied;
    [SerializeField] private OnSpawn onSpawn;

    private void OnDestroy()
    {
        onDied?.Invoke(this);
    }

    public OnDied OnDied_ { get => onDied; set => onDied = value; }
    public OnSpawn OnSpawn_ { get => onSpawn; set => onSpawn = value; }
    #endregion

    [SerializeField] protected CostData costData;
    public CostData CostData { get => costData; }

    /// <summary>
    /// Срабатывает когда сущность появляется
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="spawnRotation"></param>
    /// <param name="spawnParent"></param>
    /// <param name="cansel"></param>
    /// <returns></returns>
    public virtual Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, System.Action cansel)
    {
        return null;
    }

    private void OnBecameInvisible()
    {
        OnInvisible();
    }
    private void OnBecameVisible()
    {
        OnVisible();
    }

    protected virtual void OnVisible()
    {

    }
    protected virtual void OnInvisible()
    {

    }

    /// <summary>
    /// Срабатывает каждый тик
    /// </summary>
    public virtual void Tick()
    {

    }
}

[System.Serializable]
public struct MultiplerValue
{
    [SerializeField] private float maxValue;
    [SerializeField] private float recoveryRate;
    [SerializeField] private float multipler;
    [SerializeField] private float minMultipler;
    [SerializeField] private float maxMultipler;

    public float Multipler { get => multipler; set => multipler = Mathf.Clamp(value, minMultipler, maxMultipler); }
    public float RecoveryRate { get => recoveryRate; }

    /// <summary>
    /// Возвращает значение по множителю
    /// </summary>
    /// <returns></returns>
    public float GetCurrentValue()
    {
        return maxValue * multipler;
    }
}


[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemu : EntityUnit
{
    [SerializeField] protected Turret[] turrets;
    //[SerializeField] private float dificityMultipler;
    //[SerializeField] private CostData reachCost;
    [SerializeField] protected StaticEntityTrigger mainTrigger;
    [SerializeField] protected bool attack = false;
    [SerializeField] public EntityUnit target;
    [SerializeField] public bool blockJobMove;
    [SerializeField] public float Speed => speed.GetCurrentValue();
    [SerializeField] public float RotateOffset;
    [SerializeField] public float RotateSpeed;
    [SerializeField] public int PositionIndex = 0;
    [SerializeField] public Spawner CurrentSpawner;

    [SerializeField] private float startHealth, startStrength;

    private void OnDisable()
    {
        isDied = true;
        target = null;
        attack = false;
        if (mainTrigger)
            mainTrigger.enabled = false;
    }
    private void OnEnable()
    {
        if (mainTrigger)
            mainTrigger.enabled = true;
        Hit.SetFull();
        isDied = false;
    }

    public virtual void EnemuUpdate(int currentWave, float minHp, float maxHp)
    {
        if (startHealth == 0f) startHealth = Hit.GetMaxHealth();
        if (startStrength == 0f) startStrength = Hit.GetMaxStrength();
        //costData.Gold += Random.Range(0f, 0.3f);
        //costData.Energy += Random.Range(0.5f, 1f); ;
        //costData.Component = Random.Range(0f, 0.1f);
        // Debug.Log("Подох ");
        //int multipler = ((int)LevelManager.Instance.LevelPresset.EnemuType + 2);
        foreach(Turret t in turrets)
        {
            t.UpdateTurret();
        }
        Hit.SetHealth(startHealth + Random.Range(minHp, maxHp));
        //;Hit.SetStrength(startStrength + Random.Range(minHp, maxHp));
        //dificityMultipler += (0.05f * currentWave);
    }

    /// <summary>
    /// От индекса до остаточно пути пытается найти позицию
    /// если находит возвращает путь до этой позиции
    /// </summary>
    /// <param name="pathIndex"></param>
    /// <returns></returns>
    public Vector3[] GetFixPath(int pathIndex)
    {
        int pl = CurrentSpawner.GetPath().Length;
        if (pathIndex > pl) return null;

        NavMeshPath p = new NavMeshPath();

        Vector3[] result = null;

        for (int i = pathIndex; i < pl; i++)
        {
            Vector3 target = CurrentSpawner.GetPath()[i];

            agent.CalculatePath(target, p);
            if (p.status == NavMeshPathStatus.PathComplete)
            {
                return p.corners;
            }
        }
        return result;
    }
    [SerializeField] private bool isDied = true;
    /// <summary>
    /// if true дошёл до финиша
    /// </summary>
    /// <param name="rearch"></param>
    public virtual void Kill(bool rearch)
    {
        if (isDied) return;
        isDied = true;
        Hit.SetFull();
        if (rearch)
        {
            LevelManager.Instance.ByuManager.GetBalance().BuyItem(ReachCost.Gold, ReachCost.Energy, ReachCost.Component);
            //Debug.Log("Died");
            LevelManager.Instance.ReachEnemu(this);
        }
        else
        {
            LevelManager.Instance.ByuManager.GetBalance().Gold += Random.Range(0, ReachCost.Gold);
            LevelManager.Instance.ByuManager.GetBalance().Component += Random.Range(0, ReachCost.Component);
            LevelManager.Instance.LevelResult.KillEnemu++;
        }

        target = null;
        //transform.localPosition = Vector3.zero;
        blockJobMove = false;
        gameObject.SetActive(false);
    }

    [SerializeField] public Vector3 lastPosition;

    [SerializeField] protected MultiplerValue speed;

    [Header("Системные")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private EnemuType type;

    public EnemuType Type { get => type; }
    public NavMeshAgent Agent { get => agent; }
    public CostData ReachCost { get => costData; set => costData = value; }

    public enum NavType
    {
        Tpye_0, // Идёт сразу к выходу и разрушает всё на своём пути
        Type_1 // Сначала разрушает все постройки а потом идёт к выходу
    }
    public enum EnemuType
    {
        Ground,
        Flying
    }
    public virtual void Rotate()
    {

    }
    public virtual void TurretsTick()
    {

    }

    private void OnBecameInvisible()
    {
        OnInvisible();
    }
    private void OnBecameVisible()
    {
        OnVisible();
    }

    //protected override void OnVisible()
    //{
    //    agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    //}
    //protected override void OnInvisible()
    //{
    //    agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    //}

    public override Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, System.Action cansel)
    {
        CurrentSpawner = spawner;// LevelManager.Instance.WaveManager.GroundSpawners[Random.Range(0, LevelManager.Instance.WaveManager.GroundSpawners.Length)];
        Entity result = LevelManager.Instance.EnemuManager.SpawnEnemu<Enemu>(new Vector3(spawnPosition.x, spawnPosition.y, 0), spawner);
        if (!result) { cansel(); return null; };
        Enemu res = (Enemu)result;
        //res.agent.Warp(spawnPosition);
        if (result)
        {
            return result;
        }
        else
        {
            cansel();
            return null;
        }
    }
    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}
// Перед выпуском вынести всё общее между ними сюда, и добавить вращение
public abstract class Turret : EntityStatic
{
    [SerializeField] protected TurretRadius radius;
    [SerializeField] protected bool weaponMode = false;
    [SerializeField] protected EntityUnit target;
    [SerializeField] protected List<EntityUnit> targets;

    public void UpdateTurret()
    {

    }
    public EntityUnit Target { get => target; set => target = value; }
    public List<EntityUnit> Targets { get => targets; set => targets = value; }
    public bool WeaponMode { get => weaponMode; set => weaponMode = value; }


    /// <summary>
    /// Если у турели есть триггеры они будут выключены
    /// </summary>
    public virtual void DisableAllTriggers()
    {

    }

    public virtual void ShowRadius()
    {

    }
    public virtual void HideRadius()
    {
        radius.Close();
    }

    public virtual float GetTarretRadius()
    {
        return 0f;
    }

    public virtual void CheckTriggerAndAttackTarget()
    {

    }
    /// <summary>
    /// Устанавливает цель, и если она в дистанции то атакует
    /// </summary>
    public virtual void SetTargetAndAttack(EntityUnit target)
    {
        this.target = target;
    }

    public virtual void SetTarget(EntityUnit target)
    {
        this.target = target;
    }

    public virtual void SetTargets(List<EntityUnit> targets)
    {
        this.targets = targets;
    }

    /// <summary>
    /// Приняет эффект к данной сущности
    /// </summary>
    /// <param name="effect"></param>
    public virtual void TakeEffect(AttackEffect effect)
    {

    }

    public override Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, System.Action cansel)
    {
        MapManager map = LevelManager.Instance.MapManager;
        Entity result = null;
        TileData data = map.GetTileData(map.WallMap, spawnPosition);
        if (data != null)
        {
            if (data.Entity_ is Wall)
            {
                Wall wall = (Wall)data.Entity_;
                if (!wall.Turret)
                {
                    Turret res = Instantiate(this, wall.transform);
                    res.HideRadius();

                    wall.SetTurret(res, () => { result = wall; LevelManager.Instance.SpawnEntity(res); }, () => { Debug.Log("Turret: Не удалось создать турель"); });
                }


            }
            else
            {
                result = null;
            }
        }
        return result;
    }
}
/// <summary>
/// Класс построек
/// </summary>
public abstract class Edifice : EntityUnit
{
    //[SerializeField] public Transform[] accesPoints;
    //[SerializeField] private float accesDistance = 0.25f;
    //[SerializeField] private AccesPriority priority = AccesPriority.RightBottomDownLeft;
    [SerializeField] protected NavMeshObstacle navMeshObstacle;
    // Количество врагов которые взяли себе в качестве цели эту постройку
    private enum AccesPriority
    {
        RightBottomDownLeft
    }

    public NavMeshObstacle NavObstacle => navMeshObstacle;

    protected override void AwakeUnit()
    {
        //if (priority == AccesPriority.RightBottomDownLeft)
        //{
        //    if (accesPoints[0])
        //        accesPoints[0].transform.localPosition = new Vector3(accesDistance, 0, 0);
        //    if (accesPoints[1])
        //        accesPoints[1].transform.localPosition = new Vector3(0, -accesDistance, 0);
        //    if (accesPoints[2])
        //        accesPoints[2].transform.localPosition = new Vector3(0, accesDistance, 0);
        //    if (accesPoints[3])
        //        accesPoints[3].transform.localPosition = new Vector3(-accesDistance, 0, 0);
        //}
    }

    public virtual void DisableNearEnemu()
    {

    }
}
/// <summary>
/// Класс построек которые нельзя разрушать
/// </summary>
public abstract class StaticEdifice : EntityStatic
{

}
