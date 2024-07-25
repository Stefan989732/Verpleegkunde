using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class Testing : InputTestFixture
{
    Mouse mouse;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/Venapunctie");
        mouse = InputSystem.AddDevice<Mouse>();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void DragUIElementTest()
    {
        //// Use the Assert class to test conditions
       
         
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Debug.Log(camera);
        GameObject button = GameObject.Find("Canvas/SidePanelText/Fase 1.1");
        Debug.Log(button);
        Vector3 screenPos3 = camera.WorldToScreenPoint(button.transform.position);
        Debug.Log(screenPos3);
        Set(mouse.position, screenPos3);
        Debug.Log(mouse);
        Click(mouse.leftButton);




        GameObject desinfect = GameObject.Find("Canvas/MedicalPanel/MedicalSupplyDesinfection/Desinfection");
        Vector3 screenPos = camera.WorldToScreenPoint(desinfect.transform.position);
        Set(mouse.position, screenPos);
        Press(mouse.leftButton);
        GameObject tray = GameObject.Find("Canvas/MedicalTray");
        Vector3 screenPos2 = camera.WorldToScreenPoint(tray.transform.position);
        Set(mouse.position, screenPos2, time: 5f);
        Debug.Log(mouse.leftButton.IsPressed());
        Release(mouse.leftButton);
        Debug.Log(mouse.leftButton.IsPressed());
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new WaitForSeconds(2f);
    }
}
