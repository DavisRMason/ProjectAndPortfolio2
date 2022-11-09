using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool isShooting;
    public int shootDist;
    public int shootDamage;
    public float shootRate;
    virtual public void Start()
    {
        isShooting = gameManager.instance.playerScript.isShooting;
        shootDist = gameManager.instance.playerScript.shootDist;
        shootDamage = gameManager.instance.playerScript.shootDamage;
        shootRate = gameManager.instance.playerScript.shootRate;
    }

     public virtual IEnumerator shootBullet()
    {
        if (isShooting == false && Input.GetButton("Shoot"))
        {
            isShooting = true;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
}
