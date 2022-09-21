using UnityEngine;
using UnityEngine.EventSystems;

public class PointerTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool enter;
    public void OnPointerEnter(PointerEventData data)
    {
        enter = true;
    }
    public void OnPointerExit(PointerEventData data)
    {
        enter = false;
    }
}
