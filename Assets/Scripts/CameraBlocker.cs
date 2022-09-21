using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraBlocker : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    private ScrollRect scroll;
    private CameraController controller;

    private void Awake()
    {
        controller = FindObjectOfType<CameraController>();
        scroll = FindObjectOfType<ScrollRect>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            controller.SetBlock(false);
        }
    }




    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        controller.SetBlock(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        controller.SetBlock(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    //private void OnMouseDown()
    //{
    //    Debug.Log(123);
    //    controller.SetBlock(true);
    //}
    //private void OnMouseUp()
    //{
    //    controller.SetBlock(false);
    //}
}
