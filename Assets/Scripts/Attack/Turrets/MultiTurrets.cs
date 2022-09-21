using System.Collections.Generic;

public class MultiTurrets : Turret
{
    public bool weaponsMode = false;
    public Turret mainTurret; // турель которая сама выставляет цель
    public List<Turret> turrets;

    private void Start()
    {
        foreach (Turret t in turrets)
        {
            if(t && t != mainTurret)
            {
                t.WeaponMode = weaponsMode;
            }
        }
    }

    public override void Tick()
    {
        foreach(Turret t in turrets)
        {
            t.Tick();
            t.Target = mainTurret.Target;
        }
    }
}
