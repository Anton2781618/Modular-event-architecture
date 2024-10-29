using UnityEngine;
using UnityEngine.AI;

public class NPCMovementSystem
{
    private Animator _animator;
    private NavMeshAgent _agent;

    public NPCMovementSystem(Animator animator, NavMeshAgent agent)
    {
        _animator = animator;

        _agent = agent;

    }

    private bool FaceToPoint(Vector3 point)
    {
        Vector3 direction = (point - _agent.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation, lookRotation, Time.deltaTime * 5f);

        return Quaternion.Angle(_agent.transform.rotation, lookRotation) < 10;
    }


    public void Move(Vector3 targetPosition)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) MoveToPoint(targetPosition);

        if(Vector3.Distance(_agent.transform.position, targetPosition) <= _agent.stoppingDistance ) 
        {
            FaceToPoint(targetPosition);
        }
    }

    //двигаться к точке, пересчитывая путь
    private bool MoveToPoint(Vector3 point)
    {
        _agent.SetDestination(point);

        _animator.SetFloat("Forward", _agent.velocity.magnitude);

        if(_agent.path.corners.Length > 1)
        {
            FaceToPoint(_agent.path.corners[1]);
        }

        return Vector3.Distance(_agent.transform.position, point) <= _agent.stoppingDistance && _agent.velocity.magnitude == 0;
    }
}
