using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LoadPanel loadPanel;

    public void CloseAllPage()
    {
        UIPage[] pages = GameObject.FindObjectsOfType<UIPage>();
        foreach (UIPage page in pages)
        {
            ClosePage(page);
        }
    }

    public void OpenPage(UIPage page, System.Action onComplete)
    {
        if (!page) onComplete();
        page.gameObject.SetActive(true);
        page.transform.DOScale(page.animationData.openValue, page.animationData.duration).SetEase(page.animationData.easeType).OnComplete(() =>
        {
            if (page.gameObject)
                onComplete();
        });
    }

    public void ClosePage(UIPage page, System.Action onComplete)
    {
        if (!page) onComplete();
        page.transform.DOScale(page.animationData.closeValue, page.animationData.duration).SetEase(page.animationData.easeType).OnComplete(() =>
        {
            page.gameObject.SetActive(false);
            if (page.gameObject)
                onComplete();
        });
    }
    public void OpenPage(UIPage page)
    {
        page.gameObject.SetActive(true);
        page.transform.DOScale(page.animationData.openValue, page.animationData.duration).SetEase(page.animationData.easeType).OnComplete(() =>
        {

        });
    }

    public void ClosePage(UIPage page)
    {
        page.transform.DOScale(page.animationData.closeValue, page.animationData.duration).SetEase(page.animationData.easeType).OnComplete(() =>
        {
            page.gameObject.SetActive(false);

        });
    }
}
