using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Effect Template", menuName = "Game/Effect Template")]
public class EffectTemplate : ScriptableObject
{
    [Header("Эффект")]
    public Effect Effects;

}
