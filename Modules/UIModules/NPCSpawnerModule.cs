using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CompatibleUnit(typeof(UIHelpMenu))]
public class NPCSpawnerModule : ModuleBase
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public void SpawnNPC()
    {
        GlobalEventBus.Instance.Publish(GlobalEventBus.События.Команды.Заспавнить_моба, new SpawnEvent());
    }

    public override void UpdateMe()
    {
        
    }
}