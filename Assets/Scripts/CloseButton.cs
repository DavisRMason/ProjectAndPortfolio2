using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public GameObject credits;
   public void close()
    {
        if (credits.activeInHierarchy == true)
            credits.SetActive(false);
        else
            credits.SetActive(true);
    }
}
