
using UnityEngine;

/// <summary>
/// Отвечает за выделение объектов
/// Создание меню для вызова действий
/// </summary>
public class ActionDealerManager : MonoBehaviour
{
    [System.Serializable]
    public class OnSelectEntity : UnityEngine.Events.UnityEvent<Entity> { }

    //[System.Serializable]
    //public class OnSelectNull : UnityEngine.Events.UnityEvent<Vector3> { }


    [Header("Системные")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float startZPos;
    [SerializeField] private TileData selected;
    [SerializeField] private Transform actionsParent;
    [Header("Animation Setting")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float tempAngle;
    [SerializeField] private float maxDistance;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 startScale;
    [SerializeField] private MapManager map;
    [SerializeField] private ClickEvent[] dealers;
    [Header("Events ")]
    [SerializeField] private OnSelectEntity onSelectEntity;
    //[SerializeField] private OnSelectNull onSelectNull;
    //[Header("Другое")]
    //[SerializeField] private CameraController camController;

    private void Awake()
    {
        if (map == null)
            map = GameObject.FindObjectOfType<MapManager>();
        //if (camController == null)
        //   camController = GameObject.FindObjectOfType<CameraController>();

        dealers = gameObject.GetComponentsInChildren<ClickEvent>();

        startZPos = transform.position.z;
        startScale = transform.localScale;
        tempAngle = maxAngle;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        foreach (ClickEvent dealer in dealers)
        {
            dealer.OnDown_?.AddListener(Click);
        }
    }

    private void Click(ClickEvent dealer)
    {

    }

    public void SellTurret()
    {
        if (selected != null)
        {

            if (selected.Entity_ as Wall)
            {
                Wall wall = (Wall)selected.Entity_;
                if (wall)
                {
                    if (wall.Turret)
                    {
                        LevelManager.Instance.ByuManager.GetBalance().SellItem(wall.Turret.CostData);

                        Destroy(wall.Turret.gameObject);
                    }
                }
            }
        }


        SetAction(false);
        selected = null;
    }
    public void SellWall()
    {
        if (selected != null)
            LevelManager.Instance.ByuManager.GetBalance().SellItem((EntityUnit)selected.Entity_, true);

        SetAction(false);
        selected = null;
    }

    public void UpdateSelected()
    {

    }

    [SerializeField] private float scaleMultipler;

    private void Update()
    {
        if (isOpen)
        {
            float angle = (tempAngle / (dealers.Length));
            float scale = (1f + (Camera.main.orthographicSize / 3)) * scaleMultipler;
            transform.localScale = new Vector3(scale, scale, scale);
            actionsParent.localScale = new Vector3(scale / 1.5f, scale / 1.5f, scale / 1.5f);
            //distance = 2f - scale;
            for (int i = 0; i < dealers.Length; i++)
            {
                Transform dealer = dealers[i].transform;

                float x = (scale / 4) * Mathf.Cos((angle * i) * Mathf.Deg2Rad);
                float y = (scale / 4) * Mathf.Sin((angle * i) * Mathf.Deg2Rad);
                dealer.position = transform.position + new Vector3(x, y, 0);
            }
        }

        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //if (isOpen) return;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TileData entityData = map.GetTileData(map.WallMap, mousePos);
            TileData entityData_ = map.GetTileData(map.GeneratorMap, mousePos);
            if (entityData_ != null)
            {
                OpenActions(entityData_);
            }
            else
            if (entityData != null)
            {
                //camController.MoveToPosition(entityData.Map.GetCellCenterWorld(entityData.Position_));
                OpenActions(entityData);
            }
            else
            {
                //camController.MoveToPosition(map.WallMap.GetCellCenterWorld(map.WallMap.WorldToCell(mousePos)));
                Cansel();
            }

        }
        else
        {
            return;
        }
    }
    private void FixedUpdate()
    {
        if (selected == null || selected.Entity_ == null)
        {
            Cansel();
        }
    }
    public void Cansel()
    {
        SetActiveSelected(false);
    }

    private void SetActiveSelected(bool val)
    {
        SetAction(val);
    }


    private void SetAction(bool val)
    {
        if (selected == null) { return; }
        if (selected.Entity_ as EntityUnit)
        {
            EntityUnit unit = (EntityUnit)selected.Entity_;
            if (unit.Hit)
                unit.Hit.gameObject.SetActive(val);
        }
        if (selected.Entity_ as Turret)
        {
            Turret t = (Turret)selected.Entity_;
            if (t)
            {
                if (val)
                    t.ShowRadius();
                else t.HideRadius();
            }
        }
        if (selected.Entity_ as Wall)
        {
            Wall w = (Wall)selected.Entity_;
            if (w)
            {
                if (w.Turret)
                {
                    if (val)
                        w.Turret.ShowRadius();
                    else w.Turret.HideRadius();
                }
            }
        }

        if (!val)
        {
            actionsParent.gameObject.SetActive(false);
            transform.localScale = Vector3.zero;
            isOpen = false;
            distance = 0;
            selected = null;
            isOpen = false;
        }
    }


    private void OpenActions(TileData newSelected)
    {
        if (selected != newSelected)
        {
            SetActiveSelected(false);
            selected = newSelected;
        }
        SetActiveSelected(true);
        transform.localScale = startScale;
        transform.position = newSelected.Map.GetCellCenterWorld(newSelected.Position_);
        transform.position = new Vector3(transform.position.x, transform.position.y, startZPos);
        distance = maxDistance;
        actionsParent.gameObject.SetActive(true);
        isOpen = true;
    }

    //[System.Serializable]
    //public class OnSelect : UnityEngine.Events.UnityEvent<TileBehaviour> { }
    //[System.Serializable]
    //public class OnCansel : UnityEngine.Events.UnityEvent { }

    //[SerializeField] private LevelManager levelMAnager;

    //[SerializeField] private OnSelect onSelect;
    //[SerializeField] private OnCansel onCansel;
    //[SerializeField] private SpriteRenderer spriteRenderer;
    //[SerializeField] private CameraController camController;

    //[SerializeField] private bool autoSize;

    //[SerializeField] private float outDuration;
    //[SerializeField] private Ease outEase;
    //[SerializeField] private float openDuration;
    //[SerializeField] private Ease openEase;

    //[SerializeField] private MapManager map;
    //[SerializeField] private ActionDealer[] dealers;

    //[SerializeField] private ActionDealer canselDealer, sellDealer;

    //[SerializeField] private bool blocker = false;

    //[SerializeField] private float maxAngle;
    //[SerializeField] private float tempAngle;
    //[SerializeField] private float maxDistance;
    //[SerializeField] private float distance;
    //[SerializeField] private float startZPos;
    //[SerializeField] private Vector3 startScale;
    //[SerializeField] private TileBehaviour selected;
    //[SerializeField] private Vector3Int lastPos;
    //[SerializeField] AnimationType animationType;

    //private void Start()
    //{
    //    //maxPersent = radiusPersent;
    //    camController = FindObjectOfType<CameraController>();
    //    startZPos = transform.position.z;
    //    startScale = transform.localScale;
    //    tempAngle = maxAngle;
    //    dealers = gameObject.GetComponentsInChildren<ActionDealer>();
    //    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    //    levelMAnager = LevelManager.Instance;
    //    //originSize = spriteRenderer.bounds.size;
    //    foreach (ActionDealer dealer in dealers)
    //    {
    //        dealer.OnClick_?.AddListener(Click);
    //    }
    //}
    //public static float CalculateScaleExtents(SpriteRenderer renderer)
    //{
    //    float ScaleX = renderer.bounds.extents.x;
    //    float ScaleY = renderer.bounds.extents.y;
    //    float result = ScaleX > ScaleY ? ScaleX : ScaleY;
    //    return result;
    //}

    //private void Click(ActionDealer dealer)
    //{
    //    Debug.Log(dealer.name + " вызывал своё событие");
    //    blocker = true;
    //    StartCoroutine(WaitAction());
    //}

    //private IEnumerator WaitAction()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    blocker = false;
    //}

    //private void Update()
    //{
    //    if (blocker || levelMAnager.IsWaveStart) return;
    //    if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
    //    {
    //        blocker = true;
    //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        camController?.MoveToPosition(mousePos);
    //        Vector3Int gridPos = map.WallMap.WorldToCell(mousePos);
    //        TileBase tile = map.WallMap.GetTile(gridPos);
    //        TileSpawner d = map.GetTileSpawner(map.WallMap, gridPos);

    //        if (d && tile) // Стена построенная игроком
    //        {
    //            TileBehaviour data = d.tileBehaviour;
    //            OpenActions(gridPos, data);
    //        }
    //        else if (tile && !d) // Стена без поведения
    //        {
    //            blocker = false;
    //            return;
    //        }
    //        // Стена которая была поставлена в редакторе
    //        //OpenActions(gridPos, null);
    //        blocker = false;
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}

