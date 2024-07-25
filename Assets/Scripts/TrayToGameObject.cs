using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Create a 3D version of the object when dragged from tray
/// <<broken>>
/// </summary>
public class TrayToGameObject : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 offset;
    private RectTransform medicalTray;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (transform.parent != null)
        {
            medicalTray = transform.parent.GetComponent<RectTransform>();

        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; 
        canvasGroup.blocksRaycasts = false;

        offset = rectTransform.position - Camera.main.ScreenToWorldPoint(new Vector3
        (eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane)) + offset;
        rectTransform.position = new Vector3(newPosition.x, newPosition.y, rectTransform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == medicalTray.transform)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(medicalTray, Input.mousePosition, Camera.main))
            {
                // Use the tag of the current gameObject to activate the corresponding object in the scene
                TagManager.TagData tagData = TagManager.Instance.GetTagData(gameObject.tag);
                
                if (tagData != null)
                {
                    // activate gameobject with the same tag and destroy the current object
                    tagData.gameObject.SetActive(true);
                    Destroy(GameObject.Find(gameObject.tag + "(Clone)"));
                }
            }
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
