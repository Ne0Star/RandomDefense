using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generator : Edifice
{
    [Range(1, 100)][SerializeField] private int generatedTick; // Как часто он генерирует енергию
    [SerializeField] private float energy; // Сколько енерргии он генерирует+
    [SerializeField] private int currentTick; // Текущее количество тиков
    [SerializeField] private float spawnZ;
    [SerializeField] private TileBase tile;
    [SerializeField] private Vector3 spawnWorldPosition;
    public override Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, Action cansel)
    {
        MapManager map = LevelManager.Instance.MapManager;
        if (map.GeneratorMap.GetTile(map.GeneratorMap.WorldToCell(spawnPosition)) || map.WallMap.GetTile(map.WallMap.WorldToCell(spawnPosition))
            || map.StaticMap.GetTile(map.StaticMap.WorldToCell(spawnPosition)))
        {
            cansel();
            return null;
        }
        else
        {
            Entity result = Instantiate(this, new Vector3(spawnPosition.x, spawnPosition.y, spawnZ), spawnRotation, spawnParent);
            spawnWorldPosition = spawnPosition;
            map.CreateTile(map.GeneratorMap, tile, result, spawnPosition);
            Vector3 totalPosition = map.GeneratorMap.GetCellCenterWorld(map.GeneratorMap.WorldToCell(spawnPosition));
            result.transform.position = new Vector3(totalPosition.x, totalPosition.y, 0);
            return result;
        }
    }
    public override void Tick()
    {
        currentTick++;
        if (currentTick >= generatedTick)
        {
            LevelManager.Instance.ByuManager.GetBalance().Energy += energy;
            currentTick = 0;
        }
    }


    private void OnDestroy()
    {
        OnDied_.Invoke((Generator)this);
        //LevelManager.Instance.ByuManager.GetBalance().Energy -= energy;

    }

    //public override Property[] GetAllProperty(UpdateEntity updater)
    //{
    //    if (!updater) return null;
    //    List<Property> result = new List<Property>();
    //    result.Add(new Property("health", updater.Update_Info.Health + "", Hit.Health + "", updater.Update_Info.Health + Hit.Health + ""));
    //    result.Add(new Property("strength", updater.Update_Info.Strength + "", Hit.Strength + "", updater.Update_Info.Strength + Hit.Strength + ""));
    //    return result.ToArray();
    //}
}
