using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Rigidbody rb;

    [Header("-----Bullet Stats-----")]
    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int speed;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.Damage(damage);
        }
        Destroy(gameObject);
    }
}
