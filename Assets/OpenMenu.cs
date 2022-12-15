using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] GameObject eButton;
    [SerializeField] GameObject Menu;
    bool eButtonActive = false;


    private void Update()
    {
        if (eButtonActive == true && Input.GetKey(KeyCode.E))
        {
            eButton.SetActive(false);
            gameManager.instance.isPaused = true;
            gameManager.instance.pause();
            Menu.SetActive(true);
        }

        if(Input.GetButtonDown("Cancel"))
        {
            Menu.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            eButton.SetActive(true);
            eButtonActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eButton.SetActive(false);
            eButtonActive = false;
        }
    }
}
