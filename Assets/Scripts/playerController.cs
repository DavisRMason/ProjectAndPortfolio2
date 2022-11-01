using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    #region Variables
    
    #region Unity_Editor
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 30)][SerializeField] int healthPoints;
    [SerializeField] float playerSpeed;
    [SerializeField] float playerMaxSpeed;
    [SerializeField] float dashMod;
    [SerializeField] float dashTime;
    [Range(1.5f, 5)][SerializeField] float sprintMod;
    [Range(1.5f, 5)][SerializeField] float sprintMax;
    [Range(0, 20)][SerializeField] float jumpHeight;
    [Range(0, 40)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;

    [Header("----- Spear Stats -----")]
    [SerializeField] GameObject spear;
    [SerializeField] Transform shootPos;

    #endregion

    #region Bools_&_Statics

    Vector3 move;
    private Vector3 playerVelocity;
    float sprintCurr;
    int jumpTimes;
    int hpOrig;
    float playerOrigSpeed;
    
    bool isSprinting;
    bool isShooting;
    bool sprintEmtpy;
    bool weaponHave;

    #endregion

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        playerOrigSpeed = playerSpeed;
        hpOrig = healthPoints;
        sprintCurr = sprintMax;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
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
                if (Input.GetKey(KeyCode.S))
                {
                    StartCoroutine(dashForwards());
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    StartCoroutine(dashSides());
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    StartCoroutine(dashSides());
                }
                else
                {
                    playerSpeed *= sprintMod;
                    isSprinting = true;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    StartCoroutine(dashForwards());
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    StartCoroutine(dashForwards());
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    StartCoroutine(dashSides());
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    StartCoroutine(dashSides());
                }
            }
           
        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }

    void shoot()
    {
        if (Input.GetButton("Shoot") && weaponHave)
        {
            Instantiate(spear, shootPos.position, transform.rotation);
            weaponHave = false;
        }
    }

    IEnumerator dashForwards()
    {
        playerSpeed *= dashMod;

        move = transform.forward * (Input.GetAxis("Vertical"));
        controller.Move(move * playerSpeed * Time.deltaTime);
        yield return new WaitForSeconds(dashTime);

        playerSpeed = playerOrigSpeed;
    }

    IEnumerator dashSides()
    {
        playerSpeed *= dashMod;

        move = transform.right * (Input.GetAxis("Horizontal"));
        controller.Move(move * playerSpeed * Time.deltaTime);
        yield return new WaitForSeconds(dashTime);

        playerSpeed = playerOrigSpeed;
    }
}
