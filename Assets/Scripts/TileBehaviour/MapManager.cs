using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileData
{

    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private Vector3Int position;
    [SerializeField]
    private Entity entity;

    public Vector3Int Position_ { get => position; set => position = value; }
    public Entity Entity_ { get => entity; set => entity = value; }
    public Tilemap Map { get => map; set => map = value; }
}
public class MapManager : MonoBehaviour
{
    public BoundsInt grassBounds;
    public TileBase grassTile;
    [SerializeField] private Grid grid;
    // grass - Возле стен
    // grpund - просто земля
    [SerializeField] private Tilemap wallMap, groundMap, generatorMap, staticMap;

    [SerializeField] private List<TileData> datas = new List<TileData>();

    public Tilemap WallMap { get => wallMap; set => wallMap = value; }
    public Tilemap GroundMap { get => groundMap; set => groundMap = value; }
    public Tilemap GeneratorMap { get => generatorMap; set => generatorMap = value; }
    public Tilemap StaticMap { get => staticMap; set => staticMap = value; }

    public TileData GetTileData(Tilemap map, Vector3 worldPosition)
    {
        TileData result = null;
        Vector3Int pos = map.WorldToCell(worldPosition);

        foreach (TileData data in datas)
        {
            if (pos == data.Position_ && map == data.Map)
            {
                result = data;
            }
        }
        return result;
    }
    public void CreateTile(Tilemap map, TileBase tile, Entity enity, Vector3 worldPosition)
    {
        Vector3Int pos = map.WorldToCell(worldPosition);
        TileData newData = new TileData() { Entity_ = enity, Position_ = pos, Map = map };
        datas.Add(newData);
        enity.OnDied_.AddListener((e) => DeleteTile(newData));
        map.SetTile(pos, tile);

    }

    private void DeleteTile(TileData data)
    {
        if (!data.Map) return;
       
        data.Map.SetTile(data.Position_, null);
 datas.Remove(data);
    }

}
