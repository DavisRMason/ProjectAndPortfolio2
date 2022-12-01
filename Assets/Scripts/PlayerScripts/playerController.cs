using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] public GameObject shootPoint;
    [SerializeField] Collider playerCollider;
    [SerializeField] public AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(1, 30)][SerializeField] int healthPoints;
    [SerializeField] public float playerSpeed;
    [SerializeField] float dashMod;
    [SerializeField] float dashTime;
    [SerializeField] int dashCount;
    [SerializeField] int dashWait;
    [SerializeField] int dashCooldown;
    [SerializeField] bool isDashing;
    [Range(1.5f, 5)][SerializeField] float sprintMod;
    [Range(0, 20)][SerializeField] float jumpHeight;
    [Range(0, 40)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;
    [SerializeField] public bool onWall = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] int wallJumpMax;

    [Header("----- Weapon Stats -----")]
    [SerializeField] public float shootRate;
    [SerializeField] public int shootDist;
    [SerializeField] public int shootDamage;
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Weapon weaponFunc;

    [Header("----- Audio -----")]
    [SerializeField] List <AudioClip> jumpAudioClips = new List<AudioClip>();
    [Range(0, 1)][SerializeField] float jumpAudioVolume;
    [SerializeField] List<AudioClip> hurtAudioClips = new List<AudioClip>();
    [Range(0, 1)][SerializeField] float hurtAudioVolume;
    [SerializeField] public AudioClip shootAudioClip;
    [Range(0, 1)][SerializeField] public float shootAudioVolume;
    
    #endregion

    #region Bools_&_Statics

    //Player Movement
    Vector3 move;
    private Vector3 playerVelocity;
    int jumpTimes;
    bool jumpKeyHeld;
    float gravityValueOrig;
    //I touch
    public float playerOrigSpeed;
    //
    bool isSprinting;
    int dashCountOrig;
    bool wallRunning;

    //Weapon Stuff
    public bool isShooting;
    public bool weaponHave;
    GameObject weaponModelOrig;
    float shootRateOrig;
    int shootDamageOrig;
    int shootDistOrig;
    public float weaponFlyDist;


    //DMason: adding player health functionallity
    //Health Stuff
    public HealthBar hpBar;
    public GameObject healthBar;
    int hpOrig;

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerOrigSpeed = playerSpeed;
        hpOrig = healthPoints;
        dashCountOrig = dashCount;
        gravityValueOrig = gravityValue;

        weaponModelOrig = weaponModel;
        shootRateOrig = shootRate;
        shootDamageOrig = shootDamage;
        shootDistOrig = shootDist;

        weaponHave = true;

        changeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        WallJump();
        WallRun();
        //Dash
        if (dashCount > 0 && !isDashing && Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
            StartCoroutine(DashCoolDown());
        }
        else if (Input.GetButtonUp("Dash"))
        {
            isDashing = false;
        }

        //Shoot
        if (weaponHave && !isShooting && Input.GetButtonDown("Shoot") && !gameManager.instance.isPaused)
        {
            //aud.PlayOneShot(shootAudioClip, shootAudioVolume);
            weaponFunc.weaponStats.shootScript.shootBullet();

            if(weaponFunc.Melee)
            {
                StartCoroutine(CoolDown());
            }
        }

        if (!weaponHave || weaponFunc.Melee)
        {
            if(Input.GetKey(KeyCode.R))
            {
                weaponFunc.emptyHandScript.emptyHandScript.rButtonFunction();
            }

            if(Input.GetButtonDown("Right Mouse"))
            {
                weaponFunc.emptyHandScript.emptyHandScript.RightClick();
            }
        }
    }

    private void FixedUpdate()
    {
        if(isJumping)
        {
            if (!jumpKeyHeld)
            {
                playerVelocity.y -= gravityValue * Time.deltaTime;
            }
        }
    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpTimes = 0;
            playerVelocity.y = 0;
        }

        if (!wallRunning)
        {
            move = transform.right * Input.GetAxis("Horizontal")
                + transform.forward * Input.GetAxis("Vertical");
        }
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpMax)
        {
            jumpKeyHeld = true;
            isJumping = true;
            ResetGravity();
            aud.PlayOneShot(jumpAudioClips[Random.Range(0, jumpAudioClips.Count)], jumpAudioVolume);
            ++jumpTimes;

            playerVelocity.y = jumpHeight;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpKeyHeld = false;           
        }
        
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void WallJump()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, 1))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                if (!onWall)
                {
                    gravityValue = 1;
                    onWall = true;
                }
                
                playerVelocity.y = 0;
                wallRunning = true;
            }
        }
        else
        {
            ResetGravity();
            onWall = false;
            wallRunning = false;
        }
    }

    void WallRun()
    {
        RaycastHit hit;
        var left = transform.TransformDirection (Vector3.left);
        var right = transform.TransformDirection(Vector3.right);


        if (Physics.Raycast(gameObject.transform.position, left, out hit, 1) || Physics.Raycast(gameObject.transform.position, right, out hit, 1))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                if (!onWall)
                {
                    gravityValue = 1;
                    onWall = true;
                }

                playerVelocity.y = 0;

                move = transform.forward * (Input.GetAxis("Vertical") + 1);

                controller.Move(move * Time.deltaTime * playerSpeed);
            }
        }
        else
        {
            ResetGravity();
            onWall = false;
        }
    }

    public void ResetGravity()
    {
        onWall = false;
        gravityValue = gravityValueOrig;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {   
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        playerSpeed *= dashMod;
        gameObject.GetComponent<Collider>().enabled = false;


        move = transform.forward * (Input.GetAxis("Vertical")) + transform.right * (Input.GetAxis("Horizontal"));
        controller.Move(move * playerSpeed * Time.deltaTime);

        yield return new WaitForSeconds(dashTime);

        dashCount--;
        updateDashMeter();

        gameObject.GetComponent<Collider>().enabled = true;
        playerSpeed = playerOrigSpeed;
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCooldown);

        dashCount++;
        updateDashMeter();
    }

    void updateDashMeter()
    {
        gameManager.instance.DashBar.fillAmount = (float)dashCount / (float)dashCountOrig;
    }

    public void Teleport(Vector3 newPos)
    {
        controller.enabled = false;

        gameObject.transform.position = newPos;

        controller.enabled = true;
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

    public void changeWeapons()
    {
        shootRate = weaponFunc.weaponStats.shootRate;
        shootDamage = weaponFunc.weaponStats.shootDamage;
        shootDist = weaponFunc.weaponStats.shootDist;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponFunc.weaponStats.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponFunc.weaponStats.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        shootAudioClip = weaponFunc.weaponStats.weaponSound;

        weaponModel.transform.localScale = weaponFunc.weaponStats.weaponModel.transform.localScale;
    }

    public void changeHand()
    {
        shootRate = 0;
        shootDamage = 0;
        shootDist = 0;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponFunc.emptyHandScript.emptyHandModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponFunc.emptyHandScript.emptyHandModel.GetComponent<MeshRenderer>().sharedMaterial;

        weaponModel.transform.localScale = weaponFunc.emptyHandScript.emptyHandModel.transform.localScale;
    }


    public IEnumerator CoolDown()
    {
        isShooting = true;

        yield return new WaitForSeconds(gameManager.instance.playerScript.shootRate);

        isShooting = false;
    }
}
