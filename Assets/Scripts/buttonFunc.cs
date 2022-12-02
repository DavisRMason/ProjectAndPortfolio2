using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunc : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.unPause();
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.isPaused = false;
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        resume();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void respawn()
    {
        gameManager.instance.unPause();
        gameManager.instance.playerScript.respawn();

    }

    public void options()
    {
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.optionsMenu.SetActive(true);
    }

    public void back()
    {
        gameManager.instance.optionsMenu.SetActive(false);
        gameManager.instance.pauseMenu.SetActive(true);
    }

}
