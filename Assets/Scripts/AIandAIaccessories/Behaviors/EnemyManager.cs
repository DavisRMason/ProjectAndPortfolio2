using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamage
{
    public static EnemyManager instance;

    [Header("-----Enemy Stuff-----")]
    public GameObject enemy;
    public BaseEnemy enemyScript;

    public bool isDead = false;

    private void Awake()
    {
        instance = this;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<BaseEnemy>();
    }

    public void takeDamage(int dmg)
    {
        instance.enemyScript.HP -= dmg;
        instance.enemyScript.StartCoroutine(instance.enemyScript.FlashDamage());
        if (instance.enemyScript.HP <= 0)
            isDead = true;
        if (isDead) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
