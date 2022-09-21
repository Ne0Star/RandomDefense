using System.Collections;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    public float attackDelay;
    public float attackDamage;
    public Entity target;

    public void SetTarget(Entity target)
    {
        this.target = target;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    void OnEnable()
    {
        StartCoroutine(WaitTarget());
    }

    private IEnumerator WaitTarget()
    {
        while(target)
        {
            //target.TakeDamage(attackDamage, 0.1f);
            yield return new WaitForSeconds(attackDelay);
        }
    }

}
