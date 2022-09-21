using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable] public class OnMouse : UnityEngine.Events.UnityEvent<ClickEvent> { }
    [System.Serializable] public class OnDown : UnityEngine.Events.UnityEvent<ClickEvent> { }
    [System.Serializable] public class OnUp : UnityEngine.Events.UnityEvent<ClickEvent> { }
    [System.Serializable] public class OnAction : UnityEngine.Events.UnityEvent { }



    public OnMouse OnMouse_ { get => onMouse; set => onMouse = value; }
    public OnAction OnAction_ { get => onAction; set => onAction = value; }
    public OnUp OnUp_ { get => onUp; set => onUp = value; }
    public OnDown OnDown_ { get => onDown; set => onDown = value; }




    //public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    //public Vector2 OriginSize { get => originSize; set => originSize = value; }

    [SerializeField] private OnMouse onMouse;
    [SerializeField] private OnDown onDown;
    [SerializeField] private OnUp onUp;
    [SerializeField] private OnAction onAction;
    [SerializeField] private bool Pointer = false;
    [SerializeField] private bool Updatenvoke = false;

    private bool Down = false;
    //[SerializeField] private SpriteRenderer spriteRenderer;
    //[SerializeField] private Vector2 originSize;

    private void Start()
    {
        //if (spriteRenderer == null) spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //originSize = spriteRenderer.size;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (Pointer)
        //    onClick?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Pointer)
            onDown?.Invoke(this);
        if (Updatenvoke)
            Down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Pointer)
            onUp?.Invoke(this);
        if (Updatenvoke)
            Down = false;
    }

    private void OnMouseDown()
    {
        if (!Pointer)
        {
            onDown?.Invoke(this);
        }
        if (Updatenvoke)
            Down = true;
    }

    private void OnMouseUp()
    {
        if (!Pointer)
            onUp?.Invoke(this);
        if (Updatenvoke)
            Down = false;
    }


    private void FixedUpdate()
    {
        if (Updatenvoke && !Down) return;
        onMouse?.Invoke(this);
    }


}
