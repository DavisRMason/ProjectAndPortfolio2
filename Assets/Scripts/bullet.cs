using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

    [Header("----- Bullet Stats -----")]
    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int speed;


    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.damage(damage);
        }

        Destroy(gameObject);
    }
}
