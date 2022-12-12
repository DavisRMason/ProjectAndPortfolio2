using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GameObject effect;
    [SerializeField] AudioClip aud;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.changeWeapons();
            Destroy(gameObject);
            gameManager.instance.playerScript.weaponHave = true;
        }
    }

    private void OnDestroy()
    {
        gameManager.instance.playerScript.PlayEffect(aud);
    }
}