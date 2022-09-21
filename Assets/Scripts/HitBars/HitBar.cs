using DG.Tweening;
using UnityEngine;



[System.Serializable]
public abstract class HitBar : MonoBehaviour
{
    private void Awake()
    {
        UpdateData();
    }

    [SerializeField]
    protected float minDamage;
    [SerializeField]
    protected float maxDamage;

    [SerializeField]
    protected float totalHealthDamage;
    [SerializeField]
    protected float totalStrengthDamage;

    [SerializeField]
    protected float health;
    [SerializeField]
    protected float strength;

    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected float maxStrength;

    public float TotalHealthDamage { get => Mathf.Clamp(totalHealthDamage, 0, maxHealth); }
    public float TotalStrengthDamage { get => Mathf.Clamp(totalStrengthDamage, 0, maxStrength); }

    public float Strength { get => GetStrength(); }
    public float Health { get => GetHalth(); }

    public float GetStrength()
    {
        return strength;
    }
    public float GetHalth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetMaxStrength()
    {
        return maxStrength;
    }
    public void UpdateHealth(float value)
    {
        maxHealth += value;
        health += value;
        UpdateData();
    }
    public void UpdateStrength(float value)
    {
        maxStrength += value;
        strength += value;
        UpdateData();
    }
    public void SetHealth(float val)
    {
        maxHealth = val;
        health = Mathf.Clamp(val, 0, val);

        UpdateData();
    }
    public void SetStrength(float val)
    {
        maxStrength = val;
        strength = Mathf.Clamp(val, 0, val);

        UpdateData();
    }

    /// <summary>
    /// Срабатывает при любых изменениях показателей
    /// </summary>
    public abstract void UpdateData();


    /// <summary>
    /// false если фулл хп
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool AddHealth(float value)
    {
        bool result = health < maxHealth ? true : false;
        if (result)
        {
            health = Mathf.Clamp(health + value, 0, maxHealth);
        }
        UpdateData();
        return result;
    }

    /// <summary>
    /// false если фулл хп
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool AddStrength(float value)
    {
        bool result = strength < maxStrength ? true : false;
        if (result)
        {
            strength = Mathf.Clamp(strength + value, 0, maxStrength);
        }
        UpdateData();
        return result;
    }

    public bool isFull()
    {
        bool result = true;
        if (health < maxHealth) result = false;
        if (strength < maxStrength) result = false;
        return result;
    }

    public void SetFull()
    {
        health = maxHealth;
        strength = maxStrength;
        totalHealthDamage = maxHealth - health;
        totalStrengthDamage = maxStrength - strength;
        UpdateData();
    }

    /// <summary>
    /// false если фулл хп
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public void TakeHealth(float value, float duration)
    {


        if (health + value > maxHealth)
        {
            float tempStr = (value + health) - maxHealth;
            DOTween.To(() => health, x => health = x, Mathf.Clamp(health + value, 0, maxHealth), duration);
            DOTween.To(() => strength, x => strength = x, Mathf.Clamp(strength + value, 0, maxStrength), duration);
        }
        else
        {
            DOTween.To(() => health, x => health = x, Mathf.Clamp(health + value, 0, maxHealth), duration);
        }
        UpdateData();
    }

    // Добавить 50 здоровья, имеется 25 здоровья, максимум 25 здоровья и 50 прочности

    public void TakeDamage(float damage, float duration, System.Action onDied)
    {
        if (GetStrength() > 0)
        {
            totalStrengthDamage += damage;
            strength = Mathf.Clamp(maxStrength - TotalStrengthDamage, 0, maxStrength);
        }
        else if (GetHalth() > 0)
        {
            totalHealthDamage += damage;
            health = Mathf.Clamp(maxHealth - totalHealthDamage, 0, maxHealth);
        }
        if (GetHalth() <= 0)
            onDied();

        UpdateData();
    }
    protected void FixedUpdate()
    {
        UpdateData();
    }

    protected void OnDrawGizmos()
    {
        totalHealthDamage = maxHealth - health;
        totalStrengthDamage = maxStrength - strength;
    }

}