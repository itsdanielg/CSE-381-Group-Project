using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

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

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
