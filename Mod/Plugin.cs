using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace MaskMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.inscryption.maskmod";
	    public const string PluginName = "Mask Mod";
	    public const string PluginVersion = "0.1.0.0";

        public static string PluginDirectory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            PluginDirectory = this.Info.Location.Replace("MaskMod.dll", "");
            new Harmony(PluginGuid).PatchAll();
            
            CustomMask.AddCustomMask(LeshyAnimationController.Mask.Prospector,
	            "Scream", 
	            "Masks/custommask", 
	            "CustomMask",
	            "Masks/screammask.png"
	            );
            
            CustomMask.AddCustomMask(LeshyAnimationController.Mask.Prospector,
	            "Pig Nose", 
	            "Masks/sphere", 
	            "Sphere"
            );

            // Backgrounds
            Logger.LogInfo($"Loaded {PluginName}!");
        }
    }
}
