using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR : MonoBehaviour
{
    [SerializeField] private SpriteRenderer radiusRender;
    [SerializeField] private float openDuration;
    [SerializeField] private float closeDuration;
    [SerializeField] private Ease openEasy;
    [SerializeField] private Ease closeEasy;
    [SerializeField] private StaticEntityTrigger trigger;

    public virtual void Open()
    {
        transform.DOScale(new Vector3(trigger.TriggerRadius, trigger.TriggerRadius, trigger.TriggerRadius), openDuration).SetEase(openEasy).OnStart(() => radiusRender.enabled = true);
    }
    public virtual void Open(float radius)
    {
        transform.DOScale(new Vector3(radius,radius,radius), openDuration).SetEase(openEasy).OnStart(() => radiusRender.enabled = true);
    }
    public virtual void Close()
    {
        transform.DOScale(new Vector3(0, 0, 0), closeDuration).SetEase(closeEasy).OnKill(() => radiusRender.enabled = false);
    }
}
