using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    public string preffix;
    public Text progressText;
    public GameObject progressParent;

    public void Close()
    {
        progressParent.gameObject.SetActive(false);
    }
    public void Open()
    {
        progressParent.gameObject.SetActive(true);
    }
    public void SetProgress(float progress, string suffix)
    {
        progressText.text = preffix + System.Math.Round(progress, 1) + suffix;
    }

}
