using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class OptionsMenu : MonoBehaviour
{
    public AudioMixer Mixer;
    public Camera cam;

    private float volumeVal;
    public Slider volumeslider;

    private float fovVal;
    public Slider fovSlider;

    void Start()
    {
        volumeslider.value = PlayerPrefs.GetFloat("MainVolume");
        fovSlider.value = PlayerPrefs.GetFloat("FOV");

    }
    void Update()
    {
        Mixer.SetFloat("MainVolume", volumeVal);
        PlayerPrefs.SetFloat("MainVolume", volumeVal);

        cam.fieldOfView = fovVal;
        PlayerPrefs.SetFloat("FOV", fovVal);
    }

    public void SetVolume(float vol)
    {
        volumeVal = vol;
    }

    public void SetFoV (float fov)
    {
        fovVal = fov;
    }
    public float SetLookSens(float sens)
    {
        return sens;
    }
}
