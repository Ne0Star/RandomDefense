using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct BYU_Dealer
{
    
    [SerializeField] private float energy, gold, component;
    [SerializeField] private TMP_Text g, e, c;
    public TMP_Text Component_Text { get => c; }
    public TMP_Text Energy_Text { get => e; }
    public TMP_Text Gold_Text { get => g; }
    public float Energy { get => energy; set => energy = value; }
    public float Gold { get => gold; set => gold = value; }
    public float Component { get => component; set => component = value; }

    public void UpdateData()
    {
        Component_Text.text = component.ToString();
        Gold_Text.text = gold.ToString();
        Energy_Text.text = energy.ToString();
    }

}

public class TapeDragItem : TapeItem, IPointerDownHandler
{
    [SerializeField] private OnDown ondown;
    [SerializeField] private Color moveColor;
    [SerializeField] private TMP_Text gold, energy, component;

    [System.Serializable]
    public class OnDown : UnityEngine.Events.UnityEvent<TapeDragItem, ByuDealer> { }
    public TMP_Text Component_Text { get => component; }
    public TMP_Text Energy_Text { get => energy; }
    public TMP_Text Gold_Text { get => gold; }

    public OnDown onDown => ondown;
    public void SetShadowColor(Color color) => moveColor = color;

    private void Start()
    {
        SetCurrentItem(currentItem);
    }

    public void OnPointerDown(PointerEventData data)
    {
        ondown?.Invoke(this, currentItem);
    }

    public override void SetCurrentItem(ByuDealer item)
    {
        if (!item) return;
        contentImg.enabled = false;
        //contentImage.sprite = item.GetSprite();
        if (item.GetImageBG())
        {
            contentImgBG.sprite = item.GetImageBG();
        }
        if (item.GetImage())
        {
            contentImg.enabled = true;
            contentImg.sprite = item.GetImage();
        }

        gold.text = item.Prefab.CostData.Gold + "";
        energy.text = item.Prefab.CostData.Energy + "";
        component.text = item.Prefab.CostData.Component + "";
        currentItem = item;
    }
}
