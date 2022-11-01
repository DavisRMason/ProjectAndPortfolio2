using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

    [Header("----- Spear Stats -----")]
    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int speed;

    #endregion

    #region Bools_&_Statics

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * speed);
        rb.rotation = transform.rotation;
    }
}
