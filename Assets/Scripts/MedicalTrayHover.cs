using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MedicalTrayHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position
        initialPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the position when the mouse hovers over the UI element
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change the position back to the initial position when the mouse leaves the UI element
        transform.position = initialPosition;
    }
}
