using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace MaskMod.Patches
{
    [HarmonyPatch(typeof (DuplicateMergeSequencer), "SetSideHeadTalking", new System.Type[] {(typeof(bool))})]
    public class DuplicateMergeSequencer_SetSideHeadTalking
    {
        public static bool Prefix(bool sideHeadTalking)
        {
            // TODO: Make custom masks work with the side head rotating thingy
            // Requires: DoctorMask on the mask and a set of eyes ot turn on and off
            return false;
        }
    }
}