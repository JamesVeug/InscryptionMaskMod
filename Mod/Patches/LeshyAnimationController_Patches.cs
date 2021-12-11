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
            CustomMask customMask = CustomMask.GetRandomMask();
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
            
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(customMask.ModelPath);
            if (myLoadedAssetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle! " + customMask.ModelPath);
                return true;
            }
            
            GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(customMask.PrefabName);
            GameObject clone = GameObject.Instantiate(prefab, __instance.maskParent);
            myLoadedAssetBundle.Unload(false);

            if (!string.IsNullOrEmpty(customMask.TexturePath))
            {
                if (File.Exists(customMask.TexturePath))
                {
                    byte[] imgBytes = File.ReadAllBytes(customMask.TexturePath);
                    Texture2D texture = new Texture2D(2,2);
                    texture.LoadImage(imgBytes);
                    MeshRenderer renderer = clone.GetComponentInChildren<MeshRenderer>();
                    Material[] materials = renderer.materials;
                    materials[0].mainTexture = texture;
                    renderer.materials = materials;
                }
                else
                {
                    Plugin.Log.LogError($"Mask OverrideTexture for {customMask.MaskName} does not exist: {customMask.TexturePath}");
                }
            }
            
            __instance.SetPrivatePropertyValue("CurrentMask", clone);
            
            if (!justHead)
            {
                __instance.currentHeldMask = Object.Instantiate<GameObject>(clone, __instance.heldMaskParent);
            }
            
            return false;
        }
    }
}