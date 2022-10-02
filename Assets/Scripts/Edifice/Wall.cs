
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Wall : Edifice
{
    [SerializeField] private TileBase tile;
    [SerializeField] private Turret turret;
    [SerializeField] private Vector3 spawnWorldPosition;
    public Turret Turret { get => turret; }

    protected override void AwakeUnit()
    {
        Hit.gameObject.SetActive(false);
    }

    public override Entity Spawn(Vector3 spawnPosition, Quaternion spawnRotation, Spawner spawner, Transform spawnParent, Action cansel)
    {
        navMeshObstacle = gameObject.GetComponentInChildren<NavMeshObstacle>();
        MapManager map = LevelManager.Instance.MapManager;

        if (map.GeneratorMap.GetTile(map.GeneratorMap.WorldToCell(spawnPosition)) || map.WallMap.GetTile(map.WallMap.WorldToCell(spawnPosition))
            || map.StaticMap.GetTile(map.StaticMap.WorldToCell(spawnPosition)))
        {
            cansel();
            return null;
        }
        else
        {
            Entity result = Instantiate(this, spawnPosition, spawnRotation, spawnParent);
            spawnWorldPosition = spawnPosition;
            map.CreateTile(map.WallMap, tile, result, spawnPosition);
            Vector3 totalPosition = map.WallMap.GetCellCenterWorld(map.WallMap.WorldToCell(spawnPosition));
            result.transform.position = new Vector3(totalPosition.x, totalPosition.y, 0);
            //complete(result);
            return result;
        }
    }
    public void SetTurret(Turret turret, System.Action complete, System.Action cansel)
    {
        if (this.turret != null)
        {
            cansel();
            return;
        }
        this.turret = turret;
        turret.transform.SetParent(transform);
        turret.transform.position = transform.position;
        complete();
    }
}
