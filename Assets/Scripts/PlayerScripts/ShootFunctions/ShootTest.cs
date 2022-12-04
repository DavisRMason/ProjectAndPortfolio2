using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : Shoot
{
    [SerializeField] GameObject obj;

    public override void shootBullet()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        //float x = Mathf.Sin(Camera.main.transform.rotation.x * Mathf.Deg2Rad);
        //float y = Mathf.Cos(Camera.main.transform.rotation.y * Mathf.Deg2Rad);
        //float z = Mathf.Tan(Camera.main.transform.rotation.z * Mathf.Deg2Rad);
        //Vector3 rot = new Vector3(x, y, z);

        Instantiate(obj, objectPos, Camera.main.transform.rotation);
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.shootAudioClip[Random.Range(0, gameManager.instance.playerScript.shootAudioClip.Count)], .5f);
        gameManager.instance.playerScript.weaponHave = false;
        gameManager.instance.playerScript.changeHand();
    }

    public void DistanceCalculater()
    {
        RaycastHit hit;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        gameManager.instance.playerScript.weaponFlyDist = gameManager.instance.playerScript.shootDist;

        if (Physics.Raycast(Camera.main.transform.position, objectPos, out hit, gameManager.instance.playerScript.shootDist))
        {
            if (hit.collider != null)
            {
                gameManager.instance.playerScript.weaponFlyDist = hit.distance;
            }
        }
    }
}
