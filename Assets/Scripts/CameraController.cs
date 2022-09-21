using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Я изменение
//
// ылдфваофвыдлаодфлывоа
//
//
//
// фывафвыафвыа
//

public class CameraController : MonoBehaviour
{
    [SerializeField] private AnimationCurve interpolator;
    [SerializeField] private float zoomSpeed;
    [SerializeField, Range(0.01f, 1f)] private float speed;
    [SerializeField] private float minSpeed, maxSpeed;
    [SerializeField, Range(1f, 6f)] private float zoom;
    [SerializeField] private float minZoom, maxZoom;

    [SerializeField] private Camera cam;
    private Vector2 startPos, currentPos, result = Vector2.zero;
    [SerializeField] private Transform pos_1, pos_2;
    [SerializeField] private float fixedZ;
    [SerializeField] private bool block = false;
    [SerializeField] private bool useAcceleration;


    private void Awake()
    {
        pos_1 = GameObject.FindGameObjectWithTag("Pos1").transform;
        pos_2 = GameObject.FindGameObjectWithTag("Pos2").transform;
    }

    private void Start()
    {


        cam = gameObject.GetComponent<Camera>();
        StartCoroutine(Life());
    }

    public void SetBlock(bool val)
    {
        block = val;
    }

    private IEnumerator WaitAcceleration()
    {
        yield return new WaitForSeconds(0.5f);
        result = Vector2.zero;
        block = false;
    }

    private IEnumerator Life()
    {

        if (block)
        {
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(Life());
            yield break;
        }

        if (result != Vector2.zero)
        {
            transform.position =
                Vector3.Lerp(
                    transform.position,
                    new Vector3(
                        Mathf.Clamp(result.x, pos_1.position.x, pos_2.position.x),
                        Mathf.Clamp(result.y, pos_1.position.y, pos_2.position.y),
                        fixedZ),
                    interpolator.Evaluate(speed));
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(Life());
        yield break;
    }

    public void ChangeSpeed(float multipler)
    {
        LevelData.levelPresset.Sensitivity = multipler;
    }
    [SerializeField] private float persent;

    public void AddZoom()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomSpeed, minZoom, maxZoom);
    }
    public void DivideZoom()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoomSpeed, minZoom, maxZoom);
    }

    void Update()
    {
        //persent = Mathf.InverseLerp(minZoom, maxZoom, cam.orthographicSize);

        if (block)
            return;
        if (EventSystem.current)
            if (!EventSystem.current.currentSelectedGameObject)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                if (Input.GetMouseButton(0))// && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 delta = (Vector2)startPos - currentPos;
                    delta = new Vector2(delta.x, delta.y * 2);
                    result = (Vector2)transform.position + delta;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    DOTween.To(() => speed, x => speed = x, maxSpeed * LevelData.levelPresset.Sensitivity, 0.2f).OnKill(() =>
                    {
                        result = Vector2.zero;
                    });
                }

            }


    }
}
