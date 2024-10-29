using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Animator _animator;
    abstract public void Attack(string animationName, bool isStun = false);
    abstract public void SetHitBoolOFF();

    public void Init(Animator animator)
    {
        _animator = animator;
    }
}