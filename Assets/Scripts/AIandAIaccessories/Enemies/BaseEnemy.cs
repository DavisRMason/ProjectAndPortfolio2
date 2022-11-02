using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class BaseEnemy : Tree, IDamage
{
    [Header("-----Components-----")]
    public Transform[] waypoints;
    [SerializeField] Renderer model;

    [Header("-----Enemy Stats-----")]
    public float speed;
    public float fovRange;
    public float attackRange;
    public int HP;

    public bool isDead = false;

    protected override Node setupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckAttackRange(transform),
                new TaskAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            new TaskPatrol(transform, waypoints),
        });

        return root;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashDamage());
        if (HP <= 0)
        {
            isDead = true;
            Die(); 
        }
    }

    public IEnumerator FlashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3F);
        model.material.color = Color.white;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
