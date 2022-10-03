using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using YG;

public static class StaticData
{
    public static float TriggerUpdateTime = 0.2f;
}

[System.Serializable]
public class LevelInitialData
{
    public Text MaxWave_Text;
    public Text CurrentWave_Text;

    public Text MaxLife_Text;
    public Text CurrentLife_Text;

    [SerializeField]
    private int maxWave;
    [SerializeField]
    private int maxLife;
    [SerializeField]
    private int currentWave;
    [SerializeField]
    private int currentLife;


    // Формула просчёта общего кол-ва бонусов за прохождение уровня
    // Убийство врага добавляет 10 баллов
    // Количество возведённых построек добавляет 10 баллов
    // 
    // Количество убитых * (количество возведённых построек - потерянные постройки) * кол-во пройденных волн
    //


    public void CalculateTotalBonus(int endWave)
    {
        float killResult = totalKillEnemy * 10;
        float buildingResult = totalSpawnEdifice - totalKillEdifice;
        float result = killResult * buildingResult * endWave;
    }

    /// <summary>
    /// Общее кол-во разрушенных построек
    /// </summary>
    private int totalKillEdifice;

    /// <summary>
    /// Общее кол-во убитых врагов
    /// </summary>
    private int totalKillEnemy;

    /// <summary>
    /// Общее кол-во было возведенных построек
    /// </summary>
    private int totalSpawnEdifice;

    /// <summary>
    /// Общее кол-во сколько раз был использавана экстренная жизнь
    /// </summary>
    private int totalHealthLife;

    public int MaxWave { get => maxWave; }
    public int MaxLife { get => maxLife; }
    public int CurrentWave { get => currentWave; set => SetCurrentWave(value); }
    public int CurrentLife { get => currentLife; set => SetCurrentLife(value); }

    private void SetCurrentWave(int value)
    {
        currentWave = value;
        CurrentWave_Text.text = value.ToString();
        MaxWave_Text.text = maxWave.ToString();
    }
    private void SetCurrentLife(int value)
    {
        currentLife = value;
        CurrentLife_Text.text = value.ToString();
    }

}

public struct Setting
{

    int fps;
    int langKey;
    bool useSFX;
    bool useMusic;

    public int Fps { get => fps; set => fps = value; }
    public int LangKey { get => langKey; set => langKey = value; }
    public bool UseSFX { get => useSFX; set => useSFX = value; }
    public bool UseMusic { get => useMusic; set => useMusic = value; }
}



/// <summary>
/// Определяет количество ресурсов при старте
/// </summary>
public enum BalanceType
{
    /// <summary>
    ///  Очень мало ресурсов
    /// </summary>
    Type_0,
    /// <summary>
    /// Стандартное кол-во ресурсов
    /// </summary>
    Type_1,
    /// <summary>
    /// Много ресурсов
    /// </summary>
    Type_2,
    /// <summary>
    /// Очень много ресурсов
    /// </summary>
    Type_3
}
/// <summary>
/// Определяет редкость
/// </summary>
public enum RareType
{
    /// <summary>
    /// Заниженная редкость
    /// </summary>
    Type_0,
    /// <summary>
    /// Стандартная редкость
    /// </summary>
    Type_1,
    /// <summary>
    /// Завышенная редкость
    /// </summary>
    Type_2,
    /// <summary>
    /// Отключить редкость
    /// </summary>
    Type_3
}
/// <summary>
/// Определяет силу врагов
/// </summary>
public enum EnemuType
{
    /// <summary>
    /// Заниженные характеристики
    /// </summary>
    Type_0,
    /// <summary>
    /// Стандартные характеристики
    /// </summary>
    Type_1,
    /// <summary>
    /// Завышенные характеристики
    /// </summary>
    Type_2,
    /// <summary>
    /// Сильно завышенные характеристики
    /// </summary>
    Type_3
}
/// <summary>
/// Определяет силу и прочность построек
/// </summary>
public enum EdificeType
{
    /// <summary>
    /// Заниженные характеристики
    /// </summary>
    Type_0,
    /// <summary>
    /// Стандартные характеристики
    /// </summary>
    Type_1,
    /// <summary>
    /// Завышенные характеристики
    /// </summary>
    Type_2,
    /// <summary>
    /// Сильно завышенные характеристики
    /// </summary>
    Type_3
}

[System.Serializable]
public struct LevelPresset
{
    [SerializeField] private float treeMultipler;
    [SerializeField] private bool useSFX;
    [SerializeField] private bool useMusic;
    [SerializeField] private bool useShadow;
    [SerializeField] private bool useLight;
    [SerializeField] private bool useAddtiveLight;
    [SerializeField] private float sensitivity;
    [SerializeField] private BalanceType balanceType;
    [SerializeField] private RareType rareType;
    [SerializeField] private EnemuType enemuType;
    [SerializeField] private EdificeType edificeType;

    public BalanceType BalanceType { get => balanceType; set => balanceType = value; }
    public RareType RareType { get => rareType; set => rareType = value; }
    public EnemuType EnemuType { get => enemuType; set => enemuType = value; }
    public EdificeType EdificeType { get => edificeType; set => edificeType = value; }
    public bool UseSFX { get => useSFX; set => useSFX = value; }
    public bool UseMusic { get => useMusic; set => useMusic = value; }
    public bool UseShadow { get => useShadow; set => useShadow = value; }
    public float Sensitivity { get => sensitivity; set => sensitivity = value; }
    public bool UseLight { get => useLight; set => useLight = value; }
    public bool UseAddtiveLight { get => useAddtiveLight; set => useAddtiveLight = value; }
    public float TreeMultipler { get => treeMultipler; set => treeMultipler = value; }
}
[System.Serializable]
public struct LevelPressetReader
{
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle useShadow;
    [SerializeField] private Toggle useLight;
    [SerializeField] private Toggle useAddtiveLight;
    [SerializeField] private TMP_Dropdown balanceType;
    [SerializeField] private TMP_Dropdown rareType;
    [SerializeField] private TMP_Dropdown enemuType;
    [SerializeField] private TMP_Dropdown edificeType;
    [SerializeField] private TMP_Dropdown lang;
    [SerializeField] private Slider sensitivity;
    public TMP_Dropdown BalanceType { get => balanceType; }
    public TMP_Dropdown RareType { get => rareType; }
    public TMP_Dropdown EnemuType { get => enemuType; }
    public TMP_Dropdown EdificeType { get => edificeType; }
    public Toggle MusicToggle { get => musicToggle; }
    public Toggle SfxToggle { get => sfxToggle; }
    public Slider Sensitivity { get => sensitivity; }
    public Toggle UseShadow { get => useShadow; }
    public Toggle UseAddtiveLight { get => useAddtiveLight; }
    public Toggle UseLight { get => useLight; }
    public TMP_Dropdown Lang { get => lang; }
}

/// <summary>
/// Хранит в себе все основные данные (Уровня)
/// </summary>
public class LevelManager : OneSingleton<LevelManager>
{
    [SerializeField] private Transform[] _shadows;
    [SerializeField] private Transform[] _light;
    [SerializeField] private Transform[] _addtiveLight;
    [SerializeField] private LevelResult levelResult;

    [SerializeField] private int initialLife;
    [SerializeField] private int currentLife;

    [System.Serializable]
    public class OnGameSpeed : UnityEngine.Events.UnityEvent<float> { }

    [SerializeField] private OnGameSpeed onGameSpeed;
    [SerializeField] private int speedIndex = 0;

    [SerializeField] private GameSpeedData[] gameSpeeds;

    [System.Serializable]
    private struct GameSpeedData
    {
        public string viewValue;
        public float realValue;
    }


    [SerializeField] private float gameSpeed = 1f;
    public float GameSpeed => gameSpeed;

    [SerializeField] private TMP_Text t_gameSpeed, t_currentLife;


    //public TMP_Text enemuCount;

    //public float updateInterval = 0.5F;
    //private double lastInterval;
    //private int frames;

    //private void Update()
    //{
    //    ++frames;
    //    float timeNow = Time.realtimeSinceStartup;
    //    if (timeNow > lastInterval + updateInterval)
    //    {
    //        fps.text = (float)(frames / (timeNow - lastInterval)) + "";
    //        frames = 0;
    //        lastInterval = timeNow;
    //    }

    //    enemuCount.text = enemuManager.GetAllActiveEnemies().Count + "";
    //}

    public void StopSpeed()
    {
        uiManager.FastOpenPage(pauseMarker);
        isStop = true;
        //gameSpeed = 0f;
        if (enemuManager)
            enemuManager.SwitchSpeedMultipler(0.001f);
        Time.timeScale = 0f;

        //onGameSpeed?.Invoke(gameSpeed);
        //if (t_gameSpeed)
        //    t_gameSpeed.text = "0";
    }

    public void ResumeSpeed()
    {
        uiManager.ClosePage(pauseMarker);
        isStop = false;
        Time.timeScale = 1f;
        if (enemuManager)
            enemuManager.RemoveSpeedMultipler();
    }

    [SerializeField] private UIPage pauseMarker;

    public void ChangeSpeed()
    {
        if (isStop) return;
        Time.timeScale = 1f;
        speedIndex = Mathf.Clamp(speedIndex + 1, 0, gameSpeeds.Length);
        if (speedIndex == gameSpeeds.Length)
        {
            speedIndex = 0;
        }
        gameSpeed = gameSpeeds[speedIndex].realValue;
        onGameSpeed?.Invoke(gameSpeed);
        t_gameSpeed.text = gameSpeeds[speedIndex].viewValue;
    }
    private void OnDrawGizmos()
    {
        if (LevelManager.Instance != this)
        {
            LevelManager.Instance = this;
        }
    }

    public Color useless;
    public Color standart;
    public Color rare;
    public Color legendary;
    public Color epick;
    public Color GetRareColor(Entity target)
    {
        Color result = useless;
        switch (target.CostData.RaresType)
        {
            case CostData.Rares.Ненужный:
                result = useless; break;
            case CostData.Rares.Обычный:
                result = standart; break;
            case CostData.Rares.Редкий:
                result = rare; break;
            case CostData.Rares.Легендарный:
                result = legendary; break;
            case CostData.Rares.Эпический:
                result = epick; break;
        }
        return result;
    }
    public Color GetRareColor(CostData.Rares type)
    {
        Color result = useless;
        switch (type)
        {
            case CostData.Rares.Ненужный:
                result = useless; break;
            case CostData.Rares.Обычный:
                result = standart; break;
            case CostData.Rares.Редкий:
                result = rare; break;
            case CostData.Rares.Легендарный:
                result = legendary; break;
            case CostData.Rares.Эпический:
                result = epick; break;
        }
        return result;
    }
    private bool isStop = false;
    public void ChangePause()
    {
        if (isStop)
        {
            ResumeSpeed();
        }
        else
        {
            StopSpeed();
        }
    }

    public void StartLesson()
    {
        ResumeSpeed();
        lesson.gameObject.SetActive(true);
        uiManager.OpenPage(lesson, () =>
        {
            enemuManager.SwitchSpeedMultipler(0.0001f);
            Debug.Log("1");
            lesson.StartLesson(() =>
            {

                ResumeSpeed();
            });

        });
    }

    [SerializeField] private Transform[] banners;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private ByuManager byuManager;
    [SerializeField] private EdificeManager edificeManager;
    [SerializeField] private EnemuManager enemuManager;
    [SerializeField] private YandexGame yg;
    [SerializeField] private LevelPresset levelPresset;
    public UIManager UiManager { get => uiManager; set => uiManager = value; }
    public MapManager MapManager { get => mapManager; set => mapManager = value; }
    public WaveManager WaveManager { get => waveManager; set => waveManager = value; }
    public ByuManager ByuManager { get => byuManager; set => byuManager = value; }
    public EdificeManager EdificeManager { get => edificeManager; set => edificeManager = value; }
    public EnemuManager EnemuManager { get => enemuManager; set => enemuManager = value; }
    public LevelPresset LevelPresset { get => levelPresset; }
    public LevelResult LevelResult { get => levelResult; set => levelResult = value; }
    public bool IsStop { get => isStop; set => isStop = value; }

    [SerializeField] private Light2D globalLight;
    [SerializeField] private LessonTimeLine lesson;
    private void Awake()
    {
        LevelManager.Instance = this;
        yg = GameObject.FindObjectOfType<YandexGame>();
        //yg._FullscreenShow();
        YandexGame.CloseVideoEvent += CheckReward;
        YandexGame.OpenVideoEvent += OpenReward;
        YandexGame.OpenFullAdEvent += OpenFull;
        YandexGame.CloseFullAdEvent += CloseFull;
        levelPresset = LevelData.levelPresset;
        if (levelPresset.UseShadow)
        {
            foreach (Transform target in _shadows)
            {
                if (target)
                    Destroy(target.gameObject);
            }
        }
        if (levelPresset.UseLight)
        {
            foreach (Transform target in _light)
            {
                if (target)
                    Destroy(target.gameObject);
            }
        }
        if (levelPresset.UseAddtiveLight)
        {
            foreach (Transform target in _addtiveLight)
            {
                if (target)
                    Destroy(target.gameObject);
            }
        }
        if (levelPresset.UseAddtiveLight && levelPresset.UseLight)
        {
            globalLight.color = Color.white;
            globalLight.intensity = 1.1f;
        }
        QualitySettings.vSyncCount = 1;
        edificeManager = FindObjectOfType<EdificeManager>();
        byuManager = FindObjectOfType<ByuManager>();
        if (!uiManager)
            uiManager = FindObjectOfType<UIManager>();
        waveManager = FindObjectOfType<WaveManager>();
        mapManager = FindObjectOfType<MapManager>();
        if (!enemuManager)
            enemuManager = FindObjectOfType<EnemuManager>();
        if (!levelResult)
            levelResult = FindObjectOfType<LevelResult>();
        ChangeSpeed();

        //LevelManager.Instance = this;
    }
    private IEnumerator Start()
    {
        StartCoroutine(WaitFull());
        currentLife = initialLife;
        t_currentLife.text = currentLife.ToString();
        if (!lesson)
            lesson = FindObjectOfType<LessonTimeLine>(true);
        StartLesson();
        yield return new WaitForSeconds(1);
#if UNITY_EDITOR

        Application.targetFrameRate = 60;

#endif
        //WaveManager.StartInifinityWaves();
    }

    private IEnumerator WaitFull()
    {
        yg._FullscreenShow();
        yield return new WaitForSeconds(180f);
        StartCoroutine(WaitFull());
        yield break;
    }

    public void GoMenu()
    {
        GameManager.Instance.SwitchScene(0);
    }

    private void OpenFull()
    {
        StopSpeed();
    }
    private void CloseFull()
    {
        ResumeSpeed();
    }
    private void OpenReward(int id)
    {
        StopSpeed();
    }
    private void CheckReward(int id)
    {
        ResumeSpeed();
        switch (id)
        {
            case 0:
                currentLife += Random.Range(5, 10);
                t_currentLife.text = currentLife + "";
                break;

            case 1:
                byuManager.GetBalance().Component += Random.Range(20,50);
                break;
            case 2:
                byuManager.GetBalance().Gold += Random.Range(100,250);
                break;
            case 3:
                byuManager.GetBalance().Energy += Random.Range(50, 150);
                break;
            case 4:
                //StartCoroutine(WaitBanners());
                break;
            case 5:
                BonusFly bb = GameObject.FindObjectOfType<BonusFly>();
                bb.Fly();
                break;
        }

    }

    private IEnumerator WaitBanners()
    {
        yg._FullscreenShow();
        float lastMin = byuManager.tapeBuilding.Rect.anchorMin.y;
        float lastMax = byuManager.tapeBuilding.Rect.anchorMax.y;
        foreach (Transform t in banners)
        {
            t.gameObject.SetActive(false);
        }
        byuManager.tapeBuilding.Rect.anchorMin = new Vector2(0, 0);
        byuManager.tapeBuilding.Rect.anchorMax = new Vector2(byuManager.tapeBuilding.Rect.anchorMax.x, 1f - lastMin);
        yield return new WaitForSeconds(180f);

        foreach (Transform t in banners)
        {
            t.gameObject.SetActive(true);
        }
        byuManager.tapeBuilding.Rect.anchorMin = new Vector2(0, lastMin);
        byuManager.tapeBuilding.Rect.anchorMax = new Vector2(byuManager.tapeBuilding.Rect.anchorMax.x, lastMax);
    }

    public void ReachEnemu(Enemu target)
    {
        if (currentLife <= 0)
        {
            StopSpeed();
            uiManager.CloseAllPage();
            levelResult.gameObject.SetActive(true);
            levelResult.UpdateDisplay();

            return;
        }
        currentLife--;
        t_currentLife.text = currentLife.ToString();
    }

    /// <summary>
    /// Регистрирует наличие сущности
    /// </summary>
    /// <param name="target">сущность</param>
    public void SpawnEntity(Entity target)
    {
        if (!target) return;


        target.OnDied_.AddListener((e) => DiedEntity(e));
        edificeManager.AllEntity.Add(target);

        if (target as Wall)
        {
            Wall res = (Wall)target;
            edificeManager.AllWalls.Add(res);
            edificeManager.AllUnits.Add(res);
            edificeManager.AllEdifice.Add(res);

            int multipler = ((int)LevelManager.Instance.LevelPresset.EdificeType + 2);
            res.Hit.UpdateHealth(res.Hit.GetHalth() + (multipler * multipler * multipler));
            res.Hit.UpdateStrength(res.Hit.GetStrength() + (multipler * multipler * multipler));

            if (res.Turret)
            {
                edificeManager.AllTurrets.Add(res.Turret);
            }
        }
        else if (target as Generator)
        {
            Generator res = (Generator)target;

            int multipler = ((int)LevelManager.Instance.LevelPresset.EdificeType + 2);
            res.Hit.UpdateHealth(res.Hit.GetHalth() + (multipler * multipler * multipler));
            res.Hit.UpdateStrength(res.Hit.GetStrength() + (multipler * multipler * multipler));

            edificeManager.AllGenerators.Add(res);
            edificeManager.AllEdifice.Add(res);
        }
    }
    /// <summary>
    /// Снимает регистрацию с сущности
    /// </summary>
    /// <param name="target">сущность</param>
    public void DiedEntity(Entity target)
    {
        levelResult.DiedEdifice++;
        edificeManager.AllEntity.Remove(target);
        if (target as Wall)
        {
            Wall res = (Wall)target;
            edificeManager.AllWalls.Remove(res);
            edificeManager.AllUnits.Remove(res);
            edificeManager.AllEdifice.Remove(res);
            if (res.Turret)
            {
                edificeManager.AllTurrets.Remove(res.Turret);
            }
        }
        else if (target as Generator)
        {
            Generator res = (Generator)target;
            edificeManager.AllGenerators.Remove(res);
            edificeManager.AllEdifice.Remove(res);
        }
        else if (target as Turret)
        {
            Turret res = (Turret)target;
            edificeManager.AllTurrets.Remove(res);
        }
    }
}

