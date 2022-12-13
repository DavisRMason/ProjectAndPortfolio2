using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Components -----")]
    [SerializeField] public CharacterController controller;
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
    [SerializeField] public int charge;
    [SerializeField] int chargeMax;
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject hitEffect;
    [SerializeField] public GameObject hitEffectTwo;
    [SerializeField] public Weapon weaponFunc;

    [Header("----- Audio -----")]
    [SerializeField] List<AudioClip> jumpAudioClips = new List<AudioClip>();
    //[Range(0, 1)][SerializeField] float jumpAudioVolume;
    [SerializeField] List<AudioClip> hurtAudioClips = new List<AudioClip>();
    //[Range(0, 1)][SerializeField] float hurtAudioVolume;
    [SerializeField] public List <AudioClip> shootAudioClip = new List<AudioClip>();
    [SerializeField] AudioClip dashAudioClip;
    //[Range(0, 1)][SerializeField] public float shootAudioVolume;

    [Header("For Demonstration")]
    [SerializeField] List<Weapon> weapons = new List<Weapon>();

    #endregion

    #region Bools_&_Statics

    //Player Movement
    public Vector3 move;
    private Vector3 playerVelocity;
    int jumpTimes;
    bool jumpKeyHeld;
    float gravityValueOrig;
    //Please fucking work variable
    bool isDead;
    //I touch
    public float playerOrigSpeed;
    //
    bool isSprinting;
    int dashCountOrig;
    bool wallRunning;
    public Vector3 pushForward;

    //Weapon Stuff
    public bool isShooting;
    public bool weaponHave;
    GameObject weaponModelOrig;
    float shootRateOrig;
    int shootDamageOrig;
    int shootDistOrig;
    public float weaponFlyDist;
    public bool spearMove;
    public bool chargeCool;
    bool rButtonUp = true;
    public int currentWeapon;

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
        isDead = false;

        gameManager.instance.weaponHolderScript.DoThing();
        changeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        WallJump();
        WallRun();

        //Dash
        if (dashCount > 0 && !isDashing && Input.GetButtonDown("Dash"))
        {
            aud.PlayOneShot(dashAudioClip, Random.Range(0.5f, 1.0f));
            StartCoroutine(Dash());
            StartCoroutine(DashCoolDown());
        }
        else if (Input.GetButtonUp("Dash"))
        {
            isDashing = false;
        }

        //Shoot
        if (!isDead && Input.GetButtonDown("Shoot") && weaponHave && !isShooting && !gameManager.instance.isPaused)
        {
            //aud.PlayOneShot(shootAudioClip, shootAudioVolume);
            weaponFunc.weaponStats.shootScript.shootBullet();
            Debug.Log("Shoot button pressed");
            if (weaponFunc.Melee)
            {
                hitEffect.GetComponent<ParticleSystem>().Play();
                StartCoroutine(CoolDown(shootRate, isShooting));
            }
        }

        if (!weaponHave || weaponFunc.Melee)
        {
            if (Input.GetKey(KeyCode.R) && rButtonUp)
            {
                weaponFunc.emptyHandScript.emptyHandScript.rButtonFunction();
                rButtonUp = false;
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                rButtonUp = true;
            }

            if (Input.GetButtonDown("Right Mouse"))
            {
                weaponFunc.emptyHandScript.emptyHandScript.RightClick();
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (isJumping)
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

        if (!wallRunning && !spearMove)
        {
            move = transform.right * Input.GetAxis("Horizontal")
                + transform.forward * Input.GetAxis("Vertical");
        }

        if (spearMove && weaponFunc.Melee)
        {
            Vector3 cameraPos = Input.mousePosition;
            cameraPos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(cameraPos);
            MakeRaySphere(5, 2, shootDamage);
            pushForward = Camera.main.transform.forward * (Input.GetAxis("Vertical") + 5);
            controller.Move(pushForward * Time.deltaTime * playerSpeed);
            StartCoroutine(ForceMove());
        }
        else if (spearMove && !weaponFunc.Melee)
        {
            move = pushForward - gameObject.transform.position;

            controller.Move(move * Time.deltaTime * playerSpeed);

            if(Input.GetButtonUp("Right Mouse"))
            {
                spearMove = false;
                controller.enabled = true;
            }
        }
        else
        {
                controller.Move(move * Time.deltaTime * playerSpeed);
        }

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpMax)
        {
            jumpKeyHeld = true;
            isJumping = true;
            ResetGravity();
            aud.PlayOneShot(jumpAudioClips[Random.Range(0, jumpAudioClips.Count)]/*, jumpAudioVolume*/);
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

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, 1))
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
        var left = transform.TransformDirection(Vector3.left);
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
                wallRunning = true;
                playerVelocity.y = 0;

                move = transform.forward * (Input.GetAxis("Vertical") + 1);

                controller.Move(move * Time.deltaTime * playerSpeed);
            }
        }
        else
        {
            ResetGravity();
            onWall = false;
            wallRunning= false;
        }
    }

    public void ResetGravity()
    {
        onWall = false;
        gravityValue = gravityValueOrig;
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
        aud.PlayOneShot(hurtAudioClips[(Random.Range(0, hurtAudioClips.Count))]/*, hurtAudioVolume*/);

        if (healthPoints <= 0)
        {
            isDead = true;
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.pause();
        }
    }

    // DMason update Player HP from class
     void updatePlayerHBar()
    {
        gameManager.instance.HPBar.fillAmount = (float)healthPoints / (float)hpOrig;
       // gameManager.instance.DamageBar.fillAmount = Mathf.Lerp(gameManager.instance.HPBar.fillAmount, (float)healthPoints, Time.deltaTime * 3);
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
        gameManager.instance.resetTimer();
        //DMason
        updatePlayerHBar();
        transform.position = gameManager.instance.spawnPos.transform.position;
        gameManager.instance.playerDeadMenu.SetActive(false);

        Destroy(GameObject.FindGameObjectWithTag("PlayerWeapon"));

        controller.enabled = true;
        gameManager.instance.playerScript.weaponHave = true;
        gameManager.instance.playerScript.changeWeapons();
        isDead = false;
    }

    public void changeWeapons()
    {
        shootRate = weaponFunc.weaponStats.shootRate;
        shootDamage = weaponFunc.weaponStats.shootDamage;
        shootDist = weaponFunc.weaponStats.shootDist;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponFunc.weaponStats.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponFunc.weaponStats.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        shootAudioClip = weaponFunc.weaponStats.weaponSound;
        hitEffect = weaponFunc.weaponStats.hitEffect;
        hitEffectTwo = weaponFunc.weaponStats.muzzleFlash;

        currentWeapon = weaponFunc.weaponStats.currentWeapon;

        weaponModel.transform.localScale = weaponFunc.weaponStats.weaponModel.transform.localScale;
    }

    public void changeHand()
    {
        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponFunc.emptyHandScript.emptyHandModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponFunc.emptyHandScript.emptyHandModel.GetComponent<MeshRenderer>().sharedMaterial;

        weaponModel.transform.localScale = weaponFunc.emptyHandScript.emptyHandModel.transform.localScale;
    }


    public IEnumerator CoolDown(float timer, bool changeBool)
    {
        changeBool = true;

        yield return new WaitForSeconds(timer);

        changeBool = false;
    }

    IEnumerator ForceMove()
    {

        yield return new WaitForSeconds(calculateCharge());

        spearMove = false;
    }

    public void changeCharge()
    {
        if (!chargeCool && charge < chargeMax)
            charge++;
        StartCoroutine(CoolDown(.5f, chargeCool));

        Debug.Log("Charge is" + charge);
    }

    public float calculateCharge()
    {
        float chargeMod = 0;

        switch(charge)
        {
            case 0:
                chargeMod = .5f;
                break;
            case 1:
                chargeMod = .75f;
                break;
            case 2:
                chargeMod = 1f;
                break;
            case 3:
                chargeMod = 1.25f;
                break;
            case 4:
                chargeMod = 1.5f;
                break;
        }

        return chargeMod;
    }

    public void MakeRaySphere(float radius, float distance, int damage)
    {

        RaycastHit hit;

        if (Physics.SphereCast(shootPoint.transform.position, radius, shootPoint.transform.forward, out hit, distance))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.DrawRay(hit.transform.forward, transform.forward, color: Color.red);
                hit.collider.GetComponent<IDamage>().takeDamage(damage);
                hitEffect.GetComponent<ParticleSystem>().Play();
                Instantiate(hitEffectTwo, hit.transform.position, hit.transform.rotation);
                aud.PlayOneShot(weaponFunc.weaponStats.attackSound, volumeScale: 1);
            }
        }
    }

    public void PlayEffect(AudioClip effect)
    {
        aud.PlayOneShot(effect, Random.Range(0.5f, 1.0f));
    }

    public void WeaponChanger(int choice)
    {
        if (GameObject.FindGameObjectWithTag("PlayerWeapon") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Playerweapon"));
        }
        else if (GameObject.FindGameObjectWithTag("ThrownWeapon") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("ThrownWeapon"));
        }

        weaponHave = true;

        switch (choice)
        {
            case 0:
                weaponFunc = weapons[0];
                changeWeapons();
                break;
            case 1:
                weaponFunc = weapons[1];
                changeWeapons();
                break ;
            case 2:
                weaponFunc = weapons[2];
                changeWeapons();
                break;
            case 3:
                weaponFunc = weapons[3];
                changeWeapons();
                break;
            case 4:
                weaponFunc = weapons[4];
                changeWeapons();
                break;
        }
    }


}
