using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpearTeleEmptyHand : EmptyHand
{

    [SerializeField] GameObject particle;

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

    public override void rButtonFunction()
    {
        if (GameObject.FindGameObjectsWithTag("PlayerWeapon").Length > 0)
        {
            Instantiate(particle, GameObject.FindGameObjectWithTag("PlayerWeapon").transform.position, GameObject.FindGameObjectWithTag("PlayerWeapon").transform.rotation);
            Destroy(GameObject.FindGameObjectWithTag("PlayerWeapon"));
            gameManager.instance.playerScript.weaponHave = true;
            gameManager.instance.playerScript.changeWeapons();
        }
    }
}
