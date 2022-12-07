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


    #endregion

    #region Bools_&_Statics

    Vector3 move;
    Vector3 enemyPos;
    bool enemyDetected;
    bool once;
    float audioLevel = .25f;

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        enemyPos = Vector3.zero;
        if (gameObject.GetComponent<Collider>().enabled == false)
        {
            rb.useGravity = true;
            rb.AddForce(gameObject.transform.forward * 2500);
            Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
            rb.ResetCenterOfMass();
        }
        else
        {
            Vector3 temp = rb.position;
            temp.y += 1;
            rb.position = temp;
            rb.useGravity = false;
            rb.AddForce(gameObject.transform.forward * 2500);
            Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 2);
            StartCoroutine(TurnOnGravity());
        }
    }

    private void Update()
    {
        AttackEnemy();
        BelowValueReturn();

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

        if (Physics.Raycast(attackPos.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.DrawRay(attackPos.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.shootDamage);
                Instantiate(gameManager.instance.playerScript.hitEffectTwo, attackPos.transform.position, gameObject.transform.rotation);
                spearAudioSource.PlayOneShot(spearAudioClip, Random.Range(0.5f, 1f) + audioLevel);
            }
            else
            {
                Debug.DrawRay(attackPos.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
                Instantiate(spear, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemyDetected)
        {
            enemyPos = other.transform.position;
            enemyPos.y += 1;
            enemyDetected = true;
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

    void BelowValueReturn()
    {
        if (rb.transform.position.y < 0)
        {
            gameManager.instance.playerScript.changeWeapons();
            gameManager.instance.playerScript.weaponHave = true;
            Destroy(gameObject);
        }
    }
}