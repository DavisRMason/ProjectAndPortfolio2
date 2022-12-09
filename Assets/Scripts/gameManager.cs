using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("-----Player Stuff-----")]
    public GameObject player;
    public playerController playerScript;

    [Header("-----UI-----")]
    [Header("--menus--")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject playerDamageScreen;
    [Header("---meters---")]
    public Image HPBar;
    public Image DashBar;
    public TextMeshProUGUI enemiesLeft;
    [Header("---timer---")]
    public TextMeshProUGUI digTimer;
    [SerializeField] float maxTime = 1000f;
    public Image timerFill;
    [Header("--TBS--")]

    public int enemiesToKill;
    float currTime = 0f;

    public GameObject spawnPos;

    public bool isPaused;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Pos");

        currTime = maxTime;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
            {
                pause();
            }
            else
            {
                unPause();
                optionsMenu.SetActive(isPaused);
            }
        }

        updateTimer();

    }
    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;  //cant leave the screen
    }

    public void unPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;  //locked in the centre of the screen
    }
    public IEnumerator playerDamageFlash()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }
    public void youWin()
    {
        winMenu.SetActive(true);
        pause();
    }
    public void updateEnemyNumber()
    {
        updateUIEnemyCount(-1);

        if (enemiesToKill <= 0)
        {
            youWin();
        }
    }

    public void updateUIEnemyCount(int amount)
    {
        enemiesToKill += amount;
        enemiesLeft.text = enemiesToKill.ToString("F0");  //0 - 0 float (therefore an int), 1 - float...

    }

    public void resetTimer()
    {
        currTime = maxTime;
        timerFill.color = Color.white;
    }

    public void updateTimer()
    {
        float flashSpeed;
        currTime -= Time.deltaTime;
        if (currTime < 10)
        {
            digTimer.text = currTime.ToString("0.00");
        }
        else
        {
            digTimer.text = currTime.ToString("0");
        }

        timerFill.fillAmount = (float)currTime / (float)maxTime;
        if (currTime <= (maxTime * .25))
        {
            flashSpeed = (float)(maxTime / currTime);

            timerFill.color = Color.Lerp(Color.white, Color.red, Mathf.Cos(Time.time * flashSpeed));
        }

        if (currTime <= 0)
        {
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.pause();
        }
    }
}
