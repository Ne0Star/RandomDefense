using System.Collections;
using UnityEngine;

// Турель которая создаёт снаряды и запекает их
public class ShellTurret : RotateTurret
{
    [Header("Указанный партрон будет запечён")]
    [SerializeField] protected Patron patronPrefab;
    [Header("Сколько патронов запеч")]
    [SerializeField] protected int batchCount;

    protected TrajectorySystem trajectory;
    protected Patron[] patrons;

    private IEnumerator Start()
    {
        while (!trajectory)
        {
            trajectory = FindObjectOfType<TrajectorySystem>();
            yield return new WaitForFixedUpdate();
        }
        patrons = new Patron[batchCount];
        for (int i = 0; i < batchCount; i++)
        {
            patrons[i] = Instantiate(patronPrefab, transform.position, Quaternion.identity, null).GetComponent<Patron>();
            patrons[i].name = "Patron: " + i;
            patrons[i].gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (patrons != null)
            foreach (Patron p in patrons)
            {
                if (p)
                {
                    if (p.gameObject)
                    {
                        p.gameObject.SetActive(false);
                    }
                }
            }
    }
    private void OnDestroy()
    {
        foreach (Patron p in patrons)
        {
            if (p)
            {
                if (p.gameObject)
                {
                    Destroy(p.gameObject);
                }
            }
        }
    }

    protected Patron GetFreePatron()
    {
        Patron result = null;
        if (patrons == null) return result;
        for (int i = 0; i < batchCount; i++)
        {
            if (patrons[i])
                if (!patrons[i].gameObject.activeInHierarchy)
                {
                    result = patrons[i];
                    return result;
                }
        }
        return result;
    }

    protected int GetFreePatronIndex()
    {
        int result = 0;
        for (int i = 0; i < batchCount; i++)
        {
            if (patrons[i])
                if (!patrons[i].gameObject.activeInHierarchy)
                {
                    result = i;
                }
        }
        return result;
    }

    //      Vector3 dir = (target.transform.position - transform.position).normalized;
    //      Vector3 center = transform.position + (dir * Radius);
    //      GameObject _target = new GameObject("target");
}
