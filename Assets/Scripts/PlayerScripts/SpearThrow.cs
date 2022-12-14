using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpearThrow : MonoBehaviour
{
    #region Variables

    #region Unity_Editor

    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject spear;
    [SerializeField] GameObject attackPos;
    [SerializeField] ParticleSystem effect;
    [SerializeField] int lowestValue;
    [SerializeField] AudioSource spearAudioSource;
    [SerializeField] AudioClip spearAudioClip;

    [Header("----- Spear Stats -----")]
    [SerializeField] int attackDist;


    #endregion

    #region Bools_&_Statics

    Vector3 move;

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = true;
        rb.AddForce(gameObject.transform.forward * 4000);
        Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
        rb.ResetCenterOfMass();
        StartCoroutine(outOfBounds());
    }

    private void Update()
    {
        AttackEnemy();
        DetectGround();
    }

    void AttackEnemy()
    {
        RaycastHit hit;

        if(Physics.SphereCast(attackPos.transform.position, 3, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if(hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.DrawRay(attackPos.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
                Instantiate(gameManager.instance.playerScript.hitEffectTwo, attackPos.transform.position, gameObject.transform.rotation);
                spearAudioSource.PlayOneShot(spearAudioClip);
            }
        }
    }

    void DetectGround()
    {
        RaycastHit hit;

        if(Physics.Raycast(attackPos.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent<IDamage>() == null)
            {
                Instantiate(spear, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
            else if (hit.collider.CompareTag("DeathZone"))
            {
                gameManager.instance.playerScript.weaponHave = true;
                gameManager.instance.playerScript.changeWeapons();
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }

    void BelowValueReturn()
    {
        if (rb.transform.position.y < 0)
        {
            gameManager.instance.playerScript.changeWeapons();
            gameManager.instance.playerScript.weaponHave = true;
            Destroy(gameObject);
        }
    }

    IEnumerator outOfBounds()
    {
        yield return new WaitForSeconds(3);

        gameManager.instance.playerScript.changeWeapons();
        gameManager.instance.playerScript.weaponHave = true;
        Destroy(gameObject);
    }
}