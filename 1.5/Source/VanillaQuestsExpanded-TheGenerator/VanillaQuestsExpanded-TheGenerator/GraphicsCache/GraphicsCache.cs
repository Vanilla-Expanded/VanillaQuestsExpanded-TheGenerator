using System;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {
        public static readonly Material CriticalBreakdown = MaterialPool.MatFrom("UI/Gizmos/CriticalBreakDown_Overlay", ShaderDatabase.MetaOverlay);

    }
}
