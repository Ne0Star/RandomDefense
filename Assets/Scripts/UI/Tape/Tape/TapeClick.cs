using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TapeClick : Tape
{

    [System.Serializable]
    public class OnItemClick : UnityEngine.Events.UnityEvent<TapeClickItem, ByuDealer> { }
    [System.Serializable]
    public class OnCansel : UnityEngine.Events.UnityEvent { }

    [SerializeField] private OnItemClick onSelectItem;
    [SerializeField] private OnCansel onCansel;

    [SerializeField] private bool updateContent;
    [SerializeField] private ByuDealer[] tapeContents;
    [SerializeField] private TapeClickItem[] tapeItems;

    public OnItemClick OnSelectItem { get => onSelectItem; set => onSelectItem = value; }
    public OnCansel Cansel { get => onCansel; set => onCansel = value; }

    private void OnEnable()
    {
        SetContents(LevelManager.Instance.ByuManager.AllRollUpdates);
    }

    private void Start()
    {

        foreach (TapeClickItem item in tapeItems)
        {
            item.Onclick.AddListener(Select);
        }
    }

    public void Cansel_()
    {
        onCansel?.Invoke();
        gameObject.SetActive(false);
    }

    public void Select(TapeClickItem item, ByuDealer selected)
    {
        onSelectItem?.Invoke(item, selected);
    }


    public void Refresh(TapeClickItem item)
    {
        if (tapeItems.Contains(item))
        {
            item.SetRandomItem(tapeContents);
        }
    }

    public override void RefreshAll()
    {
        foreach (TapeItem item in tapeItems)
        {
            item.SetRandomItem(tapeContents);
        }
    }

    public void UpdateAll()
    {
        foreach (TapeItem item in tapeItems)
        {
            if (item.GetCurrentItem() == null)
            {
                item.SetRandomItem(tapeContents);
            }
        }
    }

    public override void SetContents(List<ByuDealer> newContents)
    {
        this.tapeContents = newContents.ToArray();
        UpdateAll();
    }

}
