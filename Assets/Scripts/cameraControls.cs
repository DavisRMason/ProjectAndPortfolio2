using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int senseHorizontal;
    [SerializeField] int senseVertical;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;
    [SerializeField] bool invertY;

    float xRotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * senseHorizontal;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * senseVertical;

        if (invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        // Clamp Camera Rotation
        xRotation = Mathf.Clamp(lockVertMin, xRotation, lockVertMax);

        //Rotate camera on the x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate the player
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
