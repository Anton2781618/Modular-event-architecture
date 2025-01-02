using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(UIWindow))]
    public class NPCSpawnerModule : ModuleBase
    {
        public override void Initialize()
        {
        }

        public void SpawnNPC()
        {
            GlobalEventBus.Instance.Publish(BasicActionsTypes.Commands.Spawn_Mob, new SpawnEvent());
        }

        public override void UpdateMe()
        {
            
        }
    }
}