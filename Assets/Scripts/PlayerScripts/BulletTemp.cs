using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTemp : MonoBehaviour
{
    #region

    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

    [Header("----- Spear Stats -----")]
    [SerializeField] int damage;
    [SerializeField] float timer;
    [SerializeField] int speed;
    [SerializeField] int attackDist;
    [SerializeField] int attackAngle;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(gameObject.transform.forward * speed);
        Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
        Destroy(gameObject, timer);
    }

    private void Update()
    {
        AttackEnemy();
    }

    void AttackEnemy()
    {
        RaycastHit hit;

        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
