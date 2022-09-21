using UnityEngine;


/// <summary>
/// Задача класса купить сущность
/// </summary>
/// 
[CreateAssetMenu]
public class ByuDealer : ScriptableObject
{
    [SerializeField] private Sprite ico, bg;

    [SerializeField] private Entity prefab;

    [SerializeField] private Entity instance = null;

    public Entity Prefab { get => GetPrefab(); }

    private Entity GetPrefab()
    {
        return prefab;
    }

    public Sprite GetImageBG() { return bg; }
    public Sprite GetImage() { return ico; }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>return instance</returns>
    public Entity ByuAndSpawn(Balance balance, Vector3 spawnPosition, Quaternion spawnRotation, Transform spawnParent)
    {
        if (balance.BuyItem(this))
        {
            instance = Prefab.Spawn(spawnPosition, spawnRotation, null, spawnParent, () =>
            {
                Debug.Log("Byu Dealer: Не удалось заспавнить");

            }); //Instantiate(Prefab, spawnPosition, spawnRotation, spawnParent);
            if (instance)
            {
                LevelManager.Instance.SpawnEntity(instance);

            }
            else
            {
                balance.SellItem(this);
            }

        }
        else
        {
            return null;
        }
        return instance;
    }
}
