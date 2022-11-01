using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    #region Variables

    [Header("----- Components-----")]
    [SerializeField] private CharacterController controller;

    #region Player Stats
    [Header("------ Player Stats -----")]
    [Range(1.0f, 30.0f)] [SerializeField] private float playerSpeed;
    [Range(1.5f, 50.0f)][SerializeField] private float playerJump;
    [Range(1.5f, 30.0f)][SerializeField] private float gravityValue;
    [Range(1.5f, 3.5f)][SerializeField] private float sprintMod;
    [Range(1, 4)][SerializeField] private int jumpsMax;
    [Range(0, 30)][SerializeField] int HP;
    int HPOrig;
    private Vector3 playerVelocity;
    private Vector3 move;
    float playerSpeedOrig;
    int jumpTimes;

    bool isSprinting;
    bool isShooting;
    #endregion

    #region Shoot
    [Header("----- Weapon Stats -----")]
    [SerializeField] private float shootRate;
    [SerializeField] private int shootDistance;
    [SerializeField] private int shootDamage;
    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        respawn();
        playerSpeedOrig = playerSpeed;
        controller = GetComponent<CharacterController>();
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Sprint();
        StartCoroutine(Shoot());
    }

    void Movement()
    {
        //Going to be responsible for all basic movement
        #region Player_BasicMove

        //Checks to see if the player is on the ground or not
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpTimes = 0;
        }

        //Responsible for movement
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        //Updates the players movement
        controller.Move(move * Time.deltaTime * playerSpeed);
        #endregion

        //Responsible for jumping
        #region Jump

        //Checks if the Space bar is down and launches the player into the air
        if (Input.GetKeyDown(KeyCode.Space) && jumpTimes < jumpsMax)
        {
            playerVelocity.y = playerJump;
            jumpTimes++;
        }

        #endregion

        //Updating all variables
        #region Update
        //Updates the y value to make the player jump
        playerVelocity.y += gravityValue * Time.deltaTime;
        //Updates the players x and z values to move them around the environment
        controller.Move(playerVelocity * Time.deltaTime);
        #endregion
    }

    void Sprint()
    {
        //If the button is down, make the players speed increase
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        //Else if the button is up, make the players speed decrease to normal speeds
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator Shoot()
    {
        if (isShooting == false && Input.GetButton("Shoot"))
        {
            isShooting = true;

            RaycastHit hit;
            //Check if the Raycast has hit an object
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                //If the Raycast has hit something, check if it has the IDamage Interface
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    //If the object has IDamage, deal damage to object
                    hit.collider.GetComponent<IDamage>().TakeDamage(shootDamage);
                }
            }
            //Cooldown timer
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void damage(int dmg)
    {
        HP -= dmg;

        StartCoroutine(gameManager.instance.playerDamageFlash());

        if (HP <= 0)
        {
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.pause();
        }
    }

    public void respawn()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.spawnPos.transform.position;
        HP = HPOrig;
        controller.enabled = true;
    }
}

