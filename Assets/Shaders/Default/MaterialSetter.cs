using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSetter : MonoBehaviour
{
    public bool change = false;

    public Material img, render;

    private void OnDrawGizmos()
    {
        if (change)
        {
            Image[] images = GameObject.FindObjectsOfType(typeof(Image)) as Image[];
            Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>() as Renderer[];
            foreach (Image i in images)
            {
                i.material = img;
            }
            foreach (Renderer rend in renderers)
            {
                rend.material = render;
            }
            change = false;
        }

    }
}
