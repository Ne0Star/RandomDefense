using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class test : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] particles;
    [SerializeField] private MissilesType missilesType;
    [SerializeField] private AnimationCurve origin;
    [SerializeField] private AnimationCurve exploseCurva;
    [SerializeField] private Ease exploseEase;
    [Header("Explose")]
    [SerializeField] private float exploseMin, exploseMax, exploseDuration;
    [Header("Move to target")]
    [SerializeField] private float min, max, duration;


    private void Awake()
    {

    }

    public void Initial(int count)
    {

    }

    private void Start()
    {
        particles = GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            foreach (Transform p in particles)
            {
                p.position = mousePos;
                AnimationCurve curva = missilesType.SetRandomKeys(origin, min, max);
                AnimationCurve exCurava = missilesType.SetRandomKeys(exploseCurva, exploseMin, exploseMax);
                p.DOMove(p.transform.position + new Vector3(Random.Range(-exploseMin, exploseMax), Random.Range(-exploseMin, exploseMax), 0), exploseDuration).OnComplete(() =>
                {
                    curva = missilesType.SetRandomKeys(origin, min, max);
                    p.DOMove(target.position, duration).SetEase(curva);
                }).SetEase(exploseEase);

            }


        }
    }

}
