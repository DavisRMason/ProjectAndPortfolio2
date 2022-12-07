using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumSPear : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject attackPos;
    [SerializeField] ParticleSystem effect;

    [Header("----- Spear Stats -----")]
    [SerializeField] int attackDist;


    #endregion

    #region Bools_&_Statics

    Vector3 move;

    #endregion

    #endregion

        // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = true;
        rb.AddForce(gameObject.transform.forward * 2500);
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
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
                Instantiate(gameManager.instance.playerScript.hitEffectTwo, attackPos.transform.position, gameObject.transform.rotation);
            }
            else
            {
                Debug.DrawRay(attackPos.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
                rb.AddForce(gameObject.transform.forward * 0);
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
