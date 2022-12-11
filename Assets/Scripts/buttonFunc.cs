using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunc : MonoBehaviour
{

    [SerializeField] string sceneName;
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

    public void LevelSelect()
    {
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.lvlSelectMenu.SetActive(true);
    }

    public void chooseLevel()
    {
        SceneManager.LoadScene(sceneName);
        resume();
    }

    public void back()
    {
        //introduces minor bug of returning to game from level select
        gameManager.instance.lvlSelectMenu.SetActive(false);
        gameManager.instance.optionsMenu.SetActive(false);
        gameManager.instance.pauseMenu.SetActive(true);
    }

    public void returntoMain()
    {
        SceneManager.LoadScene("TitleScreen");
        resume();
    }

    public void returntoHub()
    {
        SceneManager.LoadScene("CumulativeScene");
        resume();
    }

    public void startGame()
    {
        SceneManager.LoadScene("CumulativeScene");
        gameManager.instance.unPause();
    }

}
