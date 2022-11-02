using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class BaseEnemy : Tree
{
    [Header("-----Components-----")]
    public Transform[] waypoints;
    [SerializeField] Renderer model;

    [Header("-----Enemy Stats-----")]
    public float speed;
    public float fovRange;
    public float attackRange;
    public int HP;

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

    public IEnumerator FlashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3F);
        model.material.color = Color.white;
    }
}
