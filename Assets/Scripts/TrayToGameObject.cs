using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrayToGameObject : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 offset;
    public LayerMask gameFieldLayerMask; // Assign the game field layer mask in the Inspector

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make UI element semi-transparent
        canvasGroup.blocksRaycasts = false; // Disable raycast blocking
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
        offset = rectTransform.anchoredPosition - offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        rectTransform.anchoredPosition = localPoint + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f; // Reset transparency
        canvasGroup.blocksRaycasts = true; // Enable raycast blocking again

        // Check if the UI element is dropped on the game field layer
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gameFieldLayerMask))
        {
            // Spawn a new GameObject at the dropped position
            Spawner.Instance.Spawn(hit.point);
        }

        // Reset position of the UI element back to its original parent
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
