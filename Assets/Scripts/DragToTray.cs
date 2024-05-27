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

    // Parent object from which copies are allowed
    public Transform sourceParent;
    public Transform trayParent; // Parent object representing the tray

    // Original position to return to after dragging
    private Vector3 originalPosition;
    private Transform originalParent;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        dragManager = FindObjectOfType<DragToTrayManager>();
        objectTag = gameObject.tag; // Ensure each draggable object has a unique tag set in the Inspector

        // Save the original position and parent
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!dragManager.CanCreateNewInstance(objectTag))
        {
            this.enabled = false;
            
            Debug.Log("Maximum number of copies reached.");
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

        // Create the new instance and reset the original object's position
        if (dragManager.CanCreateNewInstance(objectTag))
        {
            CreateNewInstance();

            // Reset the position and parent of the original object
            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void CreateNewInstance()
    {
        // Instantiate a new object
        GameObject newObject = Instantiate(gameObject, rectTransform.position, Quaternion.identity);

        // Get the RectTransform component of the new object
        RectTransform newRectTransform = newObject.GetComponent<RectTransform>();

        // Set the size and parent for proper rendering and interaction with UI
        newRectTransform.sizeDelta = rectTransform.sizeDelta;
        newRectTransform.SetParent(trayParent, false); // Set the parent to the tray

        // Ensure the new object remains draggable
        DragToTray newDragToTray = newObject.GetComponent<DragToTray>();
        newDragToTray.sourceParent = sourceParent;
        newDragToTray.trayParent = trayParent;

        // Set sibling index to match the original object
        newObject.transform.SetSiblingIndex(transform.GetSiblingIndex());

        // Increment instance count in the manager
        dragManager.IncrementInstanceCount(objectTag);

        // Allow the new object to be dragged freely
        newRectTransform.anchoredPosition = trayParent.InverseTransformPoint(rectTransform.position);
        newObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
