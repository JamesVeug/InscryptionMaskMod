using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace MaskMod.Patches
{
    [HarmonyPatch(typeof (LeshyAnimationController), "SpawnMask", new System.Type[] {typeof (LeshyAnimationController.Mask), (typeof(bool))})]
    public class LeshyAnimationController_SpawnMask
    {
        /// <summary>
        /// https://answers.unity.com/questions/1339966/how-to-load-a-gameobject-from-byte-in-the-scene-on.html
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="mask"></param>
        /// <param name="justHead"></param>
        public static bool Prefix(LeshyAnimationController __instance, LeshyAnimationController.Mask mask, bool justHead = false)
        {
            CustomMask customMask = CustomMask.GetRandomMask(mask);
            if (customMask == null)
            {
                // Show original mask because we didn't load any
                return true;
            }
            
            if (__instance.CurrentMask != null)
            {
                Object.Destroy(__instance.CurrentMask);
                if (!justHead)
                {
                    Object.Destroy(__instance.currentHeldMask);
                }
            }

            GameObject clone = Utils.InstantiateMask(mask, __instance.maskParent);
            __instance.SetPrivatePropertyValue("CurrentMask", clone);
            
            if (!justHead)
            {
                __instance.currentHeldMask = Object.Instantiate<GameObject>(clone, __instance.heldMaskParent);
            }
            
            return false;
        }
    }
}