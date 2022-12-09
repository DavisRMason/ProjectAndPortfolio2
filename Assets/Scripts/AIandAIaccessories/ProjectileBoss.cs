using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Rigidbody rb;

    [Header("-----Bullet Stats-----")]
    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int speed;
    [SerializeField] float rotateSpeed;

    Vector3 playerPos;

    void Start()
    {
        playerPos = gameManager.instance.player.transform.position;
        Destroy(gameObject, timer);
    }

    private void Update()
    {
        Vector3 vectorToTarget = playerPos - transform.position;
        
        Vector3 newRot = Vector3.RotateTowards(transform.forward,vectorToTarget,rotateSpeed * Time.deltaTime, 0);

        transform.rotation = Quaternion.LookRotation(newRot);

        rb.velocity = transform.forward * speed;

        if (rb.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
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
