
using System.Collections.Generic;
using UnityEngine;

public class MultyTrigger : Trigger
{
    [SerializeField] private StaticEntityTrigger[] triggers;


    public override EntityUnit GetOneRandom()
    {
        EntityUnit result = null;

        foreach (StaticEntityTrigger trigger in triggers)
        {
            result = trigger.GetOneRandom();
            if (result) return result;
        }
        return result;
    }

    public override bool CheckRadius(Entity entity)
    {
        bool result = false;
        foreach (StaticEntityTrigger trigger in triggers)
        {
            if (trigger.CheckRadius(entity)) { result = true; return result; };

        }
        return result;
    }
    public override List<EntityUnit> GetAllRadius()
    {
        List<EntityUnit> result = new List<EntityUnit>();
        foreach (StaticEntityTrigger trigger in triggers)
        {
            result.AddRange(trigger.GetAllRadius());
        }
        return result;
    }
    public override EntityUnit GetOneNear(bool inverse)
    {
        List<EntityUnit> temp = new List<EntityUnit>();
        foreach (StaticEntityTrigger trigger in triggers)
        {
            EntityUnit res = trigger.GetOneNear(inverse);
            temp.Add(res);
        }
        EntityUnit result = GetOneNearByArray(inverse, temp.ToArray());
        return result;
    }
    public override EntityUnit GetOneNear(int searchCount, bool inverse)
    {
        EntityUnit result = null;

        foreach (StaticEntityTrigger trigger in triggers)
        {
            result = trigger.GetOneNear(searchCount, inverse);
            if (result) return result;
        }
        return result;
    }
}
