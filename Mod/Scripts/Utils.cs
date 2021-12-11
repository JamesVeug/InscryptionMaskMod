using System;
using System.IO;
using System.Reflection;
using DiskCardGame;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaskMod
{
    public static class Utils
    {
	    public static GameObject InstantiateMask(LeshyAnimationController.Mask mask, Transform parent)
	    {
		    if (InstantiateCustomPrefab(mask, parent, out GameObject customInstance))
		    {
			    return customInstance;
		    }

		    if (InstantiateVanillaPrefab(mask, parent, out GameObject vanillaInstance))
		    {
			    return vanillaInstance;
		    }

		    Debug.LogError("Could not instantiate mask!. Nothing has this! " + mask);
		    return null;
	    }

	    private static bool InstantiateVanillaPrefab(LeshyAnimationController.Mask mask, Transform parent, out GameObject clone)
	    {
		    GameObject original = ResourceBank.Get<GameObject>("Prefabs/Opponents/Leshy/Masks/Mask" + mask.ToString());
		    if (original == null)
		    {
			    clone = null;
			    return false;
		    }
		    
		    clone = GameObject.Instantiate(original, parent);
		    return true;
	    }

	    private static bool InstantiateCustomPrefab(LeshyAnimationController.Mask mask, Transform parent, out GameObject clone)
	    {
		    CustomMask customMask = CustomMask.GetRandomMask(mask);
            if (customMask == null)
            {
                // Show original mask because we didn't load any
                clone = null;
                return false;
            }
            
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(customMask.BundlePath);
            if (myLoadedAssetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle! " + customMask.BundlePath);
                clone = null;
                return false;
            }
            
            GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(customMask.BundlePrefabName);
            if (prefab == null)
            {
	            Debug.LogError($"Failed to load prefab from asset bundle! {customMask.BundlePath} {customMask.MaskName}");
	            clone = null;
	            return false;
            }
            
            clone = GameObject.Instantiate(prefab, parent);
            myLoadedAssetBundle.Unload(false);

            if (!string.IsNullOrEmpty(customMask.TextureOverridePath))
            {
                if (File.Exists(customMask.TextureOverridePath))
                {
                    byte[] imgBytes = File.ReadAllBytes(customMask.TextureOverridePath);
                    Texture2D texture = new Texture2D(2,2);
                    texture.LoadImage(imgBytes);
                    MeshRenderer renderer = clone.GetComponentInChildren<MeshRenderer>();
                    Material[] materials = renderer.materials;
                    materials[0].mainTexture = texture;
                    renderer.materials = materials;
                }
                else
                {
                    Plugin.Log.LogError($"Mask OverrideTexture for {customMask.MaskName} does not exist: {customMask.TextureOverridePath}");
                }
            }
            
            return true;
	    }
	    
        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName",
                    string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null,
                obj, new object[] { val });
        }
        
        public static void Print(GameObject go, string prefix = "\t")
        {
	        Plugin.Log.LogInfo(prefix + $"GameObject: {go}!");

	        string hierarchy = go.name;
	        Transform parent = go.transform.parent;
	        while (parent != null)
	        {
		        hierarchy = parent.name + "->" + hierarchy;
		        parent = parent.parent;
	        }
	        Plugin.Log.LogInfo(prefix + $"\tHierarchy: {hierarchy}!");
	        
	        foreach (Object component in go.GetComponents<Object>())
	        {
		        Plugin.Log.LogInfo(prefix + $"Component: {component}!");
		        Plugin.Log.LogInfo(prefix + $"\tName: {component.name}!");
		        Plugin.Log.LogInfo(prefix + $"\tType: {component.GetType()}!");
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

	        Plugin.Log.LogInfo(prefix + $"Children: {go.transform.childCount}!");
	        for (int i = 0; i < go.transform.childCount; i++)
	        {
		        Transform child = go.transform.GetChild(i);
		        Print(child.gameObject, prefix + "\t");
	        }
        }
    }
}