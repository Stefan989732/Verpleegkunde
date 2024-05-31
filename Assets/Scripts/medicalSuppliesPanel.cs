using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class medicalSuppliesPanel : MonoBehaviour
{
    public Button dropdownPanel;
    public GameObject UIElement, Tray, MedicalTrash, Trash;
    private MedicalTrayHover medicalTrayHover;

    void Start()
    {
        Button btn = dropdownPanel.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        medicalTrayHover = Tray.GetComponent<MedicalTrayHover>();
    }

    void TaskOnClick()
    {
        UIElement.SetActive(!UIElement.activeSelf);
        MedicalTrash.SetActive(!MedicalTrash.activeSelf);
        Trash.SetActive(!Trash.activeSelf);
        medicalTrayHover.enabled = false;

        Tray.transform.position = new Vector3(Tray.transform.position.x, Tray.transform.position.y + 0.2f, Tray.transform.position.z);

        // Re-enable the MedicalTrayHover script after a delay
        StartCoroutine(ReEnableMedicalTrayHover());
    }

    IEnumerator ReEnableMedicalTrayHover()
    {
        yield return new WaitForSeconds(0.5f); 
        if (UIElement.activeSelf == false) 
        {
            medicalTrayHover.enabled = true;
        }
    }
}
