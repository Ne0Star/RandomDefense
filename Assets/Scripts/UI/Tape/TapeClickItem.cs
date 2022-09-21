using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ��� ����� ������� �������� ������� ��� �������
/// </summary>
public class TapeClickItem : TapeItem, IPointerClickHandler
{

    [SerializeField] private OnClick onclick;
    [SerializeField] private TMP_Text gold, energy, component;
    public TMP_Text Component_Text { get => component; }
    public TMP_Text Energy_Text { get => energy; }
    public TMP_Text Gold_Text { get => gold; }

    [System.Serializable]
    public class OnClick : UnityEngine.Events.UnityEvent<TapeClickItem, ByuDealer> { }

    public OnClick Onclick { get => onclick; set => onclick = value; }
    private bool blockWarn = false;
    public void IndicateWarning(Color warnColor, float warnDuration)
    {
        if(blockWarn)return;
        blockWarn = true;
        Color lastGoldColor = gold.color;
        Color lastEnergyColor = energy.color;
        Color lastComponentColor = component.color;
        Color lastBgColor = colorBg.color;

        colorBg.DOColor(warnColor, warnDuration).OnComplete(() => {
            colorBg.DOColor(lastBgColor, warnDuration);
        });
        gold.DOColor(warnColor, warnDuration).OnComplete(() => {
            gold.DOColor(lastGoldColor, warnDuration);
        });
        energy.DOColor(warnColor, warnDuration).OnComplete(() => {
            energy.DOColor(lastEnergyColor, warnDuration);
        });
        component.DOColor(warnColor, warnDuration).OnComplete(() => {
            component.DOColor(lastComponentColor, warnDuration + 0.1f).OnKill(() => {
                blockWarn = false;
            });
        });
    }

    public override void SetCurrentItem(ByuDealer item)
    {
        if (item.GetImageBG() && contentImgBG)
            contentImgBG.sprite = item.GetImageBG();
        if (item.GetImage() && contentImg)
            contentImg.sprite = item.GetImage();
        currentItem = item;
        gold.text = currentItem.Prefab.CostData.Gold.ToString();
        energy.text = currentItem.Prefab.CostData.Energy.ToString();
        component.text = currentItem.Prefab.CostData.Component.ToString();
    }
    //public override void SetRandomItem(ByuDealer[] variants)
    //{
    //    int index = Random.Range(0, variants.Length);
    //    SetCurrentItem(variants[index]);
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        onclick.Invoke(this, currentItem);
    }
}
