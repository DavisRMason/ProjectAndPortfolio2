using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GungnirEmptyHand : EmptyHand
{
    [SerializeField] GameObject obj;
    public override void rButtonFunction()
    {
        if (GameObject.FindGameObjectWithTag("PlayerWeapon") != null)
        {
            Instantiate(obj, GameObject.FindGameObjectWithTag("PlayerWeapon").transform.position, Quaternion.LookRotation(gameManager.instance.player.transform.position - GameObject.FindGameObjectWithTag("PlayerWeapon").transform.position));

            GameObject.FindGameObjectWithTag("ThrownWeapon").GetComponent<Collider>().enabled = true;

            Destroy(GameObject.FindGameObjectWithTag("PlayerWeapon"));
        }
        else if (GameObject.FindGameObjectWithTag("ThrownWeapon") != null)
        {
            gameManager.instance.playerScript.weaponHave = true;
            gameManager.instance.playerScript.changeWeapons();
            Destroy(GameObject.FindGameObjectWithTag("ThrownWeapon"));
        }
    }

    public override void RightClick()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerWeapon").Length == 1)
        {
            GameObject obj;
            obj = GameObject.FindGameObjectWithTag("PlayerWeapon");
            gameManager.instance.playerScript.Teleport(obj.transform.position);
            gameManager.instance.playerScript.weaponHave = true;
            Destroy(GameObject.FindGameObjectWithTag("PlayerWeapon"));
            gameManager.instance.playerScript.ResetGravity();
            gameManager.instance.playerScript.changeWeapons();
        }
    }
}
