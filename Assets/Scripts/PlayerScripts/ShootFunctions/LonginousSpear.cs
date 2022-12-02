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

        if (Physics.Raycast(gameManager.instance.playerScript.shootPoint.transform.position, gameManager.instance.playerScript.shootPoint.transform.forward, out hit, gameManager.instance.playerScript.shootDist))
        {
            Debug.Log("Ray shot");
            if(hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.Log("Ray Hit");
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
            }
        }
        else
        {
            Debug.Log("Ray Not Shot");
        }
    }
}
