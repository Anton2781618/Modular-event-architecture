using UnityEngine;
using ModularEventArchitecture;

namespace ModularEventArchitecture.Modules.Dialog
{
    public class DialogModule : ModuleBase
    {
        [SerializeField] private bool autoSkipEnabled = false;
        [SerializeField] private float autoSkipDelay = 3f;

        public override void UpdateMe()
        {
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            
        }

    }
}
