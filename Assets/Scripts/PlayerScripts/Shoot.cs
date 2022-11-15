using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
     public virtual IEnumerator shootBullet()
    {
        if (gameManager.instance.playerScript.isShooting == false && Input.GetButton("Shoot"))
        {
            gameManager.instance.playerScript.isShooting = true;
            gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.shootAudioClip, gameManager.instance.playerScript.shootAudioVolume);
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gameManager.instance.playerScript.shootDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
                }
            }
            yield return new WaitForSeconds(gameManager.instance.playerScript.shootRate);
            gameManager.instance.playerScript.isShooting = false;
        }
    }
}
