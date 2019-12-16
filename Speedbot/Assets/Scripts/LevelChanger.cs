using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public static Animator animator;

    private static string nextScene = "";

    void Start() {
        animator = GetComponent<Animator>();
    }

    public static void fadeToMenu() {
        nextScene = "MainMenu";
        animator.SetTrigger("fadeOut");
    }

    public static void fadeToNextLevel(string level) {
        nextScene = level;
        animator.SetTrigger("fadeOut");
    }

    public void onFadeComplete() {
        SceneManager.LoadScene(nextScene);
    }

}
