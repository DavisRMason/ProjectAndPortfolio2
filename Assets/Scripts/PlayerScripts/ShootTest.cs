using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : Shoot
{
    [SerializeField] GameObject obj;

    public override IEnumerator shootBullet()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        Instantiate(obj, objectPos, Camera.main.transform.rotation);
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.shootAudioClip, gameManager.instance.playerScript.shootAudioVolume);
        gameManager.instance.playerScript.RemoveWeapon();
        yield return new WaitForSeconds(0);
    }

}
