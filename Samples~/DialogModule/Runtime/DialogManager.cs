using UnityEngine;
using ModularEventArchitecture;

namespace ModularEventArchitecture.Modules.Dialog
{
    public class DialogManager : ManagerEntity
    {
        protected override void OnInitialize()
        {
            Debug.Log("DialogManager initialized");
        }

        protected override void OnDispose()
        {
            Debug.Log("DialogManager disposed");
        }
    }
}
