using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpear : Shoot
{
    public override void shootBullet()
    {
        RaycastHit hit;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Physics.Raycast(gameManager.instance.playerScript.shootPoint.transform.position, objectPos, out hit, 15 * gameManager.instance.playerScript.calculateCharge()))
        {
            gameManager.instance.playerScript.move = transform.forward * ((Input.GetAxis("Vertical") + 1)) * 15 * gameManager.instance.playerScript.calculateCharge();
         }
    }
}
