using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    #region Variables

    #region Unity_Editor
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] public AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(1, 30)][SerializeField] int healthPoints;
    [SerializeField] float playerSpeed;
    [SerializeField] float playerMaxSpeed;
    [SerializeField] float dashMod;
    [SerializeField] float dashTime;
    [SerializeField] int dashCount;
    [SerializeField] int dashCooldown;
    [Range(1.5f, 5)][SerializeField] float sprintMod;
    [Range(1.5f, 5)][SerializeField] float sprintMax;
    [Range(0, 20)][SerializeField] float jumpHeight;
    [Range(0, 40)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;

    //DMason: adding player health functionallity
    public HealthBar hpBar;
    public GameObject healthBar;

    [Header("----- Gun Stats -----")]
    [SerializeField] public float shootRate;
    [SerializeField] public int shootDist;
    [SerializeField] public int shootDamage;
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Shoot shootFunc;
    [SerializeField] List<WeaponStats> weapons = new List<WeaponStats>();
    [SerializeField] int selectedWeapon = 0;

    [Header("----- Audio -----")]
    [SerializeField] List <AudioClip> jumpAudioClips = new List<AudioClip>();
    [Range(0, 1)][SerializeField] float jumpAudioVolume;
    [SerializeField] List<AudioClip> hurtAudioClips = new List<AudioClip>();
    [Range(0, 1)][SerializeField] float hurtAudioVolume;
    [SerializeField] public AudioClip shootAudioClip;
    [Range(0, 1)][SerializeField] public float shootAudioVolume;
    

    #endregion

    #region Bools_&_Statics

    Vector3 move;
    private Vector3 playerVelocity;
    float sprintCurr;
    int jumpTimes;
    int hpOrig;
    int DCOrig;
    float playerOrigSpeed;
    bool isSprinting;
    public bool isShooting;
    bool sprintEmtpy;
    bool weaponHave;
    GameObject weaponModelOrig;
    float shootRateOrig;
    int shootDamageOrig;
    int shootDistOrig;
    int dashCountOrig;

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerOrigSpeed = playerSpeed;
        hpOrig = healthPoints;
        sprintCurr = sprintMax;
        dashCountOrig = dashCount;

        weaponModelOrig = weaponModel;
        shootRateOrig = shootRate;
        shootDamageOrig = shootDamage;
        shootDistOrig = shootDist;

        changeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        if(!gameManager.instance.isPaused)
        weaponSelect();
        if(!isShooting && Input.GetButtonDown("Shoot") && !gameManager.instance.isPaused)
        {
            aud.PlayOneShot(shootAudioClip, shootAudioVolume);
            StartCoroutine(shootFunc.shootBullet());
        }
    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpTimes = 0;
            playerVelocity.y = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal")
            + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpMax)
        {
            aud.PlayOneShot(jumpAudioClips[Random.Range(0, jumpAudioClips.Count)], jumpAudioVolume);
            ++jumpTimes;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            if (controller.isGrounded)
            {
                if (dashCount > 0 && Input.GetKey(KeyCode.S))
                {
                    StartCoroutine(dashForwards());
                    StartCoroutine(dashCoolDown());
                }
                else if (dashCount > 0 && Input.GetKey(KeyCode.D))
                {
                    StartCoroutine(dashSides());
                    StartCoroutine(dashCoolDown());
                }
                else if (dashCount > 0 && Input.GetKey(KeyCode.A))
                {
                    StartCoroutine(dashSides());
                    StartCoroutine(dashCoolDown());
                }
                else
                {
                    playerSpeed *= sprintMod;
                    isSprinting = true;
                }
            }
            else
            {
                if (dashCount > 0 && Input.GetKey(KeyCode.W))
                {
                    StartCoroutine(dashForwards());
                    StartCoroutine(dashCoolDown());
                }
                else if (dashCount > 0 && Input.GetKey(KeyCode.S))
                {
                    StartCoroutine(dashForwards());
                    StartCoroutine(dashCoolDown());
                }
                else if (dashCount > 0 && Input.GetKey(KeyCode.D))
                {
                    StartCoroutine(dashSides());
                    StartCoroutine(dashCoolDown());
                }
                else if (dashCount > 0 && Input.GetKey(KeyCode.A))
                {
                    StartCoroutine(dashSides());
                    StartCoroutine(dashCoolDown());
                }
            }

        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator dashForwards()
    {
        playerSpeed *= dashMod;

        move = transform.forward * (Input.GetAxis("Vertical"));
        controller.Move(move * playerSpeed * Time.deltaTime);
        yield return new WaitForSeconds(dashTime);

        dashCount--;
        updateDashMeter();

        playerSpeed = playerOrigSpeed;
    }

    IEnumerator dashSides()
    {
        playerSpeed *= dashMod;

        move = transform.right * (Input.GetAxis("Horizontal"));
        controller.Move(move * playerSpeed * Time.deltaTime);
        yield return new WaitForSeconds(dashTime);

        dashCount--;
        updateDashMeter();

        playerSpeed = playerOrigSpeed;
    }

    IEnumerator dashCoolDown()
    {
        yield return new WaitForSeconds(dashCooldown);

        dashCount++;
        updateDashMeter();
    }

    void updateDashMeter()
    {
        gameManager.instance.DashBar.fillAmount = (float)dashCount / (float)dashCountOrig;
    }

    public void Damage(int dmg)
    {
        healthPoints -= dmg;
        //DMason
        updatePlayerHBar();

        //DMason: reducing slider when damaged
        //hpBar.SetHealth(healthPoints);

        StartCoroutine(gameManager.instance.playerDamageFlash());
        aud.PlayOneShot(hurtAudioClips[(Random.Range(0, hurtAudioClips.Count))], hurtAudioVolume);

        if (healthPoints <= 0)
        {
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.pause();
        }
    }

    // DMason update Player HP from class
     void updatePlayerHBar()
    {
        gameManager.instance.HPBar.fillAmount = (float)healthPoints / (float)hpOrig;
        if (healthPoints < hpOrig * 0.6 && healthPoints > hpOrig *0.3 )
        {
            gameManager.instance.HPBar.color = Color.yellow;
        }
        else if (healthPoints <= hpOrig * .3)
        {
            gameManager.instance.HPBar.color = Color.red;
        }
        else
        {
            gameManager.instance.HPBar.color = Color.green;
        }
    }
    

    public void respawn()
    {
        controller.enabled = false;
        healthPoints = hpOrig;
        //DMason
        updatePlayerHBar();
        transform.position = gameManager.instance.spawnPos.transform.position;
        gameManager.instance.playerDeadMenu.SetActive(false);
        controller.enabled = true;
    }

    public void weaponPickup(WeaponStats weaponStat)
    {
        weapons.Add(weaponStat);
        selectedWeapon++;
        changeWeapons();
    }

    void changeWeapons()
    {
        shootRate = weapons[selectedWeapon].shootRate;
        shootDamage = weapons[selectedWeapon].shootDamage;
        shootDist = weapons[selectedWeapon].shootDist;
        shootFunc = weapons[selectedWeapon].shootScript;
        weaponModel.GetComponent<MeshFilter>().sharedMesh = weapons[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weapons[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        shootAudioClip = weapons[selectedWeapon].weaponSound;

        weaponModel.transform.localScale = weapons[selectedWeapon].weaponModel.transform.localScale;
    }

    void weaponSelect()
    {
        if (weapons.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weapons.Count - 1)
            {
                Debug.Log("Going Up");
                selectedWeapon++;
                changeWeapons();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
            {
                Debug.Log("Going Down");
                selectedWeapon--;
                changeWeapons();
            }
        }
    }

    public void RemoveWeapon()
    {
        if(selectedWeapon == 0)
        {
            weapons.Remove(weapons[selectedWeapon]);

            changeWeapons();
        }
        else if (selectedWeapon <= weapons.Count)
        {
            weapons.Remove(weapons[selectedWeapon]);
            selectedWeapon--;

            changeWeapons();
        }
        else
        {
            weapons.Remove(weapons[selectedWeapon]);

            weapons = new List<WeaponStats>();
            weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponModelOrig.GetComponent<MeshFilter>().sharedMesh;
            weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponModelOrig.GetComponent<MeshRenderer>().sharedMaterial;
            shootDamage = shootDamageOrig;
            shootDist = shootDistOrig;
            shootRate = shootRateOrig;
            shootFunc = null;
            selectedWeapon = -1;
        }
    }
}