    //public void Cansel()
    //{
    //    //camController.StopMoveAndReturnPosition();
    //    if (animationType == AnimationType.Type_0)
    //    {
    //        // Дистанция возвращается на место
    //        DOTween.To(() => distance, x => distance = x, 0, openDuration).SetEase(openEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            transform.localScale = Vector3.zero;
    //        });
    //    }
    //    else if (animationType == AnimationType.Type_1)
    //    {
    //        // Круг сворачивается рулеткой
    //        DOTween.To(() => tempAngle, x => tempAngle = x, 0, outDuration).SetEase(outEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            transform.localScale = Vector3.zero;
    //        });
    //    }
    //    else if (animationType == AnimationType.Type_2)
    //    {
    //        // Круг сворачивается рулеткой
    //        DOTween.To(() => tempAngle, x => tempAngle = x, 0, outDuration).SetEase(outEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            // Дистанция возвращается на место
    //            DOTween.To(() => distance, x => distance = x, 0, openDuration).SetEase(openEase).OnKill(() =>
    //            {

    //            }).OnKill(() =>
    //            {
    //                transform.localScale = Vector3.zero;
    //            });
    //        });
    //    }
    //}
    //private void Cansel(bool reOpen, Vector3Int pos)
    //{
    //    if (animationType == AnimationType.Type_0)
    //    {
    //        // Дистанция возвращается на место
    //        DOTween.To(() => distance, x => distance = x, 0, openDuration).SetEase(openEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            transform.localScale = Vector3.zero;
    //            if (reOpen)
    //            {
    //                OpenActions(pos, tileSpawner);
    //            }
    //        });
    //    }
    //    else if (animationType == AnimationType.Type_1)
    //    {
    //        // Круг сворачивается рулеткой
    //        DOTween.To(() => tempAngle, x => tempAngle = x, 0, outDuration).SetEase(outEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            transform.localScale = Vector3.zero;
    //            if (reOpen)
    //            {
    //                OpenActions(pos, tileSpawner);
    //            }
    //        });
    //    }
    //    else if (animationType == AnimationType.Type_2)
    //    {
    //        // Круг сворачивается рулеткой
    //        DOTween.To(() => tempAngle, x => tempAngle = x, 0, outDuration).SetEase(outEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            // Дистанция возвращается на место
    //            DOTween.To(() => distance, x => distance = x, 0, openDuration).SetEase(openEase).OnKill(() =>
    //            {

