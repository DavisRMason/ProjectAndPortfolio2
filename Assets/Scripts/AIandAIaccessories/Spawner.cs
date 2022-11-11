using System.Collections;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] GameObject enemy;
    [SerializeField] Transform spawnPos;

    [Header("-----Spawn Stats-----")]
    [SerializeField] int enemiesToSpawn;
    [SerializeField] int timer;

    bool isSpawning;
    bool startSpawning;
    int enemiesSpawned;

    void Start()
    {
        gameManager.instance.updateUIEnemyCount(enemiesToSpawn);
    }

    void Update()
    {
        if (startSpawning && !isSpawning && enemiesSpawned < enemiesToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        Instantiate(enemy, spawnPos.position, enemy.transform.rotation);
        ++enemiesSpawned;

        yield return new WaitForSeconds(timer);
        isSpawning = false;
    }
}
