using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LonginousSpear : Shoot
{
    public override void shootBullet()
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        Debug.Log("Shooty McBangBang");

        if (Physics.Raycast(gameManager.instance.playerScript.shootPoint.transform.position, objectPos, out hit, gameManager.instance.playerScript.shootDist))
        {
            if(hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
            }
        }
    }
}
