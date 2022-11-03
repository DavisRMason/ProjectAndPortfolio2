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

    //DMason: adding player health functionallity
    public HealthBar hpBar;
    [Header("----- Spear Stats -----")]
    [SerializeField] GameObject spear;
    [SerializeField] Transform shootPos;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] bool classWork;

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

        //DMason: setting slider max to player max hp
        hpBar.SetMaxHealth(hpOrig);
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        if (!classWork)
        StartCoroutine(shootSpear());
        else
        StartCoroutine(shootRay());
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

    IEnumerator shootRay()
    {
        if(isShooting = false && Input.GetButton("Shoot"))
        {
            isShooting=true;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }

    }

    IEnumerator shootSpear()
    {
        if (isShooting == false && Input.GetButton("Shoot"))
        {
            isShooting = true;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

            Instantiate(spear, objectPos, Camera.main.transform.rotation);

            yield return new WaitForSeconds(shootRate);
        isShooting = false;
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

    public void Damage(int dmg)
    {
        healthPoints -= dmg;

        //DMason: reducing slider when damaged
        hpBar.SetHealth(healthPoints);
        
        if (healthPoints <= 0)
        {
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.pause();
        }
    }

    public void respawn()
    {
        controller.enabled = false;
        healthPoints = hpOrig;
        transform.position = gameManager.instance.spawnPos.transform.position;
        gameManager.instance.playerDeadMenu.SetActive(false);
        controller.enabled = true;
    }

    public void ResetWeapons()
    {
        weaponHave = true;
    }
}
