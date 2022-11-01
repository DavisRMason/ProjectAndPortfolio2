using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Sensitivity -----")]
    [SerializeField] int sensHorz;
    [SerializeField] int sensVert;

    [Header("----- Lock Controls -----")]
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [Header("----- Invert -----")]
    [SerializeField] bool invertY;

    #endregion

    #region Bools_&_Static

    float xRotate;

    #endregion

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHorz;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;

        if (invertY)
            xRotate += mouseY;
        else
            xRotate -= mouseY;

        xRotate = Mathf.Clamp(xRotate, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(xRotate, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
