using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GungnirThrow : MonoBehaviour
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
    [SerializeField] int radius;


    #endregion

    #region Bools_&_Statics

    Vector3 move;
    Vector3 enemyPos;
    bool enemyDetected;
    bool once;

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(outOfBounds());
        enemyPos = Vector3.zero;
        if (gameObject.GetComponent<Collider>().enabled == false)
        {
            rb.useGravity = true;
            rb.AddForce(gameObject.transform.forward * 3500);
            Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
            rb.ResetCenterOfMass();
        }
        else
        {
            Vector3 temp = rb.position;
            temp.y += 1;
            rb.position = temp;
            rb.useGravity = false;
            rb.AddForce(gameObject.transform.forward * 3500);
            Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
            StartCoroutine(TurnOnGravity());
        }
    }

    private void Update()
    {
        AttackEnemy();
        DestroySpear();

        if(!enemyDetected)
            DetectEnemy();

        if(enemyDetected)
        {
            rb.AddForce(gameObject.transform.forward * 200);
            rb.transform.rotation = Quaternion.LookRotation(enemyPos - rb.transform.position);
        }

        if (gameObject.GetComponent<Collider>().enabled == true)
        {
            rb.AddForce(gameObject.transform.forward * 200);
            rb.transform.rotation = Quaternion.LookRotation(gameManager.instance.player.transform.position - rb.transform.position);
        }
    }

    void AttackEnemy()
    {
        RaycastHit hit;

        if (Physics.SphereCast(attackPos.transform.position, 2, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
                Instantiate(gameManager.instance.playerScript.hitEffectTwo, attackPos.transform.position, gameObject.transform.rotation);
                spearAudioSource.PlayOneShot(spearAudioClip);
            }
        }
    }

    void DestroySpear()
    {
        RaycastHit hit;

        if (Physics.Raycast(attackPos.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent <IDamage>() == null)
            {
                Instantiate(spear, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    void DetectEnemy()
    {
        RaycastHit hit;

        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 right = Vector3.right;
        Vector3 left = Vector3.left;
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;

        if(Physics.SphereCast(attackPos.transform.position, radius, front, out hit) || Physics.SphereCast(attackPos.transform.position, radius, back, out hit) || Physics.SphereCast(attackPos.transform.position, radius, right, out hit) || Physics.SphereCast(attackPos.transform.position, radius, left, out hit) || Physics.SphereCast(attackPos.transform.position, radius, up, out hit) || Physics.SphereCast(attackPos.transform.position, radius, down, out hit))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                enemyPos = hit.collider.transform.position;
                enemyDetected = true;
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