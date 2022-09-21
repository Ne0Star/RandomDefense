using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TapeItem : MonoBehaviour
{
    [SerializeField] private bool useRare = false;
    [SerializeField] protected Image contentImgBG, contentImg, colorBg;
    [SerializeField] protected ByuDealer currentItem;

    public ByuDealer CurrentItem { get => currentItem; }

    public virtual void SetCurrentItem(ByuDealer item)
    {
        this.currentItem = item;
    }
    public ByuDealer GetCurrentItem() => currentItem;

    public void SetRandomItemRare(ByuDealer[] variants)
    {
        if(!useRare)
        {
            SetRandomItem(variants);
            return;
        }
        int index = Random.Range(0, variants.Length);
        for (int i = 0; i < variants.Length; i++)
        {
            float chance = 100.0f - Random.Range(0.0f, variants[i].Prefab.CostData.Chance);
            float tempChance = Random.Range(0.0f, 100.0f);
            if ((chance <= tempChance))
            {
                index = i;
                break;
            }
        }
        colorBg.color = LevelManager.Instance.GetRareColor(variants[index].Prefab.CostData.RaresType);
        SetCurrentItem(variants[index]);
    }
    public void SetRandomItem(ByuDealer[] variants)
    {
        if (useRare)
        {
            SetRandomItemRare(variants);
            return;
        }
        int index = Random.Range(0, variants.Length);
        colorBg.color = LevelManager.Instance.GetRareColor(variants[index].Prefab.CostData.RaresType);
        SetCurrentItem(variants[index]);
    }
    protected ByuDealer FindSuitable(List<ByuDealer> creators)
    {
        ByuDealer result = null;
        foreach (ByuDealer creator in creators)
        {
            float chance = Random.Range(0f, creator.Prefab.CostData.Chance);
            float tempChance = Random.Range(0, 100f);
            if (chance <= tempChance)
            {
                result = creator;
            }
        }
        return result;
    }

    protected IEnumerator SetCreator(List<ByuDealer> creators, System.Action<ByuDealer> result)
    {
        ByuDealer creator = null;
        while (!creator)
        {
            creator = FindSuitable(creators);
            yield return new WaitForFixedUpdate();
        }
        result.Invoke(creator);
        yield break;
    }

}
