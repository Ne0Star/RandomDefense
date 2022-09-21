
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private bool drawLine = true;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform[] path;
    public float ReachDistance;
    public Vector3[] GetPath()
    {
        Vector3[] result = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            result[i] = path[i].transform.position;
        }

        return result;
    }

    public Transform GetTarget(int index)
    {
        return path[index];
    }

    private void OnDrawGizmos()
    {
        path = gameObject.GetComponentsInChildren<Transform>();
        Gizmos.color = Color.blue;
        if (lineRenderer != null) lineRenderer.positionCount = path.Length;
        for (int i = 0; i < path.Length; i++)
        {

            path[i].gameObject.name = "Path: " + i;
            if (drawLine)
            {
                if (i < 1)
                {
                    Gizmos.DrawLine(path[0].position, transform.position);
                    Gizmos.DrawWireSphere(transform.position, ReachDistance);
                }
                else if (i > 0 && i < path.Length)
                {
                    Gizmos.DrawLine(path[i - 1].position, path[i].position);
                    Gizmos.DrawWireSphere(path[i].position, ReachDistance);
                }
                else
                {
                    Gizmos.DrawLine(path[path.Length - 1].position, transform.position);
                    Gizmos.DrawWireSphere(transform.position, ReachDistance);
                }
            }

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(i, new Vector3(path[i].position.x, path[i].position.y, 0));
            }

        }
    }
}

public class GroundSpawner : Spawner
{

}
