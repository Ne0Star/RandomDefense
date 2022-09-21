using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowChecker : MonoBehaviour
{

    private void Start()
    {
        if (LevelData.levelPresset.UseShadow)
        {
            Destroy(gameObject);
        }
        //MapManager map = FindObjectOfType<MapManager>();
        if(transform.parent)
        {
            //map.
            transform.parent.localScale = new Vector3(LevelData.levelPresset.TreeMultipler, LevelData.levelPresset.TreeMultipler);
        }
    }

}
