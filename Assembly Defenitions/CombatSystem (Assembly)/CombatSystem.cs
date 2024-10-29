using UnityEngine;
using UnityEngine.AI;

public class CombatSystem
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private GameObject _target;

    private float delayTime = 0;

    public CombatSystem(Animator animator, NavMeshAgent agent)
    {
        _animator = animator;
        _agent = agent;
    }

    public void Attack(GameObject _target)
    {
        if (Time.frameCount % 500 == 0)
        {
            if(Vector3.Distance(_animator.transform.position, _target.transform.position) <= _agent.stoppingDistance ) 
            {
                if(delayTime == 0) _animator.SetTrigger("Attack");
            }
        }
    }
}
