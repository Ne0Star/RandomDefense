using System.Collections.Generic;
using UnityEngine;


public abstract class CustomCollider : MonoBehaviour 
{
    public abstract bool CheckPoint(Vector3 worldPos);
}

public class BoxCustomCollider : CustomCollider
{


    [System.Serializable]
    public class OnTriggerEnter : UnityEngine.Events.UnityEvent { }
    [System.Serializable]
    public class OnTriggerExit : UnityEngine.Events.UnityEvent { }

    [SerializeField] private OnTriggerEnter triggerEnter;
    [SerializeField] private OnTriggerExit triggerExit;
    [SerializeField] private bool isTrigger;
    public List<Transform> checkers;

    public Color GizmozColor = Color.green;
    public Vector3 Size;
    public Vector3 Offset;

    public Vector3 LeftBottom { get => new Vector2((transform.position + Offset).x - (Size.x / 2), (transform.position + Offset).y - (Size.y / 2)); }
    public Vector3 LeftTop { get => new Vector2((transform.position + Offset).x - (Size.x / 2), (transform.position + Offset).y + (Size.y / 2)); }
    public Vector3 RightTop { get => new Vector2((transform.position + Offset).x + (Size.x / 2), (transform.position + Offset).y + (Size.y / 2)); }
    public Vector3 RightBottom { get => new Vector2((transform.position + Offset).x + (Size.x / 2), (transform.position + Offset).y - (Size.y / 2)); }


    /// <summary>
    /// Вернёт true если точка внутри триггера
    /// </summary>
    /// <returns></returns>
    public override bool CheckPoint(Vector3 worldPos)
    {
        bool result = false;
        Vector3 center = transform.position + Offset;
        //Vector2 leftBottom = new Vector2(center.x - (Size.x / 2), center.y - (Size.y / 2));
        //Vector2 rightTop = new Vector2(center.x + (Size.x / 2), center.y + (Size.y / 2));

        if (worldPos.x >= LeftBottom.x && worldPos.y >= LeftBottom.y && worldPos.x <= RightTop.x && worldPos.y <= RightTop.y)
        {
            result = true;
        }
        return result;
    }
    [SerializeField] private List<Transform> enters = new List<Transform>();
    private void Update()
    {
        if (!isTrigger) return;
        foreach (Transform t in checkers)
        {

            if (enters.Contains(t) && !CheckPoint(t.position))
            {
                enters.Remove(t);
                triggerExit.Invoke();
            }
            else if (CheckPoint(t.position) && !enters.Contains(t))
            {
                enters.Add(t);
                triggerEnter?.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmozColor;
        Gizmos.DrawWireCube(transform.position + Offset, Size);
    }
}