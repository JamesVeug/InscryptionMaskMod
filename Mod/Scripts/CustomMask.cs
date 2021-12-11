using System.Collections.Generic;
using System.IO;
using DiskCardGame;

namespace MaskMod
{
    public class CustomMask
    {
        public static List<CustomMask> customMasks = new List<CustomMask>();
        public static List<CustomMask> randomMaskPool = new List<CustomMask>();
        
        public LeshyAnimationController.Mask Mask;
        public string MaskName;
        public string ModelPath;
        public string TexturePath;
        public string PrefabName;

        public static CustomMask AddCustomMask(LeshyAnimationController.Mask mask, string name, string modelPath, string prefabName, string textureOverride=null)
        {
            CustomMask m = new CustomMask()
            {
                MaskName = name,
                ModelPath = Path.Combine(Plugin.PluginDirectory, modelPath),
                TexturePath = textureOverride != null ? Path.Combine(Plugin.PluginDirectory, textureOverride) : null,
                PrefabName = prefabName,
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