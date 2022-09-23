using System.Collections;
using UnityEngine;

public class ElectroTurret : Turret
{

    [SerializeField] private MultiplerValue maxDamage;
    [SerializeField] private MultiplerValue minDamage;

    [SerializeField] private ElecticDealer[] dealers;
    [SerializeField] private StaticEntityTrigger mainTrigger;


    [SerializeField] AttackType attackType;
    [SerializeField] private bool attack = false;
    private enum AttackType
    {
        FocusOne,
        Random
    }

    private void Start()
    {
        dealers = gameObject.GetComponentsInChildren<ElecticDealer>();
    }

    public override float GetTarretRadius()
    {
        return mainTrigger.TriggerRadius;
    }
    public override void ShowRadius()
    {
        radius.Open(mainTrigger.TriggerRadius);
    }

    [SerializeField] private int attackSpeed;
    [SerializeField] private int currentTick;
    [SerializeField] private int totalAttackSpeed;
    public override void Tick()
    {
        totalAttackSpeed = Mathf.RoundToInt(attackSpeed / LevelManager.Instance.GameSpeed);
        if (attack) return;
        currentTick++;
        if (currentTick >= totalAttackSpeed)
        {
            Attack();
            currentTick = 0;
        }
    }

    private void Attack()
    {
        StartCoroutine(Attack_Animation());
    }

    private IEnumerator Attack_Animation()
    {
        attack = true;
        targets = mainTrigger.GetAllRadius();
        if (attackType == AttackType.FocusOne)
        {
            EntityUnit target = targets[Random.Range(0, targets.Count)];
            if (target && target.gameObject.activeInHierarchy && mainTrigger.CheckRadius(target))
                foreach (ElecticDealer dealer in dealers)
                {
                    dealer.SetTarget(target, Random.Range(minDamage.GetCurrentValue(), maxDamage.GetCurrentValue()), mainTrigger.TriggerRadius);
                }

        }
        else if (attackType == AttackType.Random)
        {
            foreach (EntityUnit e in targets)
            {
                if (e && e.gameObject.activeInHierarchy && mainTrigger.CheckRadius(e))
                {
                    ElecticDealer dealer = dealers[Random.Range(0, dealers.Length)];
                    dealer.SetTarget(e, Random.Range(minDamage.GetCurrentValue(), maxDamage.GetCurrentValue()), mainTrigger.TriggerRadius);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        attack = false;
    }
    private void OnDrawGizmos()
    {
        if (radius)
        {
            radius.transform.localScale = new Vector3(mainTrigger.TriggerRadius, mainTrigger.TriggerRadius, 0);
        }
    }
}
