using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHand : Shoot
{
    [SerializeField] GameObject obj;

    public override IEnumerator shootBullet()
    {
        if(gameManager.instance.playerScript.isShooting == false && Input.GetButton("Shoot"))
        {
            gameManager.instance.playerScript.isShooting = true;
            gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.shootAudioClip, gameManager.instance.playerScript.shootAudioVolume);

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(obj, objectPos, Camera.main.transform.rotation);
        }

        yield return new WaitForSeconds(gameManager.instance.playerScript.shootRate);

        gameManager.instance.playerScript.isShooting = false;
    }
}
