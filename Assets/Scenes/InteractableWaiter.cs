using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InteractableWaiter : MonoBehaviour
{
    [SerializeField] private int duration;
    [SerializeField] private bool isWait = false;
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text text;
    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            if (!isWait)
                StartCoroutine(Wait());
        });
    }

    private IEnumerator Wait()
    {

        for (int i = 0; i < duration; i++)
        {
            isWait = true;
            btn.interactable = false;
            text.text = (duration - i).ToString();
            yield return new WaitForSeconds(1f);
        }

        text.text = "";
        btn.interactable = true;
        isWait = false;
    }

}
