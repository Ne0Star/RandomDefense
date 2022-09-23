#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelSetting))]
public class LevelSettingEditor : Editor
{

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        Debug.Log("create");
        base.OnPreviewGUI(r, background);
    }

}

public class LevelSetting : MonoBehaviour
{
    [SerializeField] private bool onEnable;
    [SerializeField] private ByuDealer[] dealers;
    [SerializeField] private string path;

    private void OnDrawGizmos()
    {
        if (onEnable)
        {
            dealers = 
        dealers = Resources.LoadAll<ByuDealer>(path);
            onEnable = false;
        }

    }
}
#endif