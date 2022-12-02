using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyChargeSpear : EmptyHand
{
    [SerializeField] float rightClickCooldown;

    public override void rButtonFunction()
    {
        gameManager.instance.playerScript.charge = 0;
    }

    public override void RightClick()
    {
        StartCoroutine(gameManager.instance.playerScript.CoolDown(rightClickCooldown));

        gameManager.instance.playerScript.changeCharge();
    }
}
