using UnityEngine;

public class StandartBar : HitBar
{
    [SerializeField] private bool destroyVisual;
    [SerializeField] Transform StrengthTarget, HealthTarget;
    private void Awake()
    {
        if (destroyVisual)
        {
            if (transform.GetChild(0))
                Destroy(transform.GetChild(0).transform.gameObject);
        }
        UpdateData();
    }
    public override void UpdateData()
    {
        if (destroyVisual) return;
        float resultX = 0f;
        float heal = Mathf.Clamp(1f / health, 0.001f, 9999f);
        float maxHeal = Mathf.Clamp(1f / maxHealth, 0.001f, 999f);
        resultX = Mathf.Clamp((1f / heal) * maxHeal, 0, 1f);
        if (HealthTarget)
            if (HealthTarget.transform)
                HealthTarget.transform.localScale = new Vector3(resultX, HealthTarget.localScale.y, HealthTarget.localScale.z);
        float str = Mathf.Clamp(1f / strength, 0.001f, 9999f);
        float maxStr = Mathf.Clamp(1f / maxStrength, 0.001f, 9999f);
        resultX = Mathf.Clamp((1f / str) * maxStr, 0, 1f);
        if (StrengthTarget)
            if (StrengthTarget.transform)
                StrengthTarget.transform.localScale = new Vector3(resultX, StrengthTarget.localScale.y, StrengthTarget.localScale.z);
    }
}