public static class GameUtils
{
    public static void DealSplashDamage(Vector3 center, EntityUnit[] es, float splashRadius, float splashDamage)
    {
        for (int i = 0; i < es.Length; i++)
        {
            if (es[i])
            {
                float distance = Vector3.Distance(center, es[i].transform.position);
                if (distance <= splashRadius)
                {
                    float percent = Mathf.InverseLerp(splashRadius, 0, distance);
                    es[i].TakeDamage(splashDamage * percent, 0.2f, center);
                }
            }
        }
    }
    /// <summary>
    /// Если передаётся эффект, то его сила будет снижена
    /// </summary>
    /// <param name="center"></param>
    /// <param name="es"></param>
    /// <param name="splashRadius"></param>
    /// <param name="splashDamage"></param>
    /// <param name="attackEffect"></param>
    public static void DealSplashDamage(Vector3 center, EntityUnit[] es, float splashRadius, float splashDamage, AttackEffect attackEffect)
    {
        for (int i = 0; i < es.Length; i++)
        {
            if (es[i])
            {

                float distance = Vector3.Distance(center, es[i].transform.position);
                //if (distance <= splashRadius)
                //{
                float percent = Mathf.InverseLerp(splashRadius, 0, distance);
                AttackEffect ef = new AttackEffect(attackEffect.CurrentEffect, attackEffect.Power * percent);
                es[i].TakeEffect(ef);
                es[i].TakeDamage(splashDamage * percent, 0.2f, center);
                //}
            }
        }
    }
    public static AnimationCurve SetRandomKeys(AnimationCurve origin, float minValue, float maxValue)
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
    public static float CalculateScaleExtents(SpriteRenderer renderer)
    {
        float ScaleX = renderer.bounds.extents.x;
        float ScaleY = renderer.bounds.extents.y;
        float result = ScaleX > ScaleY ? ScaleX : ScaleY;
        return result;
    }
    public static float RoundToValue(float round, float value)
    {
        float result = 0f;
        if (round % value > (value / 2f))
            result = (int)(round / value) * value + value;
        else
            result = (int)(round / value) * value;
        return result;
    }

    /// <summary>
    /// Возвращает true если layer содержится в маске в включённом состоянии
    /// </summary>
    /// <returns></returns>
    public static bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Аналогичен одноимённому методу, имеет минимальный радиус сжатия
    /// </summary>
    /// <param name="v"></param>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }

