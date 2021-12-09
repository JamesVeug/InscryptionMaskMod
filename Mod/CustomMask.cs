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
        public string TexturePath;

        public static CustomMask AddCustomMask(LeshyAnimationController.Mask mask, string name, string modelPath, string textureOverride=null)
        {
            CustomMask m = new CustomMask()
            {
                MaskName = name,
                ModelPath = Path.Combine(Plugin.PluginDirectory, modelPath),
                TexturePath = Path.Combine(Plugin.PluginDirectory, textureOverride),
                Mask = mask
            };
            customMasks.Add(m);
            Plugin.Log.LogInfo("Added CustomMask " + m.MaskName);
            Plugin.Log.LogInfo("\tModel path: " + m.ModelPath);
            Plugin.Log.LogInfo("\tTexture path: " + m.TexturePath);
            return m;
        }
    }
}