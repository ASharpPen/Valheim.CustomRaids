using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Serverside.StartConditions
{
    /// <summary>
    /// BaseValue is present on Player.ZDO.
    /// It is updated every FixedUpdate, by applying a physics sphere collider with a radius of 20m.
    /// Uses a BitMask from EffectArea, with name PlayerBase (value 4).
    /// 
    /// EffectArea with type PlayerBase is part of the prefab of specific pieces.
    /// 
    /// This also means BaseValue has absolutely nothing to do with comfort level, its only coincidental.
    /// </summary>
    class ConditionNearBase
    {
    }
}
