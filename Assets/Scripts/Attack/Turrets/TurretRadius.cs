using DG.Tweening;
using UnityEngine;
public class TurretRadius : MonoBehaviour
{
    [SerializeField] private SpriteRenderer radiusRender;
    [SerializeField] private float openDuration;
    [SerializeField] private float closeDuration;
    [SerializeField] private Ease openEasy;
    [SerializeField] private Ease closeEasy;

    public void Open(float radius)
    {
        transform.DOScale(new Vector3(radius, radius, radius), openDuration).SetEase(openEasy).OnStart(() => radiusRender.enabled = true);
    }

    public void Close()
    {
        transform.DOScale(new Vector3(0, 0, 0), closeDuration).SetEase(closeEasy).OnKill(() => radiusRender.enabled = false);
    }

}
