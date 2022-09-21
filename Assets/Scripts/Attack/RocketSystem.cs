using System.Collections;
using UnityEngine;

public class RocketSystem : TrajectorySystem
{
    public RocketTurret[] turrets;
    
    void OnEnable()
    {
        StartCoroutine(SetTurrets());
    }

    public void AddTurret()
    {

    }

    public IEnumerator SetTurrets()
    {
        yield return new WaitForSeconds(0.1f);
        turrets = GetComponentsInChildren<RocketTurret>();
        foreach (RocketTurret turret in turrets)
        {
            if (turret)
            {
                if (trajectories != null)
                    if (currentCount < trajectories.Length)
                        if (trajectories[currentCount] != null)
                        {
                            turret.transform.SetParent(trajectories[currentCount].transform);
                            currentCount++;
                        }
            }
        }
    }
}
