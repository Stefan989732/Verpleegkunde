using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MedicalTrayHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.position = initialPosition;
    }
}
