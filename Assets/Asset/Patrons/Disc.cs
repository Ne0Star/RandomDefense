
using UnityEngine;
public class Disc : Patron
{
    [SerializeField] Transform rotateParent;
    [SerializeField] private float attackSpeed;
    [SerializeField] private bool rotate;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool decreaseDamage; // Уменьшает урон с каждой сущностью
    [SerializeField] private float damage;
    [SerializeField] public Vector3 startPos;
    [SerializeField] private StaticEntityTrigger mainTrigger;
    [SerializeField] public float turretRadius;

    private void FixedUpdate()
    {
            for (int i = 0; i < mainTrigger.GetAllRadius().Count; i++)
            {
                EntityUnit target = mainTrigger.GetAllRadius()[i];
                if (target)
                {
                    if (decreaseDamage)
                    {
                        float distance = Vector3.Distance(startPos, target.transform.position);
                        float percent = Mathf.InverseLerp(turretRadius, 0f, distance);
                        target.TakeDamage(damage * percent, 0.1f, transform.position);
                    }
                    else
                    {
                        target.TakeDamage(damage, 0.1f, transform.position);
                    }
                }
            }
        if (rotate)
        {
            rotateParent.Rotate(new Vector3(0, 0, Time.deltaTime * rotateSpeed));
        }
    }
}
