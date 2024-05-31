using UnityEngine;
using UnityEngine.EventSystems;

public class DragToTray : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Vector2 offset = new Vector2();
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [HideInInspector] public Transform parentAfterDrag;
    private Canvas canvas;

    private DragToTrayManager dragManager;
    private string objectTag;

    public Transform sourceParent;
    public Transform trayParent;

    private Vector3 originalPosition;
    private Transform originalParent;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        dragManager = FindObjectOfType<DragToTrayManager>();
        objectTag = gameObject.tag; 

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!dragManager.CanCreateNewInstance(objectTag))
        {
            this.enabled = false;
            
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localMousePosition);

        offset = rectTransform.anchoredPosition - localMousePosition;
        canvasGroup.blocksRaycasts = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        rectTransform.anchoredPosition = localPoint + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (dragManager.CanCreateNewInstance(objectTag))
        {
            CreateNewInstance();

            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
        }
    }

private void CreateNewInstance()
{
    GameObject newObject = Instantiate(gameObject, rectTransform.position, Quaternion.identity);

    RectTransform newRectTransform = newObject.GetComponent<RectTransform>();

    newRectTransform.sizeDelta = rectTransform.sizeDelta;
    newRectTransform.SetParent(trayParent, false);

    DragToTray newDragToTray = newObject.GetComponent<DragToTray>();
    newDragToTray.sourceParent = sourceParent;
    newDragToTray.trayParent = trayParent;

    newObject.transform.SetSiblingIndex(transform.GetSiblingIndex());

    dragManager.IncrementInstanceCount(objectTag);

    newRectTransform.anchoredPosition = trayParent.InverseTransformPoint(rectTransform.position);
    newObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

    newDragToTray.enabled = false;
    TrayToGameObject trayToGameObject = newObject.GetComponent<TrayToGameObject>();
    if (trayToGameObject == null)
    {
        trayToGameObject = newObject.AddComponent<TrayToGameObject>();
    }
    trayToGameObject.enabled = true;
}

}
