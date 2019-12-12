using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void StartLevelOne() {
        SceneManager.LoadScene(LEVEL_ONE);
    }
    
    public void StartLevelTwo() {
        SceneManager.LoadScene(LEVEL_TWO);
    }

    public void StartLevelThree() {
        SceneManager.LoadScene(LEVEL_THREE);
    }

    public void StartLevelFour() {
        SceneManager.LoadScene(LEVEL_FOUR);
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
