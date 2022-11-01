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
    [Range(1, 5)][SerializeField] float playerSpeed;
    [Range(1.5f, 5)][SerializeField] float sprintMod;
    [Range(0, 20)][SerializeField] float jumpHeight;
    [Range(0, 10)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;
    #endregion

    #region Bools_&_Statics

    Vector3 move;
    private Vector3 playerVelocity;
    int jumpTimes;
    int hpOrig;
    float playerOrigSpeed;
    bool isSprinting;
    bool isShooting;
    bool sprintEmtpy;

    #endregion

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        playerOrigSpeed = playerSpeed;
        hpOrig = healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpTimes = 0;
            playerVelocity.y = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpTimes < jumpMax)
        {
            ++jumpTimes;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

}
