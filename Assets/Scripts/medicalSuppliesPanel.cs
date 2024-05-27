using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class medicalSuppliesPanel : MonoBehaviour
{
    public Button dropdownPanel;
    public GameObject UIElement, Tray, MedicalTrash, Trash;
    private MedicalTrayHover medicalTrayHover;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = dropdownPanel.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        // Get the MedicalTrayHover script from the Tray object
        medicalTrayHover = Tray.GetComponent<MedicalTrayHover>();
    }

    void TaskOnClick()
    {
        UIElement.SetActive(!UIElement.activeSelf);
        // Tray.SetActive(!Tray.activeSelf);
        MedicalTrash.SetActive(!MedicalTrash.activeSelf);
        Trash.SetActive(!Trash.activeSelf);

        // Disable the MedicalTrayHover script
        if (medicalTrayHover != null)
        {
            medicalTrayHover.enabled = false;
        }

        // Apply the transform change to the Tray
        Tray.transform.position = new Vector3(Tray.transform.position.x, Tray.transform.position.y + 0.2f, Tray.transform.position.z);

        // Re-enable the MedicalTrayHover script after a delay (if needed)
        // You can use a coroutine if you want to re-enable it after some delay, otherwise, enable it immediately if the panel is closing
        StartCoroutine(ReEnableMedicalTrayHover());
    }

    IEnumerator ReEnableMedicalTrayHover()
    {
        yield return new WaitForSeconds(0.5f); // Optional delay before re-enabling the script
        if (UIElement.activeSelf == false) // Check if the panel is closed
        {
            medicalTrayHover.enabled = true;
        }
    }
}
