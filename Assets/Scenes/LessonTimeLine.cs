using System.Collections;
using UnityEngine;

public class LessonTimeLine : UIPage
{
    [SerializeField] private Transform[] lessons;
    [SerializeField] private int current = 0;
    [SerializeField] private bool next = false;
    public void StartLesson()
    {
        
        current = 0;
        StartCoroutine(Wait(() => { }));
    }
    public void StartLesson(System.Action onComplete)
    {
        current = 0;
        StartCoroutine(Wait(onComplete));
    }
    private void Start()
    {
        foreach(Transform target in transform)
        {
            target.gameObject.SetActive(false);
        }
        StartLesson();
    }

    private IEnumerator Wait(System.Action complete)
    {
        LevelManager.Instance.StopSpeed();
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
        LevelManager.Instance.ResumeSpeed();
        LevelManager.Instance.UiManager.ClosePage(this);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            next = true;
        }
    }
}
