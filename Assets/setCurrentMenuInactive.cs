using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setCurrentMenuInactive : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public void setFalse()
    {
        menu.SetActive(false);
        gameManager.instance.isPaused = false;
        gameManager.instance.unPause();
    }

}
