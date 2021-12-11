using System.Collections.Generic;
using System.IO;
using DiskCardGame;

namespace MaskMod
{
    public class CustomMask
    {
        public static List<CustomMask> customMasks = new List<CustomMask>();
        public static List<CustomMask> randomMaskPool = new List<CustomMask>();
        public static List<string> maskNames = new List<string>();
        
        public LeshyAnimationController.Mask Mask;
        public string MaskName;
        public string TextureOverridePath;
        public string BundlePath;
        public string BundlePrefabName;

        public static CustomMask AddCustomMask(LeshyAnimationController.Mask mask, string maskName, string bundlePath, string prefabName, string textureOverride=null)
        {
            if (maskNames.Contains(maskName))
            {
                Plugin.Log.LogWarning($"Custom Mask with name '{maskName}' already exists!");
            }
            else
            {
                maskNames.Add(maskName);
            }
            
            CustomMask m = new CustomMask()
            {
                MaskName = maskName,
                BundlePath = Path.Combine(Plugin.PluginDirectory, bundlePath),
                TextureOverridePath = textureOverride != null ? Path.Combine(Plugin.PluginDirectory, textureOverride) : null,
                BundlePrefabName = prefabName,
                Mask = mask
            };
            customMasks.Add(m);
            Plugin.Log.LogInfo("Added CustomMask " + m.MaskName);
            return m;
        }

        public static CustomMask GetRandomMask()
        {
            if (randomMaskPool.Count == 0)
            {
                randomMaskPool.AddRange(customMasks);
                randomMaskPool.Randomize();
            }

            if (randomMaskPool.Count == 0)
            {
                Plugin.Log.LogWarning("No masks found!");
                return null;
            }

            int index = randomMaskPool.Count - 1;
            CustomMask customMask = randomMaskPool[index];
            randomMaskPool.RemoveAt(index);
            return customMask;
        }
    }
}