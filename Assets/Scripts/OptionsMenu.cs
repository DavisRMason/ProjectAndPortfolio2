using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class OptionsMenu : MonoBehaviour
{
    public AudioMixer Mixer;
    public Camera cam;
    [SerializeField] string mixername;

    public void SetVolume(float vol)
    {
        Mixer.SetFloat(mixername, vol);
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
