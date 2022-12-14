using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MomentumThrow : MonoBehaviour
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
        StartCoroutine(outOfBounds());
        if (gameObject.GetComponent<Collider>().enabled == false)
        {
            rb.useGravity = true;
            rb.AddForce(gameObject.transform.forward * 4000);
            Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
            rb.ResetCenterOfMass();
        }
        else
        {
            Vector3 temp = rb.position;
            temp.y += .75f;
            rb.position = temp;
            rb.useGravity = false;
            rb.AddForce(gameObject.transform.forward * 4000);
            Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
            StartCoroutine(TurnOnGravity());
        }
    }

    private void Update()
    {
        AttackEnemy();
        DetectGround();

        if(gameObject.GetComponent<Collider>().enabled == true)
        {
            rb.AddForce(gameObject.transform.forward * 200);
            rb.transform.rotation = Quaternion.LookRotation(gameManager.instance.player.transform.position - rb.transform.position);
        }
    }

    void AttackEnemy()
    {
        RaycastHit hit;

        if (Physics.SphereCast(attackPos.transform.position, 3, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
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

        if (Physics.Raycast(attackPos.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent<IDamage>() == null)
            {
                Instantiate(spear, gameObject.transform.position, gameObject.transform.rotation);
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

    IEnumerator TurnOnGravity()
    {
        yield return new WaitForSeconds(1);

        rb.useGravity = true;
    }

    IEnumerator outOfBounds()
    {
        yield return new WaitForSeconds(3);

        gameManager.instance.playerScript.changeWeapons();
        gameManager.instance.playerScript.weaponHave = true;
        Destroy(gameObject);
    }
}
