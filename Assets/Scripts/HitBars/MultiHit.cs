using System.Collections.Generic;
using UnityEngine;

public class MultiHit : HitBar
{
    [SerializeField] private List<HitBar> bars;

    public override void UpdateData()
    {
        foreach(HitBar bar in bars)
        {
            bar.UpdateData();
        }
    }
}
