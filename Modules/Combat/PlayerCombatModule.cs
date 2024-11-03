using UnityEngine;

[CompatibleUnit(typeof(Player))]
public class PlayerCombatModule : ModuleBase
{
    private bool isUIOpen = false;
    private Animator _animator;
    [SerializeField] private Weapon weapon;

    protected override void Initialize()
    {
        base.Initialize();
        
        if (!_animator) _animator = Character.GetCachedComponent<Animator>();
        
        weapon.Init(_animator);
    }

    public override void UpdateMe()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !isUIOpen)
        {

            Attack();
        }
        else
        if(Input.GetKeyDown(KeyCode.Mouse1) && !isUIOpen)
        {
            // Attack2();
        }
        else
        if(Input.GetKeyDown(KeyCode.Mouse2) && !isUIOpen)
        {
            JumpAttack();
        }
    }

    public void SetHitBoolOFF()
    {
        if(weapon)weapon.SetHitBoolOFF();
    }

    public void Attack()
    {
        weapon.Attack("Attack");
    }
    
    public void Attack2()
    {
        weapon.Attack("Attack2");
    }
    
    public void JumpAttack()
    {
        weapon.Attack("JumpAttack", true);
    }
}
