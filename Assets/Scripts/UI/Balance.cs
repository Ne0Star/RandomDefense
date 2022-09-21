using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Balance : MonoBehaviour
{
    [SerializeField] private Transform[] cheatObjects;
    [SerializeField] private bool cheatMode = false;
    // Визуальные значения
    private float _Gold, _Energy, _Component;


    private void Awake()
    {
        if(!cheatMode)
        switch (LevelManager.Instance.LevelPresset.BalanceType)
        {
            case BalanceType.Type_0:
                Gold = 1000;
                Energy = 500;
                Component = 200;
                break;
            case BalanceType.Type_1:
                Gold = 1000;
                Energy = 700;
                Component = 250;
                break;
            case BalanceType.Type_2:
                Gold = 2500;
                Energy = 1200;
                Component = 400;
                break;
            case BalanceType.Type_3:
                Gold = 5000;
                Energy = 2000;
                Component = 1000;
                break;
        }
        if(cheatMode)
        {
            foreach(Transform t in cheatObjects)
            {

                t.gameObject.SetActive(true);
            }
        }
        //labelGold.text = LangsList.GetWord("gold");
        //labelEnergy.text = LangsList.GetWord("energy");
        //labelComponents.text = LangsList.GetWord("components");

        UpdateBalance();
        StartCoroutine(AutoUpdate());
    }

    public bool BuyItem(float gold, float energy, float component)
    {
        if (((Gold - gold) >= 0) && ((Energy - energy) >= 0) && ((Component - component) >= 0))
        {
            //Debug.Log("Покупка" + component + " " + gold + " " + energy);
            Gold -= gold;
            Energy -= energy;
            Component -= component;
            UpdateBalance();
            return true;
        }
        else
        {
            if (!block1)
                StartCoroutine(WaitAnimation(gold, energy, component));
            return false;
        }
    }

    /// <summary>
    /// Имитирует покупку но не снимает деньги
    /// </summary>
    /// <returns></returns>
    public bool isByu(ByuDealer item)
    {
        if (((Gold - item.Prefab.CostData.Gold) >= 0) && ((Energy - item.Prefab.CostData.Energy) >= 0) && ((Component - item.Prefab.CostData.Component) >= 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Покупка предмета
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool BuyItem(ByuDealer item)
    {
        if (((Gold - item.Prefab.CostData.Gold) >= 0) && ((Energy - item.Prefab.CostData.Energy) >= 0) && ((Component - item.Prefab.CostData.Component) >= 0))
        {
            //Debug.Log("Покупка" + item.Component + " " + item.Gold + " " + item.Energy);
            Gold -= item.Prefab.CostData.Gold;
            Energy -= item.Prefab.CostData.Energy;
            Component -= item.Prefab.CostData.Component;
            UpdateBalance();
            return true;
        }
        else
        {
            StartCoroutine(WaitAnimation(item));
            return false;
        }
    }
    private IEnumerator WaitAnimation(float gold, float energy, float component)
    {
        if (block1) yield break;
        block1 = true;
        if (((Gold - gold) < 0))
        {
            Animation_NotBalance(textGold);
        }
        yield return new WaitUntil(() => blocker == false);
        if ((Energy - energy) < 0)
        {
            Animation_NotBalance(textEnergy);
        }
        yield return new WaitUntil(() => blocker == false);
        if (((Component - component) < 0))
        {
            Animation_NotBalance(textComponents);
        }
        yield return new WaitUntil(() => blocker == false);
        block1 = false;
    }
    private IEnumerator WaitAnimation(ByuDealer item)
    {
        if (block1) yield break;
        block1 = true;
        if (((Gold - item.Prefab.CostData.Gold) < 0))
        {
            Animation_NotBalance(textGold);
        }
        yield return new WaitUntil(() => blocker == false);
        if ((Energy - item.Prefab.CostData.Energy) < 0)
        {
            Animation_NotBalance(textEnergy);
        }
        yield return new WaitUntil(() => blocker == false);
        if (((Component - item.Prefab.CostData.Component) < 0))
        {
            Animation_NotBalance(textComponents);
        }
        yield return new WaitUntil(() => blocker == false);
        block1 = false;
    }

    public float notBalanceAnimationDuration;
    public Ease notBalanceAnimationEase;
    private bool block1 = false;
    private bool blocker = false;
    private void Animation_NotBalance(TMP_Text text)
    {
        if (blocker) return;
        blocker = true;
        Image render = text.transform.parent.GetComponent<Image>();
        if (render)
        {
            Color lastColor = render.color;
            render.DOColor(Color.black, notBalanceAnimationDuration / 2f).OnKill(() =>
            {
                render.DOColor(lastColor, notBalanceAnimationDuration / 2f).OnKill(() =>
                {
                    blocker = false;
                    UpdateBalance();
                });
            });

            //render.color = Color.black;
        }
        else
        {
            blocker = false;
        }


        //text.DOText("Not enough", notBalanceAnimationDuration).SetEase(notBalanceAnimationEase).OnKill(() =>
        //{
        //    UpdateBalance();
        //    blocker = false;
        //}).OnStart(() =>
        //{

        //});
    }

    /// <summary>
    /// Продаёт ещё не поставленный предмет
    /// </summary>
    /// <param name="item"></param>
    public void SellItem(ByuDealer item)
    {
        // float tempGold = Gold;
        // DOTween.To(() => tempGold, x => tempGold = x, tempGold + item.Gold, 0.2f).OnUpdate(() =>
        // {
        //     Gold = Mathf.Round(tempGold);
        //     UpdateBalance();

        // });

        Gold += item.Prefab.CostData.Gold;
        Energy += item.Prefab.CostData.Energy;
        Component += item.Prefab.CostData.Component;
        UpdateBalance();
    }
    public void SellItem(EntityUnit item, bool destroy)
    {
        if (item)
        {

            float health = item.Hit.Health;
            float strength = item.Hit.Strength;
            float maxValue = item.Hit.GetMaxStrength() + item.Hit.GetMaxHealth();

            float goldCost = item.CostData.Gold;
            float energyCost = item.CostData.Energy;
            float componentCost = item.CostData.Component;
            if (item is Wall)
            {
                Wall w = (Wall)item;
                if (w.Turret)
                {
                    //if (w.Turret.Hit)
                    //{
                    //    health += w.Turret.Hit.Health;
                    //    strength += w.Turret.Hit.Strength;
                    //    maxValue += w.Turret.Hit.GetMaxHealth() + w.Turret.Hit.GetMaxStrength();
                    //}

                    goldCost += w.Turret.CostData.Gold;
                    energyCost += w.Turret.CostData.Energy;
                    componentCost += w.Turret.CostData.Component;
                }
            }

            float currentValue = (health + strength);
            float persent = currentValue * 100 / maxValue;


            //Debug.Log("Сумма хп =" + maxValue + " текущее хп = " + currentValue);


            //Debug.Log("состовляет текущее от максимального " + persent);
            //Debug.Log("Стоимость " + item.CostData.Gold + " результат " + item.CostData.Gold / 100 * persent);
            Gold += goldCost / 100 * persent;
            Energy += energyCost / 100 * persent;
            Component += componentCost / 100 * persent;

            if (destroy)
            {
                Destroy(item.gameObject);
            }
        }


        UpdateBalance();
    }

    private IEnumerator AutoUpdate()
    {
        yield return new WaitForSeconds(2f);
        UpdateBalance();
        StartCoroutine(AutoUpdate());
        yield break;
    }

    public float animationDuration;

    public void UpdateBalance()
    {
        DOTween.To(() => _Gold, x => _Gold = x, Gold, animationDuration).OnUpdate(() =>
        {
            textGold.text = Math.Round(_Gold) + "";
        });
        DOTween.To(() => _Energy, x => _Energy = x, Energy, animationDuration).OnUpdate(() =>
        {
            textEnergy.text = Math.Round(_Energy) + "";
        });
        DOTween.To(() => _Component, x => _Component = x, Component, animationDuration).OnUpdate(() =>
        {
            textComponents.text = Math.Round(_Component) + "";
        });



    }

    public TMP_Text textGold, textEnergy, textComponents;
    public TMP_Text labelGold, labelEnergy, labelComponents;
    public float Gold, Energy, Component;
}
