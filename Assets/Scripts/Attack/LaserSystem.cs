using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    public LaserTurret[] turrets;
    void Awake()
    {
        SetTurrets();
    }

    public void SetTurrets()
    {
        turrets = GetComponentsInChildren<LaserTurret>();
        //foreach(LaserTurret turret in turrets)
        //{
        //    //turret.transform.SetParent(transform);
        //}
    }

}