    //            }).OnKill(() =>
    //            {
    //                transform.localScale = Vector3.zero;
    //                if (reOpen)
    //                {
    //                    OpenActions(pos, tileSpawner);
    //                }
    //            });
    //        });
    //    }
    //}

    //private enum AnimationType
    //{
    //    Type_0,
    //    Type_1,
    //    Type_2
    //}

    //public void Sell()
    //{
    //    if (selected != null)
    //    {
    //        //LevelManager.Instance.GetBalance().SellItem(selected.Prefab,true);
    //        onCansel?.Invoke();
    //    }
    //    Cansel();
    //}

    //private void OpenActions(Vector3Int pos, TileBehaviour tileSpawner)
    //{
    //    if (selected != tileSpawner)
    //    {



    //        selected = tileSpawner;

    //    }
    //    if (tileSpawner == null)
    //    {
    //        canselDealer.gameObject.SetActive(false);
    //        sellDealer.gameObject.SetActive(false);
    //        spriteRenderer.enabled = false;

    //        //onCansel?.Invoke();
    //        if (lastPos != pos)
    //        {
    //            Cansel(true, pos, tileSpawner);
    //            lastPos = pos;
    //            return;
    //        }
    //    }
    //    onSelect?.Invoke(selected);
    //    spriteRenderer.enabled = true;
    //    canselDealer.gameObject.SetActive(true);
    //    sellDealer.gameObject.SetActive(true);

