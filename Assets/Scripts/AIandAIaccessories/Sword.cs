using UnityEngine;

public class Sword : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Rigidbody rb;

    [Header("-----Sword Stats-----")]
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.Damage(damage);
        }
    }
}
