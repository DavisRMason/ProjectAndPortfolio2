using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int hP;

    private void Awake()
    {
        hP = 30;
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
