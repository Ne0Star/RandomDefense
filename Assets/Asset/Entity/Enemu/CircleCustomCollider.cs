
using UnityEngine;

public class CircleCustomCollider : CustomCollider
{
    public Color GizmozColor = Color.green;
    public float Radius;
    public Vector3 Offset;

    /// <summary>
    /// Вернёт true если точка внутри триггера
    /// </summary>
    /// <returns></returns>
    public override bool CheckPoint(Vector3 worldPos)
    {
        bool result = false;
        Vector3 center = transform.position + Offset;
        if (Vector2.Distance(center, worldPos) <= Radius)
        {
            result = true;
            return result;
        }
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmozColor;
        Gizmos.DrawWireSphere(transform.position + Offset, Radius);
    }
}
