using DG.Tweening;
using UnityEngine;

public class RotateTurret : Turret
{
    [SerializeField] protected bool rotate = true;
    [SerializeField] protected Transform rotateParent;
    [SerializeField] protected Trigger mainTrigger;
    [SerializeField] protected bool attack;
    [SerializeField] protected float rotateOffset;
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected bool rotated;

    private void Awake()
    {
        if (!rotateParent) rotateParent = transform;
    }

    public override void HideRadius()
    {
        radius.Close();
    }

    public override void ShowRadius()
    {
        radius.Open(mainTrigger.TriggerRadius);
    }

    public override float GetTarretRadius()
    {
        return mainTrigger.TriggerRadius;
    }

    public override void Tick()
    {

        if (attack) {
            if (rotate && target && target.gameObject.activeInHierarchy)
                rotateParent.DORotateQuaternion(GameUtils.LookAt2DValue(rotateParent, target.transform, rotateOffset), rotateSpeed / LevelManager.Instance.GameSpeed).OnKill(() =>
                {

                });
return;
        } 

        /// Нету цели
        if (mainTrigger && (!target || !target.gameObject.activeInHierarchy))
            target = mainTrigger.GetOneNear(true);
        if (mainTrigger && (!target || !target.gameObject.activeInHierarchy))
            target = mainTrigger.GetOneNear(false);
        if (mainTrigger && (!target || !target.gameObject.activeInHierarchy))
            target = mainTrigger.GetOneRandom();
        // Цель есть
        if (target != null && target.gameObject.activeInHierarchy)
        {
            // Если она не в диапазоне, сломать
            if (mainTrigger && !mainTrigger.CheckRadius(target))
            {
                target = null;
                return;
            }
            // Если цель в радиусе атаковать
            else
            {
                attack = true;
                if (rotate)
                    rotateParent.DORotateQuaternion(GameUtils.LookAt2DValue(rotateParent, target.transform, rotateOffset), rotateSpeed / LevelManager.Instance.GameSpeed).OnKill(() =>
                    {
                        Check();
                    });
                else
                Check();
            }
        }
    }

    private void Check()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {

            rotated = true;
            OnAttack();
        }
        else
        {
            attack = false;
        }
    }


    protected virtual void OnAttack()
    {

    }

    private void OnDrawGizmos()
    {
        if (radius)
        {
            radius.transform.localScale = new Vector3(mainTrigger.TriggerRadius, mainTrigger.TriggerRadius, 0);
        }
    }

}
