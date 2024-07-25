using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Add drag functionality to object
/// </summary>
public class DragToTray : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    // Source and destination objects
    public Transform sourceParentObject;
    public RectTransform destinationParentObject;


    // Offset with mouse position, to keep the object with the mouse
    private Vector2 offset = new Vector2();

    // Info about dragged component
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Parent for drag
    private Canvas canvas;

    // checks if component is draggable
    private DragToTrayManager dragManager;
    private string objectTag;

    // To move the object back when done moving
    private Vector3 originalPosition;
    private Transform originalParent;

    void Start()
    {
        // Get parent for drag
        canvas = GetComponentInParent<Canvas>();
        // Get info about dragged component
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        dragManager = FindObjectOfType<DragToTrayManager>();
        objectTag = gameObject.tag; 

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // is allowed to drag
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

        // Save mouse offset to use on drag
        offset = localMousePosition;
        canvasGroup.blocksRaycasts = false;
        // Move object into canvas while draggin
        transform.SetParent(canvas.transform);
    }

    /// <summary>
    /// Move object to position of mouse while dragging
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        rectTransform.localPosition = localPoint + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check if more instances are allowed
        if (dragManager.CanCreateNewInstance(objectTag))
        {
           
        // Place the clone in the tray
            CloneInstance(gameObject);

            // Move the original component back to its location
            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
            
           
        }   
    }

    /// <summary>
    /// clone the current object and add the TrayToGameObject script to the object
    /// </summary>
    /// <param name="originalGameObject">Object to copy</param>
    private void CloneInstance(GameObject originalGameObject)
    {
        GameObject newObj = Instantiate(originalGameObject, rectTransform.position, Quaternion.identity);

        // Set location
        RectTransform newObjRectTransform = newObj.GetComponent<RectTransform>();
        newObjRectTransform.sizeDelta = rectTransform.sizeDelta;
         if (RectTransformUtility.RectangleContainsScreenPoint(destinationParentObject, Input.mousePosition, Camera.main)) {
        newObjRectTransform.SetParent(destinationParentObject, false);
         }
        // Copy script and disable
        DragToTray newObjDragToTray = newObj.GetComponent<DragToTray>();
        newObjDragToTray.sourceParentObject = sourceParentObject;
        newObjDragToTray.destinationParentObject = destinationParentObject;
        newObjDragToTray.enabled = false;

        // add to parent element
        newObj.transform.SetSiblingIndex(transform.GetSiblingIndex());

        dragManager.IncrementInstanceCount(objectTag);

        // ensures object is placed in destinationParent
        newObjRectTransform.anchoredPosition = destinationParentObject.InverseTransformPoint(rectTransform.position);
        newObj.GetComponent<CanvasGroup>().blocksRaycasts = true;

        // add new script to created instance
        TrayToGameObject trayToGameObject = newObj.GetComponent<TrayToGameObject>();
        if (trayToGameObject == null)
        {
            trayToGameObject = newObj.AddComponent<TrayToGameObject>();
        }
        trayToGameObject.enabled = true;
    }

}
