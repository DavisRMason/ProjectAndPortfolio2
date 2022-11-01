using UnityEngine;

namespace BehaviourTree
{
    public class TaskPatrol : Node
    {
        private Transform _transform;
        private Animator _animator;
        private Transform[] waypoints;

        private int currentWaypointIndex = 0;

        private float waitTime = 1f;
        private float waitCounter = 0f;
        private bool waiting = false;

        public TaskPatrol(Transform transform, Transform[] _waypoints)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            waypoints = _waypoints;
        }

        public override NodeState Evaluate()
        {
            if (waiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter > waitTime)
                {
                    waiting = false;
                    _animator.SetBool("Walking", true);
                }
            }
            else
            {
                Transform wp = waypoints[currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
                {
                    _transform.position = wp.position;
                    waitCounter = 0f;
                    waiting = true;

                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    _animator.SetBool("Walking", false);
                }
                else
                {
                    _transform.position = Vector3.MoveTowards(_transform.position, wp.position, BaseEnemy.speed * Time.deltaTime);
                    _transform.LookAt(wp.position);
                }
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
