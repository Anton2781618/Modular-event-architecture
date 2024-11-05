using UnityEngine;
using UnityEngine.AI;

namespace ModularEventArchitecture
{
    [RequireComponent(typeof(NavMeshAgent))] 
    public class NPC : UnitEntity
    {
        public int ModulesCount { get => modules.Count; }
        
        protected override void Initialize()
        {

        }

        [ContextMenu("show")]
        public void Show()
        {
            LocalEvents.ShowAllEvents();
        }


        public override void Use()
        {
            base.Use();
        }
    }
}