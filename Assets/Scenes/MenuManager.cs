
using UnityEngine;
public class MenuManager : MonoBehaviour
{

    [SerializeField] private LevelPressetReader levelPressetReader;

    private void Awake()
    {
        levelPressetReader.EnemuType.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.EnemuType = (EnemuType)v;
        });
        levelPressetReader.EdificeType.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.EdificeType = (EdificeType)v;
        });
        levelPressetReader.BalanceType.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.BalanceType = (BalanceType)v;
        });
        levelPressetReader.RareType.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.RareType = (RareType)v;
        });
        levelPressetReader.SfxToggle.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.UseSFX = v;
        });
        levelPressetReader.MusicToggle.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.UseMusic = v;
        });
        levelPressetReader.UseShadow.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.UseShadow = v;
        });
        levelPressetReader.Sensitivity.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.Sensitivity = v;
        });
        levelPressetReader.UseLight.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.UseLight = v;
        });
        levelPressetReader.UseAddtiveLight.onValueChanged.AddListener((v) =>
        {
            LevelData.levelPresset.UseAddtiveLight = v;
        });
    }


    public void OpenPage(UIPage page)
    {
        GameManager.Instance.UiManager.OpenPage(page);
        Save();
    }
    private void Save()
    {
        levelPressetReader.MusicToggle.isOn = LevelData.levelPresset.UseMusic;
        levelPressetReader.SfxToggle.isOn = LevelData.levelPresset.UseSFX;
        levelPressetReader.EdificeType.value = (int)LevelData.levelPresset.EdificeType;
        levelPressetReader.EnemuType.value = (int)LevelData.levelPresset.EnemuType;
        levelPressetReader.BalanceType.value = (int)LevelData.levelPresset.BalanceType;
        levelPressetReader.RareType.value = (int)LevelData.levelPresset.RareType;
        levelPressetReader.UseShadow.isOn = LevelData.levelPresset.UseShadow;
        levelPressetReader.Sensitivity.value = LevelData.levelPresset.Sensitivity;
        levelPressetReader.UseLight.isOn = LevelData.levelPresset.UseLight;
        levelPressetReader.UseAddtiveLight.isOn = LevelData.levelPresset.UseAddtiveLight;
        GameManager.Instance.ChangeLanguage(LevelData.lang);
    }
    public void ClosePage(UIPage page)
    {
        GameManager.Instance.UiManager.ClosePage(page);
        //Save();
    }
}
