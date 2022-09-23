using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : RotateTurret
{

    [SerializeField] private float attackSpeed;
    [SerializeField] private MultiplerValue damage;
    [SerializeField] private Transform[] sfx;
    [SerializeField] private Trigger[] triggers;
    [SerializeField] private Transform radiusTarget;
    [SerializeField] private bool wait;
    [SerializeField] private float triggersDuration;


    protected override void OnAttack()
    {
        StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        foreach (Transform s in sfx)
        {
            s.gameObject.SetActive(true);
        }

        List<EntityUnit> tt = new List<EntityUnit>();
        for (int i = 0; i < triggers.Length; i++)
        {
            tt.AddRange(triggers[i].GetAllRadius());
            //Debug.Log(triggers.Length * rotateSpeed + " " + rotateSpeed / triggers.Length);
            yield return new WaitForSeconds((rotateSpeed / triggers.Length) * triggersDuration);
        }
        targets = tt;

        while (target && target.gameObject.activeInHierarchy && mainTrigger.CheckRadius(target))
        {

            yield return new WaitForSeconds(attackSpeed / LevelManager.Instance.GameSpeed);
            if (targets != null)
            {
                GameUtils.DealSplashDamage(transform.position, targets.ToArray(), mainTrigger.TriggerRadius, damage.GetCurrentValue(), effects);
            }
        }
        if (!wait)
            StartCoroutine(WaitEnd());

        attack = false;
        rotated = false;
    }

    private IEnumerator WaitEnd()
    {
        wait = true;
        yield return new WaitForSeconds(1f / LevelManager.Instance.GameSpeed);
        if (!target)
            foreach (Transform s in sfx)
            {
                s.gameObject.SetActive(false);
            }
        wait = false;
        yield break;
    }

}
