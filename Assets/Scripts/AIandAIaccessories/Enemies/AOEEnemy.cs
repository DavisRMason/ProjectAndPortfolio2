using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AOEEnemy : MonoBehaviour, IDamage
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
    [SerializeField] int funnyWait;
    [SerializeField] GameObject headPos;

    [Header("-----Deal Damage-----")]
    [SerializeField] int damageToDeal;
    [SerializeField] float dealDamageInterval;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audShoot;
    [SerializeField] AudioClip[] audDeath;

    [Header("----- Alt -----")]
    [SerializeField] bool SpawnerAlt;

    bool dealingDamage;
    bool isDead;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    float idleTime = 0;
    int hpOrig;


    void Start()
    {
        if (SpawnerAlt)
        {
            gameManager.instance.updateUIEnemyCount(1);
        }
        hpOrig = HP;
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

                FacePlayer();
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
            gameManager.instance.playerScript.playerSpeed = gameManager.instance.playerScript.playerOrigSpeed;
            isDead = true;
            gameManager.instance.updateEnemyNumber();
            anim.SetBool("Dead", true);
            agent.enabled = false;
            UI.SetActive(false);
            ParticleSystem.MainModule particle = GetComponentInChildren<ParticleSystem>().main;
            particle.loop = false;
            gameManager.instance.updateScore(700);
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

    IEnumerator DealFrost()
    {
        dealingDamage = true;
        yield return new WaitForSeconds(dealDamageInterval);
        gameManager.instance.playerScript.Damage(damageToDeal);
        dealingDamage = false;
    }

    IEnumerator FlashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3F);
        model.material.color = Color.white;
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

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            if (!dealingDamage && playerInRange && !isDead)
            {
                aud.PlayOneShot(audShoot[Random.Range(0, audShoot.Length)]);
                anim.SetTrigger("Sing");
                gameManager.instance.playerScript.playerSpeed /= 1.2f;
                StartCoroutine(DealFrost());
            }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.playerSpeed = gameManager.instance.playerScript.playerOrigSpeed;
            playerInRange = false;
        }
    }
}
