using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace MaskMod.Patches
{
    [HarmonyPatch(typeof (LeshyMaskOrbiter), "SpawnMasks", new System.Type[] {typeof (List<Opponent.Type>)})]
    public class LeshyMaskOrbiter_SpawnMasks
    {
        public static bool Prefix(LeshyMaskOrbiter __instance, List<Opponent.Type> bossTypes)
        {
            for (int i = 0; i < bossTypes.Count; i++)
            {
                LeshyAnimationController.Mask mask = __instance.BossToMask(bossTypes[i]);
                Transform parent = __instance.maskParents[i];
                Utils.InstantiateMask(mask, parent);
            }
            
            return false;
        }
    }
}