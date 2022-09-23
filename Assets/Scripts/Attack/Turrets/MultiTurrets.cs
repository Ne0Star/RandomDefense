using System.Collections.Generic;
using UnityEngine;

public class MultiTurrets : Turret
{
    public bool waotRotateMain = false;
    public bool weaponsMode = false;
    public RotateTurret mainTurret; // турель которая сама выставляет цель
    public List<Turret> turrets;

    private void Start()
    {
        foreach (Turret t in turrets)
        {
            if (t && t != mainTurret)
            {
                t.WeaponMode = weaponsMode;
            }
        }
    }

    public override float GetTarretRadius()
    {
        float radius = 100f;
        foreach (Turret t in turrets)
        {
            if(t.GetTarretRadius() < radius)
            {
                radius = t.GetTarretRadius();
            }
        }
        return radius;
    }

    private void OnDrawGizmos()
    {
        foreach (Turret t in turrets)
        {
            t.Radius.SetRadiusColor(t.CostData.RaresType);
        }
    }
    public override void HideRadius()
    {
        foreach (Turret t in turrets)
        {
            t.HideRadius();
        }
    }
    public override void ShowRadius()
    {
        foreach (Turret t in turrets)
        {
            t.ShowRadius();
        }
    }
    //bool t = false;
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        t = !t;
    //        if (t)
    //            ShowRadius();
    //        else HideRadius();
    //    }
    //}

    public override void Tick()
    {
        foreach (Turret t in turrets)
        {
            t.Tick();
            if (waotRotateMain)
            {
                if (mainTurret.Rotated)
                {

                    t.Target = mainTurret.Target;
                }
            }
            else
            {
                t.Tick();
                t.Target = mainTurret.Target;
            }
        }
    }
}
