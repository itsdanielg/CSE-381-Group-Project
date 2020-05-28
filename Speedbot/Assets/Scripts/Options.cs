using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    private float music;
    private float sound;
    private float mouseSensitivity;

    public Slider musicSlider;
    public Slider soundSlider;
    public Slider sensitivitySlider;

    void Start() {
        if (PlayerPrefs.HasKey("Music")) {
            music = PlayerPrefs.GetFloat("Music");
            musicSlider.value = music;
        }
        if (PlayerPrefs.HasKey("Sound")) {
            sound = PlayerPrefs.GetFloat("Sound");
            soundSlider.value = sound;
        }
        if (PlayerPrefs.HasKey("Sensitivity")) {
            sound = PlayerPrefs.GetFloat("Sensitivity");
            sensitivitySlider.value = mouseSensitivity;
        }
    }

    public void updateMusic(float newFloat) {
        music = newFloat;
        PlayerPrefs.SetFloat("Music", music);
    }

    public void updateSound(float newFloat) {
        sound = newFloat;
        PlayerPrefs.SetFloat("Sound", sound);
    }

    public void updateSensitivity(float newFloat) {
        mouseSensitivity = newFloat;
        PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
    }
    
}
