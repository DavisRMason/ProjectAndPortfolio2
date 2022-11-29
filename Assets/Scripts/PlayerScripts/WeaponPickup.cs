using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.changeWeapons();
            Destroy(gameObject);
            gameManager.instance.playerScript.weaponHave = true;
        }
    }
}
