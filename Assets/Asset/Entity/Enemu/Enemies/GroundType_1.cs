using System.Collections;
using UnityEngine;

public class GroundType_1 : Enemu
{
    public int attackTick, currentTick;

    private void Normalize()
    {
        speed.Multipler += speed.RecoveryRate;
        agent.speed = speed.GetCurrentValue();
    }

    public override void Tick()
    {
        TurretsTick();
        Normalize();
    }

    public override Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, System.Action cansel)
    {
        Entity result = LevelManager.Instance.EnemuManager.SpawnEnemu<GroundType_1>(new Vector3(spawner.transform.position.x, spawner.transform.position.y, 0), spawner);
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
