using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class medicalFileShow : MonoBehaviour
{
    public Button MedicalFile;
    public GameObject MedicalFileShow, MedicalTray, MedicalTrash, Trash, Arm, Tutorial;
    void Start()
    {

        Button btn = MedicalFile.GetComponent<Button>();
		btn.onClick.AddListener(ShowMedicalFile);
    }

// Show the medical file when the button is clicked and hide the other objects
   void ShowMedicalFile(){
        MedicalFileShow.SetActive(!MedicalFileShow.activeSelf);
        MedicalTray.SetActive(!MedicalTray.activeSelf);
        MedicalTrash.SetActive(!MedicalTrash.activeSelf);
        Trash.SetActive(!Trash.activeSelf);
        Arm.SetActive(!Arm.activeSelf);

	}
}
