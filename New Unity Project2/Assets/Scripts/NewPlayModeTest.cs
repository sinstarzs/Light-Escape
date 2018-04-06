
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewPlayModeTest {

    [UnityTest]
    public IEnumerator Player_Move_NoInput()
    {
        var playerPrefab = Resources.Load("Player 2");
        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;
        var playerController = runner.GetComponent<KwPlayer>();
        Assert.IsFalse(playerController.playerMoving);

        yield return null;
    }


    [UnityTest]
    public IEnumerator Joystick_DragUp()
    {
        var joyStickPrefab = Resources.Load("Fixed Joystick");
        var joyStickObject = PrefabUtility.InstantiatePrefab(joyStickPrefab) as GameObject;

        var joyStick = joyStickObject.GetComponent<FixedJoystick>();
        PointerEventData pointer = new PointerEventData(EventSystem.current);

        pointer.position = new Vector3(10, 0, 0);

        ExecuteEvents.Execute(joyStickObject.gameObject, pointer, ExecuteEvents.pointerDownHandler);


        yield return new WaitForSeconds(1);

        bool testValue = joyStick.Horizontal > 0;

        Assert.IsTrue(testValue);

    }

    [UnityTest]
    public IEnumerator Joystick_DragDown()
    {
        var joyStickPrefab = Resources.Load("Fixed Joystick");
        var joyStickObject = PrefabUtility.InstantiatePrefab(joyStickPrefab) as GameObject;

        var joyStick = joyStickObject.GetComponent<FixedJoystick>();
        PointerEventData pointer = new PointerEventData(EventSystem.current);

        pointer.position = new Vector3(-10, 0, 0);

        ExecuteEvents.Execute(joyStickObject.gameObject, pointer, ExecuteEvents.pointerDownHandler);


        yield return new WaitForSeconds(1);

        bool testValue = joyStick.Horizontal < 0;

        Assert.IsTrue(testValue);
    }

    [UnityTest]
    public IEnumerator Joystick_DragLeft()
    {
        var joyStickPrefab = Resources.Load("Fixed Joystick");
        var joyStickObject = PrefabUtility.InstantiatePrefab(joyStickPrefab) as GameObject;

        var joyStick = joyStickObject.GetComponent<FixedJoystick>();
        PointerEventData pointer = new PointerEventData(EventSystem.current);

        pointer.position = new Vector3(0, -10, 0);

        ExecuteEvents.Execute(joyStickObject.gameObject, pointer, ExecuteEvents.pointerDownHandler);


        yield return new WaitForSeconds(1);

        bool testValue = joyStick.Vertical < 0;

        Assert.IsTrue(testValue);
    }

    [UnityTest]
    public IEnumerator Joystick_DragRight()
    {
        var joyStickPrefab = Resources.Load("Fixed Joystick");
        var joyStickObject = PrefabUtility.InstantiatePrefab(joyStickPrefab) as GameObject;

        var joyStick = joyStickObject.GetComponent<FixedJoystick>();
        PointerEventData pointer = new PointerEventData(EventSystem.current);

        pointer.position = new Vector3(0, 10, 0);

        ExecuteEvents.Execute(joyStickObject.gameObject, pointer, ExecuteEvents.pointerDownHandler);


        yield return new WaitForSeconds(1);

        bool testValue = joyStick.Vertical > 0;

        Assert.IsTrue(testValue);
    }

    [UnityTest]
    public IEnumerator Player_MoveUp()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.updateTransform(0, 1, runnerController.transform);

        yield return new WaitForSeconds(1);


        Assert.IsTrue(runnerController.transform.position.y > 0);
    }

    [UnityTest]
    public IEnumerator Player_MoveDown()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.updateTransform(0, -1, runnerController.transform);

        yield return new WaitForSeconds(1);


        Assert.IsTrue(runnerController.transform.position.y < 0);
    }

    [UnityTest]
    public IEnumerator Player_Moveleft()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.updateTransform(-1, 0, runnerController.transform);

        yield return new WaitForSeconds(1);


        Assert.IsTrue(runnerController.transform.position.x < 0);

    }

    [UnityTest]
    public IEnumerator Player_MoveRight()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.updateTransform(1, 0, runnerController.transform);

        yield return new WaitForSeconds(1);


        Assert.IsTrue(runnerController.transform.position.x > 0);



    }

    [UnityTest]
    public IEnumerator Player_CameraSetting()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        yield return new WaitForSeconds(1);

        if (runnerController.isLocalPlayer)
        {
            Assert.IsTrue(runnerController.gameObject.GetComponentInChildren<Camera>().enabled);
        }
        else
        {
            Assert.IsFalse(runnerController.gameObject.GetComponentInChildren<Camera>().enabled);
        }
    }

    [UnityTest]
    public IEnumerator PointLightToggle_Hidder()
    {

        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.PointLightToggle("hider");
        yield return new WaitForSeconds(1);

        float intensity = runnerController.transform.GetChild(1).gameObject.GetComponent<Light>().intensity;

        Assert.AreEqual(0f, intensity);

    }

    [UnityTest]
    public IEnumerator PointLightToggle_Chaser()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runner2 = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.PointLightToggle("chaser");
        yield return new WaitForSeconds(1);

        float intensity = runnerController.transform.GetChild(1).gameObject.GetComponent<Light>().intensity;

        Assert.AreNotEqual(0f, intensity);
    }

    [UnityTest]
    public IEnumerator DirectionalLightToggle_Hider()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runner2 = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.DirectionalLightToggle("hider");

        bool testValue = runner.transform.GetChild(0).gameObject.GetComponent<Camera>().GetComponent<CameraController>().render;

        yield return new WaitForSeconds(1);

        Assert.IsTrue(testValue);
    }

    [UnityTest]
    public IEnumerator DiretionalLightToggle_Chaser()
    {
        var playerPrefab = Resources.Load("Player 2");

        var runner = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runner2 = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;

        var runnerController = runner.GetComponent<KwPlayer>();

        runnerController.DirectionalLightToggle("chaser");

        bool testValue = runner.transform.GetChild(0).gameObject.GetComponent<Camera>().GetComponent<CameraController>().render;

        yield return new WaitForSeconds(1);

        Assert.IsFalse(testValue);
       }

    [UnityTest]
    public IEnumerator UI_playButton_Test()
    {
        SceneManager.LoadScene("Menu");
        MainMenu menu = new GameObject().AddComponent<MainMenu>();
        menu.PlayGame();
        
        yield return new WaitForSeconds(1);

        Assert.IsTrue(SceneManager.GetActiveScene().buildIndex == 1);

        yield return SceneManager.UnloadSceneAsync(1);

    }

    [UnityTest]
    public IEnumerator UI_optionButton_Test()
    {
        SceneManager.LoadScene("Menu");

        yield return new WaitForSeconds(1);
        Button optionButton = GameObject.Find("OptionButton").GetComponent<Button>();

        optionButton.onClick.Invoke();

        Assert.IsNull(GameObject.Find("MainMenu"));

        yield return SceneManager.UnloadScene("Menu");
    } 

    [UnityTest]
    public IEnumerator UI_backButton_Test()
    {
        SceneManager.LoadScene("Menu");


        yield return new WaitForSeconds(1);

        Button optionButton = GameObject.Find("OptionButton").GetComponent<Button>();

        optionButton.onClick.Invoke();

        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();

        backButton.onClick.Invoke();

        Assert.IsNull(GameObject.Find("OptionMenu"));

    }

    [UnityTest]
    public IEnumerator UI_Robustness_Test()
    {
        SceneManager.LoadScene("Menu");


        yield return new WaitForSeconds(1);

        for (int i = 0; i < 100; i++)
        {
            Button optionButton = GameObject.Find("OptionButton").GetComponent<Button>();

            optionButton.onClick.Invoke();

            Button backButton = GameObject.Find("BackButton").GetComponent<Button>();

            backButton.onClick.Invoke();
        }
        Assert.IsNull(GameObject.Find("OptionMenu"));
    }


    [TearDown]
    public void AfterEveryTest()
    {

        foreach (var runner in GameObject.FindGameObjectsWithTag("Player"))
            GameObject.Destroy(runner);
        foreach (var joytick in GameObject.FindGameObjectsWithTag("joystick"))
            GameObject.Destroy(joytick);
    }
    
    
}


