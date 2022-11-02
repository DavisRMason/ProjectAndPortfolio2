using UnityEngine;
using BehaviourTree;

public class TaskGoToTarget : Node
{
    private Transform transform;

    public TaskGoToTarget(Transform _transform)
    {
        transform = _transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, EnemyManager.instance.enemyScript.speed * Time.deltaTime);
            transform.LookAt(target.position);
        }
        state = NodeState.RUNNING;
        return state;
    }
}
