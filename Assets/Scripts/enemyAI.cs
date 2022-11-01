using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats")]
    [SerializeField] int HP;
    [SerializeField] float wait;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.enemiesToKill++;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

        playerDir = (gameManager.instance.player.transform.position - transform.position);

        if(playerInRange)
        {
            facePlayer();
            if (!isShooting)
            StartCoroutine(shoot());
        }
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashDamage());
        //If HP falls low enough, destroy the object, or implement something else
        if(HP <= 0)
        {
            gameManager.instance.updateEnemyNumber();
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;

        yield return new WaitForSeconds(wait);

        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting=false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
