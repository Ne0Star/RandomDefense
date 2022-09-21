using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInvisibler : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void StartWait()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        canvas.enabled = false;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        canvas.enabled = true;
        yield break;
    }

}
