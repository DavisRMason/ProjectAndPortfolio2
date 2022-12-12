using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] GameObject eButton;
    [SerializeField] GameObject Menu;

    private void Update()
    {
        if (eButton && Input.GetKey(KeyCode.E))
        {
            eButton.SetActive(false);
            gameManager.instance.isPaused = true;
            gameManager.instance.pause();
            Menu.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            eButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eButton.SetActive(false);
        }
    }
}
