using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrow : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject spear;

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
        rb.useGravity = true;
        rb.AddForce(gameObject.transform.forward * speed);
        Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
        rb.ResetCenterOfMass();
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            other.GetComponent<IDamage>().takeDamage(damage);
            gameManager.instance.playerScript.ResetWeapon();
        }
        else
        {
            Instantiate(spear, gameObject.transform.position, Camera.main.transform.rotation);
            Destroy(gameObject);
        }
    }
}