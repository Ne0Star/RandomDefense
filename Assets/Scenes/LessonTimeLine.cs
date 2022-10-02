using System.Collections;
using UnityEngine;

public class LessonTimeLine : UIPage
{
    [SerializeField] private Transform[] lessons;
    [SerializeField] private int current = 0;
    [SerializeField] private bool next = false;
    //public void StartLesson()
    //{

    //    current = 0;
    //    StartCoroutine(Wait(() => { }));
    //}
    public void StartLesson(System.Action onComplete)
    {
        if (!gameObject.activeInHierarchy)
        {
            onComplete();
            return;
        }
        current = 0;
        StartCoroutine(Wait(onComplete));
    }

    private void OnEnable()
    {
        foreach (Transform target in transform)
        {
            target.gameObject.SetActive(false);
        }
    }
    private IEnumerator Wait(System.Action complete)
    {
        //LevelManager.Instance.StopSpeed();
        lessons[current].gameObject.SetActive(true);
        yield return new WaitUntil(() => next);
        next = false;
        lessons[current].gameObject.SetActive(false);
        current++;
        if (current != lessons.Length)
        {
            StartCoroutine(Wait(complete));

            yield break;
        }
        complete();
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            next = true;
        }
    }
}
