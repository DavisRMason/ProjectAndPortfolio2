using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGround : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.ResetWeapon();
            Destroy(gameObject);
        }
    }
}
