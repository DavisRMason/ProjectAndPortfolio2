using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer Mixer;
    public Camera cam;

    public void SetVolume(float vol)
    {
        Mixer.SetFloat("MainVolume", vol);
    }

    public void SetFoV (float fov)
    {
        cam.fieldOfView = fov;
    }
}
