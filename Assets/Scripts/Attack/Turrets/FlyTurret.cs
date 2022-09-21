using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FlyTurret : Turret
{

    [SerializeField] private AttackEffect attackEffect;

    [SerializeField] private Ease in_ = Ease.InBack, out_ = Ease.OutBack;
    [SerializeField] private NavMeshObstacle agent;


    [SerializeField] private Transform radiusVFX;
    [SerializeField] private StaticEntityTrigger mainTrigger;

    [SerializeField] private int attackTick;
    [SerializeField] private int currentTick;

    [SerializeField] private MultiplerValue in_duration, out_duration;

    private void OnEnable()
    {
        radiusVFX.localScale = Vector3.zero;
        StartCoroutine(AgentFix());
    }

     private IEnumerator AgentFix()
    {
        yield return new WaitForSeconds(0.05f);
        agent.enabled = true;
    }

    private void OnDisable()
    {
        agent.enabled = false;
    }
    public override void Tick()
    {
        currentTick++;
        if (currentTick >= attackTick)
        {
            target = mainTrigger.GetOneRandom();
            if (target && target.gameObject.activeInHierarchy && !attack)
                Attack();
            currentTick = 0;
        }
    }

    [SerializeField] private bool attack = false;

    private void Attack()
    {
        attack = true;

        float rad = 0f;
        DOTween.To(() => rad, x => rad = x, mainTrigger.TriggerRadius, in_duration.GetCurrentValue()).SetEase(in_).OnUpdate(() =>
        {
            agent.radius = rad;
            radiusVFX.transform.localScale = new Vector3(rad, rad, rad);
        }).OnKill(() =>
        {
            DOTween.To(() => rad, x => rad = x, 0, in_duration.GetCurrentValue()).SetEase(in_).OnUpdate(() =>
            {
                agent.radius = rad;
                radiusVFX.transform.localScale = new Vector3(rad, rad, rad);
            }).OnKill(() =>
            {
                attack = false;
            });
        });

        //radiusVFX.DOScale(mainTrigger.TriggerRadius, in_duration.GetCurrentValue()).SetEase(in_).OnKill(() =>
        //{
        //    radiusVFX.DOScale(0, out_duration.GetCurrentValue()).SetEase(out_).OnKill(() =>
        //    {
        //        attack = false;
        //    });
        //});

    }

    ////private IEnumerator SetDefault(float speed, Enemu target)
    ////{
    ////    //Debug.Log(Mathf.Clamp(power / (power * 20), 0.01f, 10f));
    ////    yield return new WaitForSeconds(Mathf.Clamp(power / (power * 20), 0.01f, 10f));
    ////    if (target)
    ////        target.Agent.speed = speed;
    ////}

}
