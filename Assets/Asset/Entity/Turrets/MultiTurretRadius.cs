using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTurretRadius : TR
{
    public List<TR> radiuses;

    public override void Open()
    {
        foreach(TR tr in radiuses)
        {
            tr.Open();
        }
    }

    public override void Close()
    {
        foreach (TR tr in radiuses)
        {
            tr.Close();
        }
    }

}
