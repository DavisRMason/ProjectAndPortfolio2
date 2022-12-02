using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LectureEnemy : MonoBehaviour, IDamage
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
    //[SerializeField] int speedChase;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    //[SerializeField] int roamDist;
    //[SerializeField] int animLerpSpeed;
    [SerializeField] GameObject headPos;

    [Header("-----Gun Stats-----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;

    [SerializeField] bool SpawnerAlt;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDirection;
    Vector3 startingPos;
    float angleToPlayer;
    float stoppingDistOrig;
    float agentSpeedOrig;
    int hpOrig;


    void Start()
    {
        if (SpawnerAlt)
        {
            gameManager.instance.updateUIEnemyCount(1);
        }

        hpOrig = HP;
        agentSpeedOrig = agent.speed;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        //roam();
        UpdateHPBar();
    }


    void Update()
    {
        //anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));
        if (agent.enabled)
        {
            if (playerInRange)
            {
                CanSeePlayer();
            }
            //else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
            //    roam();
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

                if (!isShooting && playerInRange)
                    StartCoroutine(Shoot());
            }
        }
    }

    //void roam()
    //{
    //    agent.stoppingDistance = 0;

    //    Vector3 randomDir = Random.insideUnitSphere * roamDist;
    //    randomDir += startingPos;

    //    NavMeshHit hit;
    //    NavMesh.SamplePosition(new Vector3(randomDir.x, 0, randomDir.z), out hit, 1, 1);
    //    NavMeshPath path = new NavMeshPath();
    //    agent.CalculatePath(hit.position, path);
    //    agent.SetPath(path);
    //}

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
