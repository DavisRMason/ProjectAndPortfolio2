using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunc : MonoBehaviour
{

    [SerializeField] string sceneName;

    public GameObject LoadingScreen;
    public Slider Slider;
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
        StartCoroutine(LoadAsync(sceneName));
        resume();
    }

    public void back()
    {
        //introduces minor bug of returning to game from level select
        gameManager.instance.lvlSelectMenu.SetActive(false);
        gameManager.instance.optionsMenu.SetActive(false);
        gameManager.instance.pauseMenu.SetActive(true);
    }

    IEnumerator LoadAsync(string s)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(s);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            Slider.value = progress;

            yield return null;
        }
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

    public void switchTele()
    {
        gameManager.instance.playerScript.WeaponChanger(0);
        gameManager.instance.weaponHolderScript.ChangeWeapon(gameManager.instance.playerScript.weaponFunc);
    }

    public void switchMomen()
    {
        gameManager.instance.playerScript.WeaponChanger(1);
        gameManager.instance.weaponHolderScript.ChangeWeapon(gameManager.instance.playerScript.weaponFunc);
    }

    public void switchCharge()
    {
        gameManager.instance.playerScript.WeaponChanger(2);
        gameManager.instance.weaponHolderScript.ChangeWeapon(gameManager.instance.playerScript.weaponFunc);
    }

    public void switchLong()
    {
        gameManager.instance.playerScript.WeaponChanger(3);
        gameManager.instance.weaponHolderScript.ChangeWeapon(gameManager.instance.playerScript.weaponFunc);
    }

    public void switchGung()
    {
        gameManager.instance.playerScript.WeaponChanger(4);
        gameManager.instance.weaponHolderScript.ChangeWeapon(gameManager.instance.playerScript.weaponFunc);
    }
}
