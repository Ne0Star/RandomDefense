using System.Collections;
using UnityEngine;

public class Boss_Type_0 : Enemu
{
    [SerializeField] protected float attackSpeed;
    //
    [SerializeField] protected StaticEntityTrigger searchTrigger, attackTrigger;
    public int attackTick, currentTick;

    private void Normalize()
    {
        speed.Multipler += speed.RecoveryRate;
        agent.speed = speed.GetCurrentValue();
    }

    public override void Tick()
    {
        Normalize();
        if (attack) return;

        if (target && target.gameObject.activeInHierarchy)
        {
            attack = true;
            blockJobMove = true;
            StartCoroutine(Attack());
        }
        else
        {

            target = searchTrigger.GetOneRandom();
        }

    }

    private IEnumerator Attack()
    {
        while (target && target.gameObject.activeInHierarchy)
        {
            blockJobMove = true;
            if (!searchTrigger.CheckRadius(target)) { target = null; attack = false; break; }


            foreach (Turret t in turrets)
            {
                t.Tick();
            }
            yield return new WaitForSeconds(attackSpeed / LevelManager.Instance.GameSpeed);
        }
        attack = false;
        blockJobMove = false;
        yield break;
    }

    public override Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, System.Action cansel)
    {
        Entity result = LevelManager.Instance.EnemuManager.SpawnEnemu<Boss_Type_0>(new Vector3(spawnPosition.x, spawnPosition.y, 0), spawner);
        if (!result) { cansel(); return null; };
        if (result)
        {
            return result;
        }
        else
        {
            cansel();
            return null;
        }
    }

    private IEnumerator WaitEffect(AttackEffect effect)
    {

        yield break;
    }
}
