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
    [SerializeField] float timer;
    [SerializeField] int speed;

    #endregion

    #region Bools_&_Statics

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = null;
        rb.AddForce(gameObject.transform.forward * speed);
        Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
        rb.ResetCenterOfMass();
    }

    void Awake()
    {
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager.instance.enemyScript.takeDamage(damage);
        }
    }

    IEnumerator Destroy()
    {

        yield return new WaitForSeconds(timer);

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }
}
