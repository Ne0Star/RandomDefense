using System.Collections;
using UnityEngine;

public class Patron : MonoBehaviour
{
    public Renderer rend;
    public float destroyParticleLife;
    public GameObject destroyParticle, updateParticle, startParticle;

    private void Awake()
    {
        if (rend)
            rend = gameObject.GetComponent<Renderer>();
    }

    public virtual void Explose(System.Action onComplete)
    {
        StartCoroutine(WaitExplose(onComplete));
    }

    private IEnumerator WaitExplose(System.Action onComplete)
    {
        destroyParticle.gameObject.SetActive(true);
        yield return new WaitForSeconds(destroyParticleLife);
        destroyParticle.gameObject.SetActive(false);
        onComplete();
    }

    public void DisableRendering()
    {
        rend.enabled = false;
    }
    public void EnableRendering()
    {
        rend.enabled = true;
    }

    public void SetDestroyParticleScale(Vector3 scale)
    {
        destroyParticle.transform.localScale = scale;
    }
}
