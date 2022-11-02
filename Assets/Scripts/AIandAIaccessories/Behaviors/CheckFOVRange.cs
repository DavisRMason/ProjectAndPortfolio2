using BehaviourTree;
using UnityEngine;

public class CheckFOVRange : Node
{
    private static int playerLayerMask = 1 << 3;

    private Transform transform;
    private Animator animator;

    public CheckFOVRange(Transform _transform)
    {
        transform = _transform;
        animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, EnemyManager.instance.enemyScript.fovRange, playerLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                animator.SetBool("Walking", true);
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}