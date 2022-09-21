using System.Collections;
using UnityEngine;


[System.Serializable]
public struct LaserPresset
{

}

[RequireComponent(typeof(LineRenderer))]
public class LaserTurret : RotateTurret
{
    [SerializeField] private AttackEffect attackEffect;
    [SerializeField] private Transform startParticle, endParticle;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;

    private void DisableLine()
    {
        lineRenderer.positionCount = 0;
        startParticle.gameObject.SetActive(false);
        endParticle.gameObject.SetActive(false);
    }
    protected override void OnAttack()
    {
        StartCoroutine(Attack_Animation());
    }
    
    private IEnumerator Attack_Animation()
    {

        while (target && target.gameObject.activeInHierarchy && mainTrigger.CheckRadius(target))
        {
            GameUtils.LookAt2DSmooth(transform, target.transform.position, rotateOffset, rotateSpeed, 0.1f);
            lineRenderer.positionCount = 2;
            startParticle.transform.position = transform.position;
            endParticle.transform.position = target.transform.position;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, new Vector2(0, Vector2.Distance(transform.position, target.transform.position)));
            startParticle.gameObject.SetActive(true);
            endParticle.gameObject.SetActive(true);
            target.TakeDamage(attackDamage * LevelManager.Instance.GameSpeed, 0.05f, target.transform.position);
            yield return new WaitForSeconds(attackSpeed / LevelManager.Instance.GameSpeed);
        }
        DisableLine();
        attack = false;
        rotated = false;
        yield break;
    }
}