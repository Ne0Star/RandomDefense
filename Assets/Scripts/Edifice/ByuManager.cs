using System.Collections.Generic;
using UnityEngine;

public class ByuManager : MonoBehaviour
{
    [SerializeField] private Balance balance;
    public Tape tapeBuilding;
    public UIPage tapeBuildingPage;


    [SerializeField] private List<ByuDealer> allBuildings = new List<ByuDealer>();
    [SerializeField] private List<ByuDealer> allRollWalls;
    [SerializeField] private List<ByuDealer> allRollTurrets;
    [SerializeField] private List<ByuDealer> allRollUpdates;
    [SerializeField] private List<ByuDealer> allRoll;
    public List<ByuDealer> AllRoll { get => allRoll; set => allRoll = value; }
    public List<ByuDealer> AllRollUpdates { get => allRollUpdates; set => allRollUpdates = value; }
    public List<ByuDealer> AllRollTurrets { get => allRollTurrets; set => allRollTurrets = value; }
    public List<ByuDealer> AllRollWalls { get => allRollWalls; set => allRollWalls = value; }
    public List<ByuDealer> AllBuildings { get => allBuildings; set => allBuildings = value; }

    public Balance GetBalance() => balance;



    [SerializeField] private bool refresh = false;
    private void OnDrawGizmos()
    {
        if (refresh)
        {
            Roll();

            refresh = false;
        }

    }

    public void Roll()
    {
        allRollWalls.Sort((x, y) => y.Prefab.CostData.Chance.CompareTo(x.Prefab.CostData.Chance));
    }

    private void Awake()
    {
        balance = FindObjectOfType<Balance>();
        allRoll.AddRange(AllRollWalls);
        allRoll.AddRange(AllRollTurrets);
        allRoll.AddRange(allRollUpdates);


        allBuildings.AddRange(AllRollWalls);
        allBuildings.AddRange(AllRollTurrets);

    }

    private void Start()
    {
        tapeBuilding.SetContents(allBuildings);
        Roll();
        //tapeBuilding.RefreshAll();
    }
}
