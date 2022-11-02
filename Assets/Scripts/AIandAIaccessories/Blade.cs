using UnityEngine;

public class Blade : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Rigidbody rb;

    [Header("-----Blade Stats-----")]
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.Damage(damage);
        }
    }
}
