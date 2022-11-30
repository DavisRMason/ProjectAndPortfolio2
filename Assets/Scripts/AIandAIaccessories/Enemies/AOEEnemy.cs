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
    [SerializeField] Image HPBar;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerBaseSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] GameObject headPos;

    [Header("-----Deal Damage-----")]
    [SerializeField] int damageToDeal;
    [SerializeField] float dealDamageInterval;

    [SerializeField] bool SpawnerAlt;

    bool dealingDamage;
    bool isDead;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
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
            isDead = true;
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
