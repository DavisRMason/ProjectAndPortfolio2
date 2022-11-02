using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Header("-----Enemy Stuff-----")]
    public GameObject enemy;
    public BaseEnemy enemyScript;

    private int hP;

    private void Awake()
    {
        instance = this;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<BaseEnemy>();
    }

    public bool TakeHit()
    {
        hP -= 10;
        bool isDead = hP <= 0;
        if (isDead) Die();
        return isDead;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
