using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LonginousSpear : Shoot
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audClip;
    public override void shootBullet()
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 1.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        Debug.Log("Shooty McBangBang");

        if (Physics.Raycast(gameManager.instance.playerScript.shootPoint.transform.position, gameManager.instance.playerScript.shootPoint.transform.forward, out hit, 3))
        {;
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
                Instantiate(gameManager.instance.playerScript.hitEffectTwo, hit.transform.position, gameObject.transform.rotation);
                gameManager.instance.playerScript.PlayEffect(audClip);
            }
        }
    }
}
