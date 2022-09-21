
using System.Collections;
using UnityEngine;

public class GroundType_5 : Enemu
{
    [SerializeField] protected float attackSpeed;

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
            attack = false;
            target = mainTrigger.GetOneRandom();
        }

    }

    private IEnumerator Attack()
    {
        while (target && target.gameObject.activeInHierarchy && mainTrigger.CheckRadius(target))
        {
            //GameUtils.LookAt2DSmooth(transform, target.transform.position, rotateOffset, rotateSpeed, 0.1f);

            foreach (Turret t in turrets)
            {
                t.SetTarget(target);
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
        Entity result = LevelManager.Instance.EnemuManager.SpawnEnemu<GroundType_5>(new Vector3(spawner.transform.position.x, spawner.transform.position.y, 0), spawner);
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
