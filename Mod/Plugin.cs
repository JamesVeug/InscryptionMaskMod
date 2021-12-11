using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Mask = DiskCardGame.LeshyAnimationController.Mask;

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

            List<CustomMaskData> defaultMasks = new List<CustomMaskData>();
            
            string pluginPath = Paths.PluginPath;
            foreach (string filePath in Directory.EnumerateFiles(pluginPath, "*.custommask", SearchOption.AllDirectories))
            {
	            CustomMaskData maskData = CustomMaskData.FromJson(filePath);
	            if (maskData == null)
	            {
		            Log.LogWarning("Failed to load custom mask at: " + filePath);
		            continue;
	            }
	            
	            if (filePath.Contains(PluginDirectory))
	            {
		            defaultMasks.Add(maskData);
	            }
	            else
	            {
		            maskData.Load();
	            }
            }

            if (CustomMask.customMasks.Count == 0)
            {
	            Logger.LogInfo($"No mods exist so adding default masks!");
	            foreach (CustomMaskData maskData in defaultMasks)
	            {
		            maskData.Load();
	            }
            }

            // Backgrounds
            Logger.LogInfo($"Loaded {PluginName}!");
        }
    }
}
