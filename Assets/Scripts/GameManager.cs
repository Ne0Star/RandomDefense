using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public static class LevelData
{
    public static int lang;
    public static bool useSFX;
    public static bool useMusic;
    public static LevelPresset levelPresset;

    //public static void UpdateText(this Text text)
    //{
    //    text.gameObject.SetActive(false);
    //    text.text = LangsList.GetWord(text.gameObject.name);
    //    text.GraphicUpdateComplete();
    //    text.gameObject.SetActive(true);
    //}
    //public static void UpdateText(this TMP_Text text)
    //{
    //    text.gameObject.SetActive(false);
    //    text.text = LangsList.GetWord(text.gameObject.name);
    //    text.GraphicUpdateComplete();
    //    text.gameObject.SetActive(true);
    //}
}

public class GameManager : OneSingleton<GameManager>
{
    [System.Serializable]
    public class OnChangeLanguage : UnityEngine.Events.UnityEvent<int> { }
    [SerializeField] private TMP_Text scroboardText;
    [SerializeField] private Transform[] disableBanners;
    [SerializeField] private bool setDefaultPresset;
    [SerializeField] private LevelPresset levelPresset;
    [SerializeField] private bool updateTexts = false;
    [SerializeField] private OnChangeLanguage onChangeLanguage;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private UIPage loadPage;
    [SerializeField] private TMP_Text loadText;
    [SerializeField] private YandexGame yg;
    public UIManager UiManager { get => uiManager; }
    public LevelPresset LevelPresset { get => levelPresset; }

    public void SwitchScene(int index)
    {
        StartCoroutine(OpenSceneAsyncSingle(index));
    }
    public IEnumerator OpenSceneAsyncSingle(int index)
    {
        AsyncOperation time = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        Scene scene = gameObject.scene;
        if (loadPage) uiManager.OpenPage(loadPage);
        while (!time.isDone)
        {
            if (loadText)
            {
                float progress = Mathf.Clamp01(time.progress / 1.05f);
                loadText.text = progress + "";
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void Awake()
    {
        GameManager.Instance = this;
        yg = GameObject.FindObjectOfType<YandexGame>();

        if (setDefaultPresset)
        {
            LevelData.levelPresset = levelPresset;
        }
    }

    public void GetLoad()
    {
        if (scroboardText)
        {
            scroboardText.text = "";
            scroboardText.text = YandexGame.savesData.lastRecoord + "";
        }

    }

#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            yg._FullscreenShow();
        }
    }
#endif
    private IEnumerator Start()
    {
        foreach (Transform t in disableBanners)
        {
            t.gameObject.SetActive(false);
        }
#if UNITY_EDITOR
        if (YandexGame.SDKEnabled)
        {
            GetLoad();
        }
        yield break;
#else
        yield return new WaitUntil(() => YandexGame.SDKEnabled);
            GetLoad();
#endif
    }

    public void ChangeLanguage(int index)
    {
        return;
        ////if (LevelData.lang == index) return;
        //LevelData.lang = index;
        //LangsList.SetLanguage(index);

        //TMP_Text[] texsts = FindObjectsOfType<TMP_Text>(true);
        //Text[] texts_ = FindObjectsOfType<Text>(true);
        //DropDownLocalizer[] drops = FindObjectsOfType<DropDownLocalizer>(true);
        //foreach (TMP_Text text in texsts)
        //{
        //    if (!text.text.Contains("null") && !text.gameObject.name.Contains("null"))
        //        text.UpdateText();
        //}
        //foreach (Text text in texts_)
        //{
        //    if (!text.text.Contains("null") && !text.gameObject.name.Contains("null"))
        //        text.UpdateText();
        //}
        //foreach (DropDownLocalizer drop in drops)
        //{
        //    drop.UpdateTexts();
        //}
        //onChangeLanguage?.Invoke(index);
    }

}
