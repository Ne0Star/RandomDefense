
using UnityEngine;
using TMPro;
using YG;

public class LevelResult : MonoBehaviour
{
    [SerializeField] private Transform[] banners;
    [SerializeField] private TMP_Text totalValue, diedEdifice, killEnemu;
    public int DiedEdifice;
    public int KillEnemu;

    void Start()
    {

    }

    private void OnEnable()
    {
        foreach (Transform banner in banners)
            banner.gameObject.SetActive(true);
    }

    public void UpdateDisplay()
    {
        diedEdifice.text = " " + (1.5f * (DiedEdifice + 1));
        killEnemu.text = " " + (5.5f * (KillEnemu + 1));
        totalValue.text = "" + Mathf.RoundToInt(((1.5f * (DiedEdifice + 1)) * (5.5f * (KillEnemu + 1))) * 2);
        SaveResult(Mathf.RoundToInt(((1.5f * (DiedEdifice + 1)) * (5.5f * (KillEnemu + 1))) * 0.75f));
    }

    private void SaveResult(int total)
    {
        var lb = "lida";
        if (total > YandexGame.savesData.lastRecoord)
        {
            YandexGame.NewLeaderboardScores(lb, total);
            YandexGame.savesData.lastRecoord = total;
            YandexGame.SaveProgress();
        }


        //Bridge.leaderboard.GetScore((success, score) =>
        //{
        //    if (success)
        //    {
        //        if (score < total)
        //        {
        //            Bridge.leaderboard.SetScore((s) =>
        //            {
        //                if (s)
        //                {

        //                }
        //                else
        //                {
        //                    Debug.Log("�� ������� ���������� ���������");
        //                }
        //            }, new SetScoreYandexOptions(total, ln));

        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("�� ������� �������� ���������");
        //    }
        //}, new GetScoreYandexOptions(ln));
    }
}
