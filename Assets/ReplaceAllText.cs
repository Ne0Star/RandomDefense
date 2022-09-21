using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReplaceAllText : MonoBehaviour
{
    [SerializeField] private bool replace = false;


    private void OnDrawGizmos()
    {
        if (replace)
        {

            TMP_Text[] texts = FindObjectsOfType<TMP_Text>(true);

            foreach (TMP_Text text in texts)
            {
                GameObject gameObject = text.gameObject;
                string label = text.text;
                RectTransform rect = text.rectTransform;
DestroyImmediate(text);
                Text newText = gameObject.AddComponent<Text>();
                
            }

            replace = false;
        }
    }
}
