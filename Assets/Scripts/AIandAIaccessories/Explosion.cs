using Unity.Burst.CompilerServices;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Rigidbody rb;

    [Header("-----Explosion Stats-----")]
    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int expandSpeed;

    void Start()
    {
        Destroy(gameObject, timer);
    }

    void Update()
    {
        transform.localScale = transform.localScale + (new Vector3(expandSpeed, expandSpeed, expandSpeed) * Time.deltaTime);
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
