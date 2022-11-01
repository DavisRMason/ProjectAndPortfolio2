using UnityEngine;
using BehaviourTree;

public class TaskAttack : Node
{
    private Animator animator;

    private Transform lastTarget;
    private EnemyManager enemyManager;

    private float attackTime = 1f;
    private float attackCounter = 0f;

    public TaskAttack(Transform _transform)
    {
        animator = _transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("Target");

        if (target != lastTarget)
        {
            enemyManager = target.GetComponent<EnemyManager>();
            lastTarget = target;
        }

        attackCounter = Time.deltaTime;
        if (attackCounter >= attackTime)
        {
            bool enemyIsDead = enemyManager.TakeHit();
            if (enemyIsDead)
            {
                ClearData("Target");
                animator.SetBool("Attacking", false);
                animator.SetBool("Walking", true);
            }
            else
            {
                attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