    //    transform.localScale = startScale;

    //    if (animationType == AnimationType.Type_0)
    //    {
    //        transform.position = map.WallMap.GetCellCenterWorld(pos);//Camera.main.WorldToScreenPoint();
    //        transform.position = new Vector3(transform.position.x, transform.position.y, startZPos);
    //        // Дистанция растёт
    //        DOTween.To(() => distance, x => distance = x, maxDistance, outDuration).SetEase(outEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            blocker = false;
    //        });

    //    }
    //    else if (animationType == AnimationType.Type_1)
    //    {
    //        transform.position = map.WallMap.GetCellCenterWorld(pos);//Camera.main.WorldToScreenPoint();
    //        transform.position = new Vector3(transform.position.x, transform.position.y, startZPos);
    //        // Круг развертывается
    //        DOTween.To(() => tempAngle, x => tempAngle = x, maxAngle, outDuration).SetEase(outEase).OnKill(() =>
    //        {
    //            maxAngle = maxAngle * -1;
    //            blocker = false;
    //        });

    //    }
    //    else if (animationType == AnimationType.Type_2)
    //    {
    //        transform.position = map.WallMap.GetCellCenterWorld(pos);//Camera.main.WorldToScreenPoint();
    //        transform.position = new Vector3(transform.position.x, transform.position.y, startZPos);
    //        // Дистанция растёт
    //        DOTween.To(() => distance, x => distance = x, maxDistance, outDuration).SetEase(outEase).OnKill(() =>
    //        {

    //        }).OnKill(() =>
    //        {
    //            // Круг развертывается
    //            DOTween.To(() => tempAngle, x => tempAngle = x, maxAngle, outDuration).SetEase(outEase).OnKill(() =>
    //            {
    //                maxAngle = maxAngle * -1;
    //                blocker = false;
    //            });
    //        });

    //    }







    //}

    //private void FixedUpdate()
    //{
    //    //radius = ((Screen.width - Screen.height) / 100 * radiusPersent);// + (Screen.height / 100 * radiusPersent);
    //    if (autoSize)
    //    {
    //        float angle = (tempAngle / (dealers.Length));
    //        for (int i = 0; i < dealers.Length; i++)
    //        {
    //            Transform dealer = dealers[i].transform;

    //            float x = distance * Mathf.Cos((angle * i) * Mathf.Deg2Rad);
    //            float y = distance * Mathf.Sin((angle * i) * Mathf.Deg2Rad);
    //            dealer.position = transform.position + new Vector3(x, y, 0);
    //        }
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    float angle = (tempAngle / (dealers.Length));
    //    for (int i = 0; i < dealers.Length; i++)
    //    {
    //        if (!dealers[i]) return;
    //        Transform dealer = dealers[i].transform;

    //        float x = distance * Mathf.Cos((angle * i) * Mathf.Deg2Rad);
    //        float y = distance * Mathf.Sin((angle * i) * Mathf.Deg2Rad);
    //        dealer.position = transform.position + new Vector3(x, y, 0);
    //    }
    //}
}