    public static bool LookAt2DSmooth(Transform whom, Vector3 where, float offset, float time, float distanceToComplete)
    {
        bool result = false;
        float angle = GameUtils.RoundToValue(Mathf.Atan2(where.y - whom.transform.position.y, where.x - whom.transform.position.x) * Mathf.Rad2Deg - offset, 0.05f);
        Quaternion total = Quaternion.Euler(whom.transform.rotation.eulerAngles.x, whom.transform.rotation.eulerAngles.y, angle);
        whom.transform.rotation = Quaternion.Lerp(whom.transform.rotation, total, time);
        if (GameUtils.RoundToValue(Vector2.Distance(new Vector2(total.z, 0), new Vector2(whom.transform.rotation.z, 0)), 0.05f) <= distanceToComplete) result = true;
        return result;
    }
    public static void LookAt2DSmooth(TransformAccess whom, Vector3 where, float offset, float time, float distanceToComplete, System.Action onComplete)
    {
        float angle = GameUtils.RoundToValue(Mathf.Atan2(where.y - whom.position.y, where.x - whom.position.x) * Mathf.Rad2Deg - offset, 0.05f);
        Quaternion total = Quaternion.Euler(whom.rotation.eulerAngles.x, whom.rotation.eulerAngles.y, angle);
        whom.rotation = Quaternion.Lerp(whom.rotation, total, time);
        if (GameUtils.RoundToValue(Vector2.Distance(new Vector2(total.z, 0), new Vector2(whom.rotation.z, 0)), 0.05f) <= distanceToComplete) onComplete();
    }
    public static void LookAt2DSmooth(Transform whom, Vector3 where, float offset, float time, float distanceToComplete, System.Action onComplete)
    {
        float angle = GameUtils.RoundToValue(Mathf.Atan2(where.y - whom.position.y, where.x - whom.position.x) * Mathf.Rad2Deg - offset, 0.05f);
        Quaternion total = Quaternion.Euler(whom.rotation.eulerAngles.x, whom.rotation.eulerAngles.y, angle);
        whom.rotation = Quaternion.Lerp(whom.rotation, total, time);
        if (GameUtils.RoundToValue(Vector2.Distance(new Vector2(total.z, 0), new Vector2(whom.rotation.z, 0)), 0.05f) <= distanceToComplete) onComplete();
    }
    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(Transform whom, Transform where, float offset)
    {
        float angle = Mathf.Atan2(where.transform.position.y - whom.transform.position.y, where.transform.position.x - whom.transform.position.x) * Mathf.Rad2Deg - offset;
        Quaternion total = Quaternion.Euler(whom.transform.rotation.eulerAngles.x, whom.transform.rotation.eulerAngles.y, angle);

        whom.transform.rotation = total;
    }

    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static Quaternion LookAt2DValue(Transform whom, Transform where, float offset)
    {
        if (!whom || !where) return Quaternion.identity;
        float angle = Mathf.Atan2(where.transform.position.y - whom.transform.position.y, where.transform.position.x - whom.transform.position.x) * Mathf.Rad2Deg - offset;
        Quaternion total = Quaternion.Euler(whom.transform.rotation.eulerAngles.x, whom.transform.rotation.eulerAngles.y, angle);
        return total;
    }


    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(Transform whom, Transform where, float offset, float duration)
    {
        float angle = Mathf.Atan2(where.transform.position.y - whom.transform.position.y, where.transform.position.x - whom.transform.position.x) * Mathf.Rad2Deg - offset;
        Quaternion total = Quaternion.Euler(whom.transform.rotation.eulerAngles.x, whom.transform.rotation.eulerAngles.y, angle);
        whom.transform.DORotateQuaternion(total, duration);
        //whom.transform.rotation = total;
    }
    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(Transform whom, Transform where, float offset, float duration, System.Action complete)
    {
        float angle = Mathf.Atan2(where.transform.position.y - whom.transform.position.y, where.transform.position.x - whom.transform.position.x) * Mathf.Rad2Deg - offset;
        Quaternion total = Quaternion.Euler(whom.transform.rotation.eulerAngles.x, whom.transform.rotation.eulerAngles.y, angle);
        whom.transform.DORotateQuaternion(total, duration).OnKill(() => complete());
        //whom.transform.rotation = total;
    }

    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(Rigidbody2D whom, Rigidbody2D where, float offset)
    {
        float angle = Mathf.Atan2(where.transform.position.y - whom.transform.position.y, where.transform.position.x - whom.transform.position.x) * Mathf.Rad2Deg - offset;
        whom.rotation = angle;
    }
    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(TransformAccess whom, Vector2 where, float offset)
    {
        float angle = Mathf.Atan2(where.y - whom.position.y, where.x - whom.position.x) * Mathf.Rad2Deg;
        Quaternion total = Quaternion.Euler(whom.rotation.eulerAngles.x, whom.rotation.eulerAngles.y, angle + offset);

        whom.rotation = total;
    }
    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(Transform whom, Vector2 where, float offset)
    {
        float angle = Mathf.Atan2(where.y - whom.position.y, where.x - whom.position.x) * Mathf.Rad2Deg;
        Quaternion total = Quaternion.Euler(whom.rotation.eulerAngles.x, whom.rotation.eulerAngles.y, angle + offset);

        whom.rotation = total;
    }
    /// <summary>
    /// whom поворачивается в сторону where, с оффестом offset
    /// </summary>
    /// <param name="whom"></param>
    /// <param name="where"></param>
    /// <param name="offset"></param>
    public static void LookAt2D(Transform whom, Vector2 where, float offset, float duration)
    {
        float angle = Mathf.Atan2(where.y - whom.position.y, where.x - whom.position.x) * Mathf.Rad2Deg;
        Quaternion total = Quaternion.Euler(whom.rotation.eulerAngles.x, whom.rotation.eulerAngles.y, angle + offset);
        whom.transform.DORotateQuaternion(total, duration);
    }
}