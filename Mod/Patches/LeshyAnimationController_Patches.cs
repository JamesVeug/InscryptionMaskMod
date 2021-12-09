using System.Collections.Generic;
using System.IO;
using DiskCardGame;
using Dummiesman;
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
            Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] Prefix " + mask);
            foreach (CustomMask customMask in CustomMask.customMasks)
            {
                if (__instance.CurrentMask != null)
                {
                    Object.Destroy(__instance.CurrentMask);
                    if (!justHead)
                    {
                        Object.Destroy(__instance.currentHeldMask);
                    }
                }
                
                Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] loading: " + customMask.ModelPath);
                /*OBJLoader objLoader = new OBJLoader();
                GameObject loadedObject = objLoader.Load(customMask.ModelPath, customMask.MaterialPath);
                
                Material[] LoadedMaterials = new Material[objLoader.Materials.Count];
                objLoader.Materials.Values.CopyTo(LoadedMaterials, 0);

                foreach (Material material in LoadedMaterials)
                {
                    Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] material: " + material.name + " " + material);
                    Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] material: " + material.shader.name);

                    Texture texture = Utils.GetTextureFromPath(customMask.TexturePath);
                    Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] material texture: " + texture.name + " " + texture);
                    material.mainTexture = texture;
                    material.ToFadeMode();
                }

                Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] loaded prefab: " + loadedObject);
                GameObject clone = UnityEngine.Object.Instantiate(loadedObject, __instance.maskParent);
                MeshRenderer meshRenderer = clone.GetComponentInChildren<MeshRenderer>();
                Material[] materials = meshRenderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = LoadedMaterials[i];
                }
                meshRenderer.materials = materials;*/
                
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(customMask.ModelPath);
                if (myLoadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                }
                
                Plugin.Log.LogInfo($"Loaded bundle!");
                GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>("CustomMask");
                GameObject clone = GameObject.Instantiate(prefab, __instance.maskParent);
                Plugin.Log.LogInfo($"Prefab: " + prefab);
                myLoadedAssetBundle.Unload(false);

                __instance.SetPrivatePropertyValue("CurrentMask", clone);
                if (!justHead)
                {
                    __instance.currentHeldMask = Object.Instantiate<GameObject>(clone, __instance.heldMaskParent);
                }
                Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] done");
                return false;
            }
            
            return true;
        }
    }
}