using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ChargedShotEnemy : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject UID;
    [SerializeField] Image HPBar;
    [SerializeField] AudioSource aud;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerBaseSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] float funnyWait;
    [SerializeField] GameObject headPos;

    [Header("-----Gun Stats-----")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform shootPos;
    [SerializeField] float chargeTime;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audShoot;
    [SerializeField] AudioClip[] audDeath;
    [SerializeField] AudioClip chargeSound;

    [Header("----- Alt -----")]
    [SerializeField] bool SpawnerAlt;

    bool isShooting;
    bool playerInRange;
    bool charging;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    float agentSpeedOrig;
    float idleTime = 0;
    float chargingTime;
    int hpOrig;


    void Start()
    {
        if (SpawnerAlt)
        {
            gameManager.instance.updateUIEnemyCount(1);
        }

        hpOrig = HP;
        agentSpeedOrig = agent.speed;
        stoppingDistOrig = agent.stoppingDistance;
        UpdateHPBar();
    }


    void Update()
    {
        if (agent.enabled)
        {
            IdleTime();
            if (playerInRange)
            {
                CanSeePlayer();
            }
        }

        if (UID.activeInHierarchy)
            UID.transform.position = UID.transform.position + (new Vector3(0, 3, 0) * Time.deltaTime);
    }

    void CanSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.transform.position);

        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(headPos.transform.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                //if (agent.remainingDistance < agent.stoppingDistance)
                FacePlayer();

                if (!isShooting && playerInRange)
                    ChargedShot();
            }
        }
    }

    void FacePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerBaseSpeed);
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        UI.SetActive(true);
        UpdateHPBar();
        StartCoroutine(DisableUI());

        agent.stoppingDistance = 0;
        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(FlashDamage());

        if (HP <= 0)
        {
            StartCoroutine(FlashTimeAdded());
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)]);
            gameManager.instance.updateEnemyNumber();
            anim.SetBool("Dead", true);
            agent.enabled = false;
            UI.SetActive(false);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(MegaDeath());
        }
    }

    void UpdateHPBar()
    {
        HPBar.fillAmount = (float)HP / (float)hpOrig;
    }

    void IdleTime()
    {
        if (!playerInRange)
            idleTime += Time.deltaTime;
        else
            idleTime = 0;
        if (idleTime >= funnyWait)
        {
            anim.SetTrigger("Taunt");
            idleTime = -2.6f;
        }
    }

    void ChargedShot()
    {
        charging = true;
        if (charging)
        {
            chargingTime += Time.deltaTime;
            FacePlayer();
        }
        if (chargingTime >= 0.0015)
            aud.PlayOneShot(chargeSound);
        if (chargingTime >= chargeTime)
        {
            StartCoroutine(Shoot());
            playerInRange = false;
            charging = false;
            chargingTime = 0;
        }
    }

    IEnumerator FlashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3F);
        model.material.color = Color.white;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        agent.speed = 0;

        anim.SetTrigger("Shoot");

        aud.priority = 1;
        aud.PlayOneShot(audShoot[Random.Range(0, audShoot.Length)]);
        Instantiate(hitEffect, shootPos.position, hitEffect.transform.rotation);
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(chargeTime);
        agent.speed = agentSpeedOrig;
        isShooting = false;
    }

    IEnumerator FlashTimeAdded()
    {
        UID.SetActive(true);
        yield return new WaitForSeconds(1.6f);
        UID.SetActive(false);
    }

    IEnumerator DisableUI()
    {
        yield return new WaitForSeconds(7);
        if (UI.activeInHierarchy)
            UI.SetActive(false);
    }

    IEnumerator MegaDeath()
    {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }
}
