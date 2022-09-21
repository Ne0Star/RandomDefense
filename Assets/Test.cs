
#if UNITY_EDITOR
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        
    }

    //[MenuItem("Assets/Byu Dealer")]
    //private static void DoSomething()
    //{
    //    Debug.Log("You did something!");
    //}

    //[MenuItem("Assets/Byu Dealer", true)]
    //private static bool DoSomethingValidation()
    //{
    //    return Selection.activeObject.GetType() == typeof(MonoScript);
    //}

    //public bool search = false;

    //[SerializeField]
    //string[] guids = AssetDatabase.FindAssets("t:Wall");

    //private void OnDrawGizmos()
    //{
    //    if (search)
    //    {
    //        foreach (var guid in guids)
    //        {
    //            var path = AssetDatabase.GUIDToAssetPath(guid);
    //            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
    //        }
    //        search = false;
    //    }
    //}





    //public Material imageMaterial;
    //public Material spriteMaterial;
    //public bool use;
    //private void OnDrawGizmos()
    //{
    //    if (use)
    //    {
    //        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
    //        Image[] images = GameObject.FindObjectsOfType<Image>();
    //        foreach (Renderer rend in renderers)
    //        {
    //            rend.material = spriteMaterial;
    //        }
    //        foreach(Image img in images)
    //        {
    //            img.material = imageMaterial;
    //        }
    //        use = false;
    //    }

    //}
}
#endif