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
    [SerializeField] GameObject attackPos;

    [Header("----- Spear Stats -----")]
    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int speed;
    [SerializeField] int attackAngle;
    [SerializeField] int attackDist;


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

    private void Update()
    {
        AttackEnemy();
    }

    void AttackEnemy()
    {
        RaycastHit hit;

        if(Physics.Raycast(attackPos.transform.position, transform.TransformDirection(Vector3.forward) , out hit, attackDist))
        {
            if(hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.DrawRay(attackPos.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                hit.collider.GetComponent<IDamage>().takeDamage(damage);
            }
            else
            {
                Debug.DrawRay(attackPos.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
                Instantiate(spear, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }
}