using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyChargeSpear : EmptyHand
{
    [SerializeField] float rightClickCooldown;

    public override void rButtonFunction()
    {
        gameManager.instance.playerScript.MakeRaySphere(gameManager.instance.playerScript.shootDist * gameManager.instance.playerScript.charge, 5, (int)(gameManager.instance.playerScript.shootDamage * gameManager.instance.playerScript.calculateCharge()));
        gameManager.instance.playerScript.charge = 0;
    }

    public override void RightClick()
    {
        gameManager.instance.playerScript.changeCharge();
    }
}
