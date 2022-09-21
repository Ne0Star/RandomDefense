using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int timeToStart;

    [SerializeField] private int waveDuration;
    [SerializeField] private int waveDurationUpdate;

    [SerializeField] private List<Enemu> enemiesVariants;

    [SerializeField] private TMP_Text _enemuCount, _currentWave, _nextWave, _fastText;
    [SerializeField] private Button fastBtn;
    [SerializeField] private Image _fastStart;
    [SerializeField] private bool fastStart;

    [SerializeField] private Wave infinituWave;

    [SerializeField] private int currentWave = 0;
    [SerializeField] private bool wait = false;

    [SerializeField] private GroundSpawner[] groundSpawners;
    [SerializeField] private GameObject[] flyingSpawners, groundInput, flyingInput;
    [SerializeField] private List<Wave> waves;

    //public EntityUnit[] TEST;

    public GroundSpawner[] GroundSpawners { get => groundSpawners; }
    public GameObject[] FlyingSpawners { get => flyingSpawners; }

    public GameObject[] GroundInput { get => groundInput; }
    public GameObject[] FlyingInput { get => flyingInput; }

    public int CurrentWave { get => currentWave; }

    private void Awake()
    {
        //_fastStart.gameObject.SetActive(false);
        //groundSpawners = GameObject.FindObjectsOfType<GroundSpawner>();
        flyingSpawners = GameObject.FindGameObjectsWithTag("Flying Spawner");
        groundInput = GameObject.FindGameObjectsWithTag("Ground Input");
        flyingInput = GameObject.FindGameObjectsWithTag("Flying Input");

    }

    private void Start()
    {

    }

    //public void StartNextWave()
    //{
    //    if (wait) return;
    //    wait = true;

    //    StartCoroutine(WaitWave());
    //}

    public void StartInifinityWaves()
    {
        StartCoroutine(Wait());
    }
    private void OnEnable()
    {
        StartCoroutine(Wait());
    }
    public void StartFast()
    {
        if(!blockFast)
        fastStart = true;



    }
    [SerializeField] private bool blockFast = false;
    private IEnumerator Wait()
    {
        //fastStart = true;
        int temp = 0;
        //_fastText.text = LangsList.GetWord("ready");
        while (temp < timeToStart)
        {
            if (fastStart && LevelManager.Instance.EnemuManager.ActiveEnemiesCount < 100)
            {
                break;
            }
            yield return new WaitForSeconds(1f / LevelManager.Instance.GameSpeed);
            temp += 1;
            timeToNext = (timeToStart - temp);
            UpdateVisual();
            //_nextWave.text = LangsList.GetWord("spawning");

            float t = Mathf.InverseLerp(0, timeToStart, temp);
            _fastStart.fillAmount = (1f - t);
        }
        fastStart = false;
        for (int i = 0; i < infinituWave.WaveUnits.Count; i++)
        {
            infinituWave.WaveUnits[i].CurrentChance = infinituWave.WaveUnits[i].MaxChance;
        }
        StartCoroutine(WaitWaveDuration());
    }

    private void UpdateVisual()
    {
        _nextWave.text = timeToNext.ToString();
        _currentWave.text = currentWave.ToString();
        int enemuCount = LevelManager.Instance.EnemuManager.ActiveEnemiesCount;
        _enemuCount.text = enemuCount.ToString();
        if(enemuCount > 100)
        {
            fastBtn.interactable = false;
        } else
        {
            fastBtn.interactable = true;
        }
    }

    [Header("Улучшение жизней врагов")]
    [SerializeField] private float МинимальноеУлучшение;
    [SerializeField] private float МаксимальноеУлучшение;
    [Header("Награда за волну")]
    [SerializeField] private float ЗолотоЗаВолну;
    [SerializeField] private float ДеталиЗаВолну;
    [Header("Потолок жизней")]
    [SerializeField] private float МинимальноеУлучшение_;
    [SerializeField] private float МаксмальноеУлучшение_;
    [SerializeField] private float ТекущийПотолок = 100f;
    [Header("Спавн враго, не обязательно трогать")]
    [SerializeField] private float spawnDuration;
    [SerializeField] private int timeToNext;
    private IEnumerator WaitWaveDuration()
    {
        //_fastText.text = LangsList.GetWord("startFast");
        currentWave++;

        for (int i = 0; i < currentWave; i++)
        {
            UpdateVisual();
            Spawner spawner = groundSpawners[Random.Range(0, groundSpawners.Length)];
            LevelManager.Instance.EnemuManager.SpawnIntellectually(Random.Range(5, Random.Range(5, Random.Range(0, currentWave / 8))), currentWave, ТекущийПотолок, spawner, МинимальноеУлучшение, МаксимальноеУлучшение);
            yield return new WaitForSeconds(spawnDuration);
            UpdateVisual();
        }
        spawnDuration += 0.01f;
        int temp = 0;
        ТекущийПотолок += Random.Range(МинимальноеУлучшение_, МаксмальноеУлучшение_);


        while (temp < waveDuration)
        {
            if (fastStart && LevelManager.Instance.EnemuManager.ActiveEnemiesCount < 100)
            {
                break;
            }
            yield return new WaitForSeconds(1f / LevelManager.Instance.GameSpeed);
            float t = Mathf.InverseLerp(0, waveDuration, temp);
            _fastStart.fillAmount = (1f - t);

            temp += 1;
            timeToNext = (waveDuration - temp);
            UpdateVisual();
        }
        LevelManager.Instance.ByuManager.GetBalance().Component += ДеталиЗаВолну;
        LevelManager.Instance.ByuManager.GetBalance().Gold += ЗолотоЗаВолну;
        //fastStart = false;
        //if (currentWave % 2 == 0)
        //    waveDuration += waveDurationUpdate;
        StartInifinityWaves();
    }
}
