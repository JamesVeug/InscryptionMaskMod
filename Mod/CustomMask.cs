using System.Collections.Generic;
using System.IO;
using DiskCardGame;

namespace MaskMod
{
    public class CustomMask
    {
        public static List<CustomMask> customMasks = new List<CustomMask>();
        
        public LeshyAnimationController.Mask Mask;
        public string MaskName;
        public string ModelPath;
        public string MaterialPath;
        public string TexturePath;

        public static CustomMask AddCustomMask(string name, string modelPath, string materialPath, string texturePath, LeshyAnimationController.Mask mask)
        {
            CustomMask m = new CustomMask()
            {
                MaskName = name,
                ModelPath = Path.Combine(Plugin.PluginDirectory, modelPath),
                MaterialPath = Path.Combine(Plugin.PluginDirectory, materialPath),
                TexturePath = Path.Combine(Plugin.PluginDirectory, texturePath),
                Mask = mask
            };
            customMasks.Add(m);
            Plugin.Log.LogInfo("Added CustomMask " + name);
            Plugin.Log.LogInfo("\tModel path: " + m.ModelPath);
            Plugin.Log.LogInfo("\tMaterial path: " + m.MaterialPath);
            Plugin.Log.LogInfo("\tTexture path: " + m.TexturePath);
            return m;
        }
    }
}