using UnityEngine;
using ModularEventArchitecture;

namespace ModularEventArchitecture.Samples.Dialog
{
    public class DialogModule : ModuleBase
    {
        [SerializeField] private bool autoSkipEnabled = false;
        [SerializeField] private float autoSkipDelay = 3f;
        
        public override void Initialize()
        {
            Debug.Log("Dialog Module initialized");
            Debug.Log($"Auto skip is {(autoSkipEnabled ? "enabled" : "disabled")} with delay {autoSkipDelay}s");
        }

        public override void Dispose()
        {
            Debug.Log("Dialog Module disposed");
        }
    }
}
