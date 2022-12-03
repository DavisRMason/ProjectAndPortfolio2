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

    void Start()
    {
        volumeslider.value = PlayerPrefs.GetFloat("MainVolume");
    }
    void Update()
    {
        Mixer.SetFloat("MainVolume", volumeVal);
        PlayerPrefs.SetFloat("MainVolume", volumeVal);
    }

    public void SetVolume(float vol)
    {
        volumeVal = vol;
    }

    public void SetFoV (float fov)
    {
        cam.fieldOfView = fov;
    }
    public float SetLookSens(float sens)
    {
        return sens;
    }
}
