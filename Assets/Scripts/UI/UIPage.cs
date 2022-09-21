using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIPage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AnimationData animationData;

    [System.Serializable]
    public class OnClosePage : UnityEngine.Events.UnityEvent { }
    [System.Serializable]
    public class OnOpenPage : UnityEngine.Events.UnityEvent { }
    [System.Serializable]
    public class OnUpPage : UnityEngine.Events.UnityEvent { }
    [SerializeField]
    private OnUpPage onUppage;
    [SerializeField]
    private OnOpenPage onOpenpage;
    [SerializeField]
    private OnClosePage onClosepage;

    private RectTransform rect;
    public RectTransform Rect => rect;

    public void OnPointerUp(PointerEventData data)
    {
            onUppage?.Invoke();
    }
    public void OnPointerDown(PointerEventData data)
    {
        
    }
    private void OnEnable()
    {
        onOpenpage?.Invoke();
    }
    private void OnDisable()
    {
        onClosepage?.Invoke();
    }

    void Awake()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }

    [System.Serializable]
    public struct AnimationData
    {
        public Ease easeType;
        public float duration;
        public Vector3 openValue, closeValue;
    }

}
