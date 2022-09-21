using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет айтемами
/// </summary>
public abstract class Tape : MonoBehaviour
{
    [SerializeField] protected BYU_Dealer refreshCost;
    [SerializeField] private RectTransform rect;

    public RectTransform Rect { get => rect; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

    }
    private void OnEnable()
    {
        if (!rect) rect = GetComponent<RectTransform>();
        refreshCost.UpdateData();
    }
    public void BuyRefresh()
    {
        bool buy = LevelManager.Instance.ByuManager.GetBalance().BuyItem(refreshCost.Gold, refreshCost.Energy, refreshCost.Component);
        if (buy)
        {
            RefreshAll();
            refreshCost.Gold += Random.Range(0f, 0.5f);
            refreshCost.Energy += Random.Range(0f, 0.3f);
            refreshCost.Component += Random.Range(0f, 0.1f);
        }
        refreshCost.UpdateData();
    }
    /// <summary>
    /// Устанавливает контент
    /// </summary>
    /// <param name="newContents"></param>
    public virtual void SetContents(List<ByuDealer> newContents)
    {

    }
    /// <summary>
    /// Распределяет заранее установленный контент по ячейкам
    /// </summary>
    public virtual void RefreshAll()
    {

    }
}

public class TapeDrag : Tape
{
    [System.Serializable]
    public class OnStartDrag : UnityEngine.Events.UnityEvent<TapeDragItem, ByuDealer> { }
    [System.Serializable]
    public class OnEndDrag : UnityEngine.Events.UnityEvent { }

    [SerializeField] private OnStartDrag startDrag;
    [SerializeField] private OnEndDrag endDrag;

    [SerializeField] private bool useRare;

    [SerializeField] private ByuDealer[] tapeContents;

    [SerializeField] private TapeDragItem[] tapeItems;

    [SerializeField] private SpriteRenderer turretRadiusPreview;
    [SerializeField] private SpriteRenderer shadow_Bg, shadow_Img;

    [SerializeField] private bool blocker = false;
    [SerializeField] private bool isMove = false;


    public OnStartDrag StartDrag => startDrag;
    public OnEndDrag EndDrag => endDrag;


    public void Awake()
    {
        shadow_Bg = new GameObject().AddComponent<SpriteRenderer>();
        shadow_Img = new GameObject().AddComponent<SpriteRenderer>();
        shadow_Bg.sortingOrder = 50;
        shadow_Img.sortingOrder = 51;
        shadow_Img.transform.SetParent(shadow_Bg.transform);

        tapeItems = gameObject.GetComponentsInChildren<TapeDragItem>();
        foreach (TapeDragItem item in tapeItems)
        {
            item.onDown?.AddListener(BlockItems);
        }
    }

    private void Start()
    {
        useRare = LevelManager.Instance.LevelPresset.RareType == RareType.Type_3 ? false : true;
        if (refreshCost.Energy_Text)
            refreshCost.Energy_Text.text = refreshCost.Energy + "";
        if (refreshCost.Component_Text)
            refreshCost.Component_Text.text = refreshCost.Component + "";
        if (refreshCost.Gold_Text)
            refreshCost.Gold_Text.text = refreshCost.Gold + "";
    }

    private void BlockItems(TapeDragItem item, ByuDealer spawning)
    {
        if (LevelManager.Instance.ByuManager.GetBalance().isByu(spawning))
        {
            if (blocker) return;
            blocker = true;
            startDrag?.Invoke(item, spawning);
            shadow_Bg.gameObject.SetActive(true);
            if (spawning.GetImageBG())
                shadow_Bg.sprite = spawning.GetImageBG();
            if (spawning.GetImage())
            {
                shadow_Img.gameObject.SetActive(true);
                shadow_Img.sprite = spawning.GetImage();
            }

            StartCoroutine(WaitUp(item, spawning));
        }
        else
        {

        }
    }
    private IEnumerator Move(Transform temp, ByuDealer currentCreator)
    {
        bool radius = false;
        if (currentCreator.Prefab as Turret)
        {
            Turret t = (Turret)currentCreator.Prefab;
            if (t.GetTarretRadius() > 0.01f)
            {
                radius = true;
                turretRadiusPreview.transform.gameObject.SetActive(true);
                turretRadiusPreview.transform.localScale = new Vector3(t.GetTarretRadius(), t.GetTarretRadius(), t.GetTarretRadius());
            }
        }
        while (isMove)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            temp.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            if (radius)
            {
                turretRadiusPreview.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }



    private IEnumerator WaitUp(TapeDragItem currentItem, ByuDealer currentCreator)
    {
        isMove = true;

        StartCoroutine(Move(shadow_Bg.transform, currentCreator));
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        isMove = false;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //ByuDealer c = Instantiate(currentCreator);

        Entity spawning = currentCreator.ByuAndSpawn(LevelManager.Instance.ByuManager.GetBalance(), mousePos, Quaternion.identity, null);

        //c.Spawn(new Vector3(mousePos.x, mousePos.y, 0), () => complete = true, () => cansel = true);

        if(spawning)
        CompleteSpawn(currentItem);

        shadow_Bg.gameObject.SetActive(false);
        shadow_Img.gameObject.SetActive(false);
        turretRadiusPreview.gameObject.SetActive(false);
        //Destroy(spawning.gameObject);

        blocker = false;
        endDrag?.Invoke();
    }



    private void CompleteSpawn(TapeItem item)
    {
        item.SetRandomItemRare(tapeContents);
    }

    /// <summary>
    /// Перемешивает заранее установленный контент
    /// </summary>
    public override void RefreshAll()
    {
        foreach (TapeItem item in tapeItems)
        {
            if (useRare)
            {
                item.SetRandomItemRare(tapeContents);
            }
            else
            {
                item.SetRandomItem(tapeContents);
            }

        }
    }
    /// <summary>
    /// Устанавливает лист контента
    /// </summary>
    /// <param name="newContents"></param>
    public override void SetContents(List<ByuDealer> newContents)
    {
        this.tapeContents = newContents.ToArray();
    }
}

