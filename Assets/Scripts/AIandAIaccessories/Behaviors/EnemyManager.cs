using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Header("-----Enemy Stuff-----")]
    public GameObject enemy;
    public BaseEnemy enemyScript;

    private void Awake()
    {
        instance = this;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<BaseEnemy>();
    }
}
