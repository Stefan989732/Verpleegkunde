using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrayItemsSnap : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            DragToTray dragToTray = droppedObject.GetComponent<DragToTray>();
            if (dragToTray != null)
            {
                // Set the new parent to this transform (MedicalTray)
                droppedObject.transform.SetParent(transform);
                dragToTray.parentAfterDrag = transform;  // Update the parentAfterDrag

                // Correct the position in the new parent
                RectTransform droppedRectTransform = droppedObject.GetComponent<RectTransform>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPoint);

                droppedRectTransform.anchoredPosition = localPoint;
            }
        }
    }
}
