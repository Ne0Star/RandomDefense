
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    [SerializeField] private LevelPressetReader levelPressetReader;

    private void Awake()
    {
        if (levelPressetReader.EnemuType)
            levelPressetReader.EnemuType.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.EnemuType = (EnemuType)v;
            });
        if (levelPressetReader.EdificeType)
            levelPressetReader.EdificeType.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.EdificeType = (EdificeType)v;
            });
        if (levelPressetReader.BalanceType)
            levelPressetReader.BalanceType.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.BalanceType = (BalanceType)v;
            });
        if (levelPressetReader.RareType)
            levelPressetReader.RareType.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.RareType = (RareType)v;
            });
        if (levelPressetReader.SfxToggle)
            levelPressetReader.SfxToggle.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.UseSFX = v;
            });
        if (levelPressetReader.MusicToggle)
            levelPressetReader.MusicToggle.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.UseMusic = v;
            });
        if (levelPressetReader.UseShadow)
            levelPressetReader.UseShadow.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.UseShadow = v;
            });
        if (levelPressetReader.Sensitivity)
            levelPressetReader.Sensitivity.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.Sensitivity = v;
            });
        if (levelPressetReader.UseLight)
            levelPressetReader.UseLight.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.UseLight = v;
            });
        if (levelPressetReader.UseAddtiveLight)
            levelPressetReader.UseAddtiveLight.onValueChanged.AddListener((v) =>
            {
                LevelData.levelPresset.UseAddtiveLight = v;
            });
        if (levelPressetReader.Lang)
            levelPressetReader.Lang.onValueChanged.AddListener((v) =>
            {
                GameManager.Instance.ChangeLanguage(v);
            });
    }

    private void OnEnable()
    {
        Save();
    }
    private void OnDisable()
    {
        Save();
    }
    private void Save()
    {
        if (levelPressetReader.MusicToggle)
            levelPressetReader.MusicToggle.isOn = LevelData.levelPresset.UseMusic;
        if (levelPressetReader.SfxToggle)
            levelPressetReader.SfxToggle.isOn = LevelData.levelPresset.UseSFX;
        if (levelPressetReader.EdificeType)
            levelPressetReader.EdificeType.value = (int)LevelData.levelPresset.EdificeType;
        if (levelPressetReader.EnemuType)
            levelPressetReader.EnemuType.value = (int)LevelData.levelPresset.EnemuType;
        if (levelPressetReader.BalanceType)
            levelPressetReader.BalanceType.value = (int)LevelData.levelPresset.BalanceType;
        if (levelPressetReader.RareType)
            levelPressetReader.RareType.value = (int)LevelData.levelPresset.RareType;
        if (levelPressetReader.UseShadow)
            levelPressetReader.UseShadow.isOn = LevelData.levelPresset.UseShadow;
        if (levelPressetReader.Sensitivity)
            levelPressetReader.Sensitivity.value = LevelData.levelPresset.Sensitivity;
        if (levelPressetReader.UseLight)
            levelPressetReader.UseLight.isOn = LevelData.levelPresset.UseLight;
        if (levelPressetReader.UseAddtiveLight)
            levelPressetReader.UseAddtiveLight.isOn = LevelData.levelPresset.UseAddtiveLight;
        if (levelPressetReader.Lang)
            levelPressetReader.Lang.value = LevelData.lang;
        //GameManager.Instance.ChangeLanguage(LevelData.lang);
    }
}
