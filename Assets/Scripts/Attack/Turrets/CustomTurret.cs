using UnityEngine;

public class CustomTurret : Turret
{
    [SerializeField] private Turret[] turrets;

    void Start()
    {
        turrets = gameObject.GetComponentsInChildren<Turret>();
    }
}
