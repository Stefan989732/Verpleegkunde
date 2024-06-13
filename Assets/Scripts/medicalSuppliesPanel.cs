using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class medicalSuppliesPanel : MonoBehaviour
{
    public Button dropdownPanel;
    public GameObject MedicalSupplyPanel, Tray, MedicalTrash, Trash;
    private MedicalTrayHover medicalTrayHover;

    void Start()
    {
        Button btn = dropdownPanel.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        medicalTrayHover = Tray.GetComponent<MedicalTrayHover>();
    }

    void TaskOnClick()
    {
        MedicalSupplyPanel.SetActive(!MedicalSupplyPanel.activeSelf);
        MedicalTrash.SetActive(!MedicalTrash.activeSelf);
        Trash.SetActive(!Trash.activeSelf);
        medicalTrayHover.enabled = false;


        Tray.transform.position = new Vector3(Tray.transform.position.x, Tray.transform.position.y + 0.2f, Tray.transform.position.z);

        // Re-enable the MedicalTrayHover script after a delay
        StartCoroutine(ReEnableMedicalTrayHover());
    }

    IEnumerator ReEnableMedicalTrayHover()
    {
        yield return new WaitForSeconds(0); 
        if (MedicalSupplyPanel.activeSelf == false) 
        {
            medicalTrayHover.enabled = true;
            Tray.transform.position = new Vector3(Tray.transform.position.x, Tray.transform.position.y - 0.4f, Tray.transform.position.z);
        }
    }
}
