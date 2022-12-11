using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] GameObject UI;
    [SerializeField] Image HPBar;
    [SerializeField] AudioSource aud;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerBaseSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] GameObject headPos;

    [Header("-----Gun Stats-----")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform[] shootPos;
    [SerializeField] float shootRate;

    [Header("-----Explosion Stats-----")]
    [SerializeField] GameObject explosion;
    [SerializeField] Transform explosionPos;

    [Header("-----Sword Stats-----")]
    [SerializeField] GameObject Sword;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audShoot;
    [SerializeField] AudioClip audMelee;
    [SerializeField] AudioClip audExplode;
    [SerializeField] AudioClip[] audDeath;

    [Header("----- Alt -----")]
    [SerializeField] bool SpawnerAlt;

    bool isShooting;
    bool isAttacking;
    bool hasAttacked;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    float agentSpeedOrig;
    int hpOrig;
    int timesShot;


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

                FacePlayer();

                if (!isShooting && playerInRange)
                {
                    if (timesShot == 3)
                        anim.SetTrigger("Shoot");
                    else if (timesShot == 8)
                    {
                        Explode();
                        timesShot = 0;
                    }
                    else if (!isAttacking)
                        StartCoroutine(Shoot());
                }
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
            gameManager.instance.updateEnemyNumber();
            anim.SetBool("Dead", true);
            agent.enabled = false;
            UI.SetActive(false);
            gameManager.instance.updateScore(1000);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(MegaDeath());
        }
    }

    void UpdateHPBar()
    {
        HPBar.fillAmount = (float)HP / (float)hpOrig;
    }

    void Explode()
    {
        anim.SetTrigger("Kaboom");
        aud.PlayOneShot(audExplode);
        Instantiate(explosion, explosionPos.position, transform.rotation);
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

        anim.SetTrigger("Locked");

        for (int i = 0; i < shootPos.Length; ++i)
        {
            Instantiate(bullet, shootPos[i].position, shootPos[i].rotation);
            Instantiate(hitEffect, shootPos[i].position, hitEffect.transform.rotation);
            aud.PlayOneShot(audShoot[Random.Range(0, audShoot.Length)]);
        }
        ++timesShot;

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

    public void MeleeOn()
    {
        Sword.SetActive(true);
        aud.PlayOneShot(audMelee);
    }

    public void MeleeOff()
    {
        Sword.SetActive(false);
        ++timesShot;
    }
}
