using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField] protected float triggerRadius = 0f;
    [SerializeField] protected EntityType type;

    public float TriggerRadius { get => triggerRadius; set => triggerRadius = value; }
    public void SetRange(float range) => triggerRadius = range;

    public EntityUnit[] GetAllEntities()
    {
        EntityUnit[] result = null;
        switch (type)
        {
            case EntityType.Edifice:
                return LevelManager.Instance.EdificeManager.AllEdifice.ToArray();
            case EntityType.Enemu:
                return LevelManager.Instance.EnemuManager.GetAllActiveEnemies().ToArray();
            case EntityType.Wall:
                return LevelManager.Instance.EdificeManager.AllWalls.ToArray();
        }
        return result;
    }



    ///// <summary>
    ///// Возвращает все сущности в радиусе
    ///// </summary>
    ///// <returns></returns>
    public virtual List<EntityUnit> GetAllRadius()
    {
        List<EntityUnit> result = new List<EntityUnit>();
        //int t = 0;
        EnemuManager em = LevelManager.Instance.EnemuManager;
        for (int i = 0; i < GetAllEntities().Length; i++)
        {
            if (GetAllEntities()[i] && GetAllEntities()[i].gameObject.activeInHierarchy)
            {
                if (Vector2.Distance(transform.position, GetAllEntities()[i].transform.position) < triggerRadius)
                {
                    result.Add(GetAllEntities()[i]);
                }

                //if (t >= maxSearchCount)
                //{
                //    return result;
                //}
                //t++;
            }
        }
        return result;
    }

    /// <summary>
    /// Возвращает ближайшую сущность, из последних доавленных в массив, в пределах индекса
    /// </summary>
    /// <param name="serachCount"></param>
    /// <returns></returns>
    public virtual EntityUnit GetOneNear(int searchCount, bool inverse)
    {
        float max = float.MaxValue;
        int t = 0;
        EntityUnit result = null;

        EnemuManager em = LevelManager.Instance.EnemuManager;
        for (int i = inverse ? GetAllEntities().Length - 1 : 0; inverse ? (i > 0) : (i < GetAllEntities().Length); i = inverse ? (i - 1) : (i + 1))
        {
            if (GetAllEntities()[i] && GetAllEntities()[i].gameObject.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, GetAllEntities()[i].transform.position);
                if (distance < triggerRadius && distance < max)
                {
                    result = GetAllEntities()[i];
                    max = distance;
                }

                if (t >= searchCount)
                {
                    return result;
                }
                t++;
            }
        }
        return result;

    }


    /// <summary>
    /// Возвращает ближайшую сущность, из последних доавленных в массив, из половины юнитов
    /// </summary>
    /// <param name="serachCount"></param>
    /// <returns></returns>
    public virtual EntityUnit GetOneNear(bool inverse)
    {
        float max = float.MaxValue;
        int t = 0;
        EntityUnit result = null;
        EnemuManager em = LevelManager.Instance.EnemuManager;
        int searchCount = GetAllEntities().Length / 4;
        for (int i = inverse ? GetAllEntities().Length - 1 : 0; inverse ? (i > 0) : (i < GetAllEntities().Length); i = inverse ? (i - 1) : (i + 1))
        {
            if (GetAllEntities()[i] && GetAllEntities()[i].gameObject.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, GetAllEntities()[i].transform.position);
                if (distance < triggerRadius && distance < max)
                {
                    result = GetAllEntities()[i];
                    max = distance;
                }

                if (t >= searchCount)
                {
                    return result;
                }
                t++;
            }
        }
        return result;

    }

    protected EntityUnit GetOneNearByArray(bool inverse, EntityUnit[] array)
    {
        float max = float.MaxValue;
        int t = 0;
        EntityUnit result = null;

        int searchCount = array.Length / 4;
        for (int i = inverse ? array.Length - 1 : 0; inverse ? (i > 0) : (i < array.Length); i = inverse ? (i - 1) : (i + 1))
        {
            if (array[i] && array[i].gameObject.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, array[i].transform.position);
                if (distance < triggerRadius && distance < max)
                {
                    result = array[i];
                    max = distance;
                }

                if (t >= searchCount)
                {
                    return result;
                }
                t++;
            }
        }
        return result;

    }


    ///// <summary>
    ///// Возвращает одну
    ///// </summary>
    ///// <returns></returns>
    public virtual EntityUnit GetOneRandom()
    {
        EntityUnit result = null;
        List<EntityUnit> variants = new List<EntityUnit>();
        //int t = 0;
        EnemuManager em = LevelManager.Instance.EnemuManager;
        for (int i = 0; i < GetAllEntities().Length; i++)
        {
            if (GetAllEntities()[i] && GetAllEntities()[i].gameObject.activeInHierarchy)
            {
                if (Vector2.Distance(transform.position, GetAllEntities()[i].transform.position) < triggerRadius)
                {
                    variants.Add(GetAllEntities()[i]);
                }
                //if (t >= maxSearchCount)
                //{
                //    return result;
                //}
                //t++;
            }
        }
        if(variants.Count > 0)
        result = variants[Random.Range(0, variants.Count - 1)];
        return result;
    }

    /// <summary>
    /// Возвращает true если сущность в радиусе
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual bool CheckRadius(Entity entity)
    {
        bool result = false;
        if (entity)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(entity.transform.position.x, entity.transform.position.y)) < triggerRadius)
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        return result;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
    //}
    public enum EntityType
    {
        Edifice,
        Enemu,
        Wall,
        Turret,
        Generator
    }
}
