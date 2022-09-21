using Unity.Collections;
using UnityEngine;

public class TrajectorySystem : MonoBehaviour
{
    public int currentCount = 0;
    public Allocator allocator = Allocator.Persistent;
    public TrajectoryDealer[] trajectories;

    public bool AllowBuild()
    {
        bool result = true;
        foreach (TrajectoryDealer dealer in trajectories)
        {
            if (!dealer.AllowBuild())
            {
                result = false;
            }
        }
        return result;
    }

    private void Awake()
    {
        trajectories = gameObject.GetComponentsInChildren<TrajectoryDealer>();
        foreach (TrajectoryDealer dealer in trajectories)
        {
            dealer.allocator = allocator;
        }
    }

    public void CreatePatron(DynamicData data, Transform patron, Transform target, System.Action<Transform, Transform> OnReached)
    {
        if (currentCount == trajectories.Length)
        {
            currentCount = 0;
        }
        trajectories[currentCount].CreatePatron(data, patron, target, OnReached);
        currentCount++;
    }
}