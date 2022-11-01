using UnityEngine;
using BehaviourTree;

public class CheckAttackRange : Node
{
    private Transform transform;
    private Animator animator;

    public CheckAttackRange(Transform _transform)
    {
        transform = _transform;
        animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("Target");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if (Vector3.Distance(transform.position, target.position) <= BaseEnemy.attackRange)
        {
            animator.SetBool("Attacking", true);
            animator.SetBool("Walking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
