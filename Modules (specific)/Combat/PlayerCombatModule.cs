using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(Player))]
    public class PlayerCombatModule : ModuleBase
    {
        private bool _isUIOpen = false;
        private Animator _animator;
        [SerializeField] private Weapon _weapon;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_animator) _animator = Entity.GetCachedComponent<Animator>();
            
            _weapon.Init(_animator);
        }

        public override void UpdateMe()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && !_isUIOpen)
            {

                Attack();
            }
            else
            if(Input.GetKeyDown(KeyCode.Mouse1) && !_isUIOpen)
            {
                // Attack2();
            }
            else
            if(Input.GetKeyDown(KeyCode.Mouse2) && !_isUIOpen)
            {
                JumpAttack();
            }
        }

        public void SetHitBoolOFF()
        {
            if(_weapon)_weapon.SetHitBoolOFF();
        }

        public void Attack()
        {
            _weapon.Attack("Attack");
        }
        
        public void Attack2()
        {
            _weapon.Attack("Attack2");
        }
        
        public void JumpAttack()
        {
            _weapon.Attack("JumpAttack", true);
        }
    }
}