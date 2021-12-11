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
            Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] Prefix " + mask);
            Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] Unity " + Application.unityVersion);
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
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(customMask.ModelPath);
                if (myLoadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                }
                
                Plugin.Log.LogInfo($"Loaded bundle!");
                GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(customMask.PrefabName);
                GameObject clone = GameObject.Instantiate(prefab, __instance.maskParent);
                Plugin.Log.LogInfo($"Prefab: " + clone);
                Plugin.Log.LogInfo($"Size: " + clone.transform.localScale);
                Plugin.Log.LogInfo($"Pos: " + clone.transform.localPosition);
                //Plugin.Print(clone);
                myLoadedAssetBundle.Unload(false);

                /*if (!string.IsNullOrEmpty(customMask.TexturePath))
                {
                    Plugin.Log.LogInfo($"Overriding texture: " + customMask.TexturePath);
                    if (File.Exists(customMask.TexturePath))
                    {
                        byte[] imgBytes = File.ReadAllBytes(customMask.TexturePath);
                        Texture2D texture = new Texture2D(2,2);
                        Plugin.Log.LogInfo($"Overriding texture: " + texture);
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
                }*/
                
                __instance.SetPrivatePropertyValue("CurrentMask", clone);
                if (!justHead)
                {
                    __instance.currentHeldMask = Object.Instantiate<GameObject>(prefab, __instance.heldMaskParent);
                }
                Plugin.Log.LogInfo("[LeshyAnimationController_SpawnMask] done");
                return false;
            }
            
            return true;
        }
    }
}