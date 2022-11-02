using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class BaseEnemy : Tree
{
    [Header("-----Components-----")]
    public Transform[] waypoints;

    [Header("-----Enemy Stats-----")]
    public float speed;
    public float fovRange;
    public float attackRange;

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
}
