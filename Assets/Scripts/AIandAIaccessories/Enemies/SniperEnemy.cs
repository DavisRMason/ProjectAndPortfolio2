using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SniperEnemy : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] GameObject UI;
    [SerializeField] Image HPBar;
    [SerializeField] LineRenderer line;
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
    [SerializeField] float shootRate;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audShoot;
    [SerializeField] AudioClip[] audDeath;

    [Header("----- Alt -----")]
    [SerializeField] bool SpawnerAlt;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    float agentSpeedOrig;
    float idleTime = 0;
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

                if (!isShooting  && playerInRange)
                    StartCoroutine(Shoot());
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
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)]);
            line.enabled = false;
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

        aud.PlayOneShot(audShoot[Random.Range(0, audShoot.Length)]);
        Instantiate(hitEffect, shootPos.position, hitEffect.transform.rotation);
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        agent.speed = agentSpeedOrig;
        isShooting = false;
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

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
