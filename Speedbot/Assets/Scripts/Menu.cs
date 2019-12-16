using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private const string LEVEL_ONE = "LevelOne";
    private const string LEVEL_TWO = "LevelTwo";
    private const string LEVEL_THREE = "LevelThree";
    private const string LEVEL_FOUR = "LevelFour";

    public GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame() {
        LevelChanger.fadeToNextLevel("Tutorial");
    }

    public void StartLevelOne() {
        LevelChanger.fadeToNextLevel(LEVEL_ONE);
    }
    
    public void StartLevelTwo() {
        LevelChanger.fadeToNextLevel(LEVEL_TWO);
    }

    public void StartLevelThree() {
        LevelChanger.fadeToNextLevel(LEVEL_THREE);
    }

    public void StartLevelFour() {
        LevelChanger.fadeToNextLevel(LEVEL_FOUR);
    }

    public void OpenLevelMenu()
    {
        gameObject.SetActive(false);
        FindInActiveObjectByName("LevelMenu").SetActive(true);
    }

    public void OpenControlMenu()
    {
        gameObject.SetActive(false);
        FindInActiveObjectByName("ControlMenu").SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        gameObject.SetActive(false);
        FindInActiveObjectByName("MainMenu").SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
