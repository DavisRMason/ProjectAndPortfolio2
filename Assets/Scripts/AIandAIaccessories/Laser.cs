using Unity.Burst.CompilerServices;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("-----Explosion Stats-----")]
    [SerializeField] int damage;
    [SerializeField] int timer;

    void Start()
    {
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
