using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
public class EdgeDrag : MonoBehaviour
{
    public Collider targetCollider; // The collider of the target object whose edges we are following
    public float smoothTime = 0.1f; // Smoothing time for the drag effect


    private Camera cam;
    private bool isDragging = false;
    private Vector3 velocity = Vector3.zero; // Velocity used by SmoothDamp
    private IEnumerator coroutine;
    private bool firstPierceFeedback = false;
    void Start()
    {
        cam = Camera.main;
       
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isDragging = true;
            } 
        } 
        
        if (Input.GetMouseButton(1))
        {
            var otherPosn = transform.position;
            transform.position = new Vector3(otherPosn.x, otherPosn.y, otherPosn.z + 0.000299f);
        }
        if (Input.GetMouseButtonUp(0)) isDragging = false;

        if (isDragging) DragAlongEdge();
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Veins" && firstPierceFeedback == false)
        {
            coroutine = WaitAndPrint(1.5f);
            StartCoroutine(coroutine);

        }
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            firstPierceFeedback = true;
            yield return new WaitForSeconds(waitTime);
            this.enabled = false;
            StopCoroutine(coroutine);
            
        }
    }

    void DragAlongEdge()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (targetCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 targetPosition = hit.point;
            Vector3 lookAt = Vector3.Cross(-hit.normal, transform.right);
            lookAt = lookAt.y < 0 ? -lookAt : lookAt;
            
            // Use SmoothDamp for smooth dragging
            transform.rotation = Quaternion.LookRotation(hit.point + lookAt, hit.normal);
            transform.position = Vector3.SmoothDamp(transform.position , hit.point,ref velocity, smoothTime);
        }
    }
}
