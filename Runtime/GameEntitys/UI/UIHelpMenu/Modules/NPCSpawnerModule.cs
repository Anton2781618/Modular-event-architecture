using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(UIHelpMenu))]
    public class NPCSpawnerModule : ModuleBase
    {
        public override void Initialize()
        {
        }

        public void SpawnNPC()
        {
            GlobalEventBus.Instance.Publish(GlobalEventBus.События.Команды.Заспавнить_моба, new SpawnEvent());
        }

        public override void UpdateMe()
        {
            
        }
    }
}