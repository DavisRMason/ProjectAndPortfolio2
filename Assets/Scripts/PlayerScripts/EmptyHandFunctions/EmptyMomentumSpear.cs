using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMomentumSpear : EmptyHand
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
    }

    public override void RightClick()
    {
        if (GameObject.FindGameObjectWithTag("PlayerWeapon") != null)
        {
            gameManager.instance.playerScript.spearMove = true;

            gameManager.instance.playerScript.pushForward = GameObject.FindGameObjectWithTag("PlayerWeapon").transform.position;
        }
    }
}
