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
	    public const string PluginVersion = "0.0.1.0";

        public static string PluginDirectory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            PluginDirectory = this.Info.Location.Replace("MaskMod.dll", "");
            new Harmony(PluginGuid).PatchAll();
            
            CustomMask.AddCustomMask(LeshyAnimationController.Mask.Prospector,
	            "Test", 
	            "Masks/sphere", 
	            "CustomMask"
	            );

           /* string assetsPath = Application.streamingAssetsPath;
            Logger.LogInfo($"Streaming assets folder {assetsPath}");
            if (!Directory.Exists(assetsPath))
            {
	            Directory.CreateDirectory(assetsPath);
            }
            
            string maskPath = Path.Combine(Plugin.PluginDirectory, "Masks/model.obj");*/
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
            
            /*string prefabPath = Path.Combine(Plugin.PluginDirectory, "Masks/model.obj");
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
            }*/


            // Backgrounds
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        public static void Print(GameObject go, string prefix = "\t")
        {
	        Log.LogInfo(prefix + $"GameObject: {go}!");

	        string hierarchy = go.name;
	        Transform parent = go.transform.parent;
	        while (parent != null)
	        {
		        hierarchy = parent.name + "->" + hierarchy;
		        parent = parent.parent;
	        }
	        Log.LogInfo(prefix + $"\tHierarchy: {hierarchy}!");
	        
	        foreach (Object component in go.GetComponents<Object>())
	        {
		        Log.LogInfo(prefix + $"Component: {component}!");
		        Log.LogInfo(prefix + $"\tName: {component.name}!");
		        Log.LogInfo(prefix + $"\tType: {component.GetType()}!");
		        if (component as MeshRenderer)
		        {
			        MeshRenderer m = (MeshRenderer)component;
			        foreach (Material material in m.materials)
			        {
				        Plugin.Log.LogInfo(prefix + $"\tMaterial: " + material.name);
				        if (material.mainTexture != null)
				        {
					        Plugin.Log.LogInfo(prefix + $"\t\tTexture: " + material.mainTexture.name);
				        }
				        else
				        {
					        Plugin.Log.LogInfo(prefix + $"\t\tTexture: null");
				        }

				        Plugin.Log.LogInfo(prefix + $"\t\tshader: " + material.shader.name);
			        }
		        }
		        else if (component as MeshFilter)
		        {
			        MeshFilter mf = (MeshFilter)component;
			        Plugin.Log.LogInfo(prefix + $"\tName: " + mf.name);
			        Plugin.Log.LogInfo(prefix + $"\tMesh: " + mf.mesh);
		        }
		        else if (component as Transform)
		        {
			        Transform t = (Transform)component;
			        Plugin.Log.LogInfo(prefix + $"\tPosition: " + t.position + " " + t.localPosition);
			        Plugin.Log.LogInfo(prefix + $"\tLossyScale: " + t.lossyScale + " " + t.localScale);
		        }
	        }

	        Log.LogInfo(prefix + $"Children: {go.transform.childCount}!");
	        for (int i = 0; i < go.transform.childCount; i++)
	        {
		        Transform child = go.transform.GetChild(i);
		        Print(child.gameObject, prefix + "\t");
	        }
        }
        
    }
}
