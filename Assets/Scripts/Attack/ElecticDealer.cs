using System.Collections;
using UnityEngine;

public class ElecticDealer : MonoBehaviour
{

    [SerializeField] private float offsetMultipler;
    [SerializeField] private float attackSpeed, animationSpeed;
    [SerializeField] private LineRenderer tentacle;
    [SerializeField] private EntityUnit selected;
    [SerializeField] private AttackEffect attackEffect;
    [SerializeField] private AnimationCurve offsetX, offsetY;
    [SerializeField] private float minRand, maxRand;
    [SerializeField] private bool randX, randY;
    [SerializeField] private float damage;
    [SerializeField] private float radius;
    private void Awake()
    {
        StartCoroutine(Life());
    }

    public void SetTarget(EntityUnit target, float damage, float radius)
    {
        this.selected = target;
        this.damage = damage;
        this.radius = radius;
    }

    [SerializeField] private Vector3[] points;


    private void SetPositions(int index, Vector3 vector)
    {
        for (int i = index; i < tentacle.positionCount; i++)
        {
            tentacle.SetPosition(i, vector);
        }
    }
    public AnimationCurve SetRandomKeys(ref AnimationCurve origin, float minValue, float maxValue)
    {
        AnimationCurve result = new AnimationCurve();
        Keyframe[] keys = origin.keys;
        for (int I = 0; I < keys.Length; I++)
        {
            if (I > 0 && I < keys.Length - 1)
            {
                Keyframe key = keys[I];
                key.value = Random.Range(key.value - minValue, key.value + maxValue) * -1f;
                keys[I] = key;
            }
        }
        result.keys = keys;
        return result;
        //origin = result;
    }

    private bool blocker = false;

    private IEnumerator Life()
    {
        //SetPositions(0, transform.position);
        if (selected && selected.gameObject.activeInHierarchy && !blocker && Vector2.Distance(selected.transform.position, transform.position) <= radius)
        {
            blocker = true;
            int trajectoryDetails = 10;
            float time = 0;
            float distance = Vector2.Distance(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(selected.transform.position.x, selected.transform.position.y, 0));
            float distanceStep = distance / trajectoryDetails; // 0.2 Шаг с которым надо двигаться (Процент от дистанции, перевести в время)
            float persent = distanceStep * 100 / distance;
            float timeStep = ((1.1f / 100) * persent);

            tentacle.positionCount = trajectoryDetails;

            for (int i = 0; i < trajectoryDetails; i++)
            {


                AnimationCurve Y = offsetY, X = offsetX;

                if (randY)
                    Y = SetRandomKeys(ref offsetY, minRand, maxRand);
                if (randX)
                    X = SetRandomKeys(ref offsetX, minRand, maxRand);
                //
                float x = X.Evaluate(time);
                float y = Y.Evaluate(time);
                if (selected)
                    if (selected.gameObject && Vector2.Distance(selected.transform.position, transform.position) <= radius)
                    {
                        Vector3 pathVector = Vector3.Lerp(transform.position, selected.transform.position + new Vector3(x, y, 0) * offsetMultipler, time);

                        //tentacle.SetPosition(i, pathVector);

                        float tempTime = 0;
                        while (Vector2.Distance(new Vector3(tentacle.GetPosition(i).x, tentacle.GetPosition(i).y, 0), pathVector) > 0.01f && Vector2.Distance(selected.transform.position, transform.position) <= radius)
                        {
                            tempTime += animationSpeed * Time.unscaledDeltaTime;

                            Vector3 result = Vector3.Lerp(tentacle.GetPosition(i), pathVector, tempTime);
                            SetPositions(i, result);
                            yield return new WaitForSeconds(0.0001f);
                        }
                    }

                time += timeStep;
            }
                selected.TakeDamage(damage, 0.1f, transform.position);
            
            tentacle.positionCount = 0;
        }



        blocker = false;
        yield return new WaitForSeconds(attackSpeed / LevelManager.Instance.GameSpeed);
        StartCoroutine(Life());
    }

}
