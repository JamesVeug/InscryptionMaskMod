using System.IO;
using System.Xml.XPath;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using Dummiesman;
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
	    public const string PluginVersion = "0.0.1.0";

        public static string PluginDirectory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            PluginDirectory = this.Info.Location.Replace("MaskMod.dll", "");
            new Harmony(PluginGuid).PatchAll();
            
            CustomMask.AddCustomMask("Test", 
	            "Masks/custommask", 
	            "Masks/CustomMaskMat.mat", 
	            "Masks/CustomMask.png", 
	            LeshyAnimationController.Mask.Prospector);

            string assetsPath = Application.streamingAssetsPath;
            Logger.LogInfo($"Streaming assets folder {assetsPath}");
            if (!Directory.Exists(assetsPath))
            {
	            Directory.CreateDirectory(assetsPath);
            }
            
            string maskPath = Path.Combine(Plugin.PluginDirectory, "Masks/model.obj");
            /*string fileName = Path.GetFileName(maskPath);
            string streamingFilePath = Path.Combine(assetsPath, fileName);
            string persistentFilePath = Path.Combine(Application.persistentDataPath, fileName);
            
            Logger.LogInfo($"Resource Path assets folder {maskPath}");
            Logger.LogInfo($"Streaming Path assets folder {streamingFilePath}");
            Logger.LogInfo($"Persist Path assets folder {persistentFilePath}");
            Logger.LogInfo($"Prefab Path assets folder {persistentFilePath}");
            
            Mesh oldMesh = Resources.Load<Mesh>(maskPath);
            Logger.LogInfo($"Resource Mesh Loaded: {oldMesh}!");
            
            File.Copy(maskPath, streamingFilePath, true);
            Mesh mesh = Resources.Load<Mesh>(streamingFilePath);
            Logger.LogInfo($"Streaming Mesh Loaded: {mesh}!");
            
            File.Copy(maskPath, persistentFilePath, true);
            Mesh persistentMesh = Resources.Load<Mesh>(persistentFilePath);
            Logger.LogInfo($"Persistent Mesh Loaded: {persistentMesh}!");*/
            
            string prefabPath = Path.Combine(Plugin.PluginDirectory, "Masks/model.obj");
            Logger.LogInfo($"Loading prefab from: {prefabPath}!");
            GameObject loadedObject = new OBJLoader().Load(prefabPath);
            Logger.LogInfo($"Loaded prefab: {loadedObject}!");
            Print(loadedObject);
            
            string bundlePath = Path.Combine(Plugin.PluginDirectory, "Masks/custommask");
            Logger.LogInfo($"Loading bundle: {bundlePath}!");
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(bundlePath);
            if (myLoadedAssetBundle == null)
            {
	            Debug.Log("Failed to load AssetBundle!");
            }
            else
            {
	            Logger.LogInfo($"Loaded bundle!");
	            GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>("CustomMask");
	            Logger.LogInfo($"Prefab: " + prefab);
	            Print(prefab);
	            myLoadedAssetBundle.Unload(false);
            }


            // Backgrounds
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void Print(GameObject go, string prefix = "\t")
        {
	        Logger.LogInfo(prefix + $"GameObject: {go}!");
	        foreach (Object component in go.GetComponents<Object>())
	        {
		        Logger.LogInfo(prefix + $"Component: {component}!");
		        Logger.LogInfo(prefix + $"\tName: {component.name}!");
		        Logger.LogInfo(prefix + $"\tType: {component.GetType()}!");
	        }

	        Logger.LogInfo(prefix + $"Children: {go.transform.childCount}!");
	        for (int i = 0; i < go.transform.childCount; i++)
	        {
		        Transform child = go.transform.GetChild(i);
		        Print(child.gameObject, prefix + "\t");
	        }
        }
        
    }
}
