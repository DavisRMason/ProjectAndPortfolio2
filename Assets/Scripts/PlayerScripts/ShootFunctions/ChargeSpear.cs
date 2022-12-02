using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpear : Shoot
{
    public override void shootBullet()
    {
        gameManager.instance.playerScript.spearMove = true;

        RaycastHit hit;

        if (Physics.SphereCast(gameManager.instance.playerScript.shootPoint.transform.position, 5, gameManager.instance.playerScript.shootPoint.transform.forward, out hit, 10))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
            }
        }
    }
}
