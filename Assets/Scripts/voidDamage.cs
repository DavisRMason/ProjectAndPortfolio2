using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class voidDamage : MonoBehaviour
{
    public GameObject reSpawnPos;
    [SerializeField] int damage;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.controller.enabled = false;
            gameManager.instance.playerScript.Damage(damage) ;
            gameManager.instance.player.transform.position = reSpawnPos.transform.position;
            gameManager.instance.playerScript.controller.enabled = true;
        }
        else if (other.CompareTag("ThrownWeapon"))
        {
            Destroy(GameObject.FindGameObjectWithTag("ThrownWeapon"));
            gameManager.instance.playerScript.changeWeapons();
            gameManager.instance.playerScript.weaponHave = true;
        }
    }

}
