using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.unpause();
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.isPaused = false;
    }

    public void respawn()
    {
        gameManager.instance.playerScript.respawn();
        gameManager.instance.unpause();
        gameManager.instance.playerDeadMenu.SetActive(false);
    }

    public void restart()
    {
        gameManager.instance.unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }
}
