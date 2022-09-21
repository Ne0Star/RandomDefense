using DG.Tweening;
using System.Collections;
using UnityEngine;



// Задача просто получать информацию о сущностях в триггере и делать что либо с ними
public class Drone : Patron
{
    public SpriteRenderer radiusPreview;
    public StaticEntityTrigger splashTrigger;
    public float SFXDuration;
    public GameObject SFX;
    public Transform station;
    public Transform[] rotors;
    public bool isComplete = true;
    public bool rotateRotors;
    public bool isFlying = false;
    public Vector3 startPosition;


    private void Start()
    {
        isComplete = true;
        if (!station) station = transform.parent;
        startPosition = transform.position;
    }



    private void FixedUpdate()
    {
        if (splashTrigger)
            splashTrigger.gameObject.SetActive(false);
        if (!isComplete && rotateRotors)
            foreach (Transform t in rotors)
            {
                t.transform.Rotate(t.transform.rotation.eulerAngles + new Vector3(0, 0, Time.deltaTime * 0.1f));
            }
    }

    public void SetTargetType()
    {

    }

    public void MoveToStartPosition()
    {
        if (!splashTrigger || blockMove) return;
        StartCoroutine(Move());
    }
    private bool blockMove = false;
    private IEnumerator Move()
    {
        blockMove = true;
        float t = 0f;
        while (Vector2.Distance(transform.position, startPosition) > 0.01f)
        {
            t += 0.1f;
            transform.position = Vector3.Lerp(transform.position, startPosition, t);
            yield return new WaitForSeconds(0.01f / LevelManager.Instance.GameSpeed);
        }
        isComplete = true;
        splashTrigger.gameObject.SetActive(false);
        blockMove = false;
    }

    public void CloseSFX()
    {
        if (SFX)
            SFX.gameObject.SetActive(false);
    }
    [SerializeField] private AttackEffect attackEffect;
    public void ApplyAction(DroneActionType actionType, EntityUnit entity, float value)
    {
        switch (actionType)
        {
            case DroneActionType.Attack:
                if (splashTrigger)
                    splashTrigger.gameObject.SetActive(true);
                if (SFX)
                    SFX.gameObject.SetActive(true);
                if (entity)
                {
                    GameUtils.DealSplashDamage(transform.position, splashTrigger.GetAllRadius().ToArray(), splashTrigger.TriggerRadius, value);
                }
                break;
            case DroneActionType.Health:
                if (SFX)
                    SFX.gameObject.SetActive(true);
                if (entity)
                    entity.TakeHeal(value, 0.1f);
                break;
        }
    }

    public void SetViewTarget(Vector3 targetPos)
    {
        if (radiusPreview)
        {
            radiusPreview.transform.SetParent(null);
            radiusPreview.transform.position = targetPos;
            radiusPreview.transform.localScale = new Vector3(splashTrigger.TriggerRadius, splashTrigger.TriggerRadius, 0);
            radiusPreview.gameObject.SetActive(true);
            radiusPreview.transform.DORotate(new Vector3(0, 0, 360), 1f);
        }
    }

    private void OnDestroy()
    {
        if (radiusPreview)
        {
            radiusPreview.transform.localPosition = Vector3.zero;
            radiusPreview.transform.localScale = Vector3.zero;
            radiusPreview.gameObject.SetActive(false);
        }
    }

    public void RemoveViewTarget()
    {
        if (radiusPreview)
        {
            radiusPreview.transform.SetParent(transform);

            radiusPreview.transform.localPosition = Vector3.zero;
            radiusPreview.transform.localScale = Vector3.zero;
            radiusPreview.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (splashTrigger)
        {

            Gizmos.DrawWireSphere(transform.position, splashTrigger.TriggerRadius);

            if (radiusPreview)
            {
                radiusPreview.transform.localScale = new Vector3(splashTrigger.TriggerRadius, splashTrigger.TriggerRadius, 0);
            }
        }


    }
}
