using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpear : Shoot
{
    public override void shootBullet()
    {
        gameManager.instance.playerScript.spearMove = true;
    }
}
