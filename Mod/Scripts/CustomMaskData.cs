using System;
using System.IO;
using BepInEx;
using DiskCardGame;
using UnityEngine;

namespace MaskMod
{
    [Serializable]
    public class CustomMaskData
    {
        public string MaskType; // Enum type (Prospector/Woodcarver/Angler/Trapper/Trader/Doctor)
        public string MaskName; // Name that the user wants to call this mask
        public string TextureOverridePath; // Optional Texture that we want to override the model with
        public string BundlePath; // What Model should this mask use?
        public string PrefabName; // Name of the GameObject within the bundle that the mask will use

        public string MaskFilePath;

        public static CustomMaskData FromJson(string filePath)
        {
            CustomMaskData customMaskData = JsonUtility.FromJson<CustomMaskData>(File.ReadAllText(filePath));
            customMaskData.MaskFilePath = filePath;
            return customMaskData;
        }
        
        public void Load()
        {
            // Optional
            string fullTexturePath = "";
            if (!string.IsNullOrEmpty(TextureOverridePath))
            {
                fullTexturePath = GetFullPath(TextureOverridePath);
            }

            // Get type of mask in case we want to filter it for any reason
            LeshyAnimationController.Mask mask = (LeshyAnimationController.Mask)Enum.Parse(typeof(LeshyAnimationController.Mask), MaskType);
            
            // Add to the list
            CustomMask.AddCustomMask(mask,
                MaskName, 
                GetFullPath(BundlePath), 
                PrefabName,
                fullTexturePath
            );
        }

        private string GetFullPath(string path)
        {
            if (GetFullModPath(path, out string modPath))
            {
                return modPath;
            }
            
            // The mod did not provide the file. See if we have it instead
            if (GetFullLocalPath(path, out string localPath))
            {
                return localPath;
            }
            
            Plugin.Log.LogError("Couldn't file at path: " + modPath);
            return modPath;
        }

        private bool GetFullLocalPath(string path, out string fullPath)
        {
            fullPath = Path.Combine(Plugin.PluginDirectory, path);
            return File.Exists(fullPath);
        }
        
        private bool GetFullModPath(string path, out string fullPath)
        {
            //
            // Overly complicated but it works and doesn't break atm. So that's fine
            //
            
            // file:      C:\...\BepInEx\plugins\MaskMod\Masks\screammask.custommask
            // plugin:    C:\...\BepInEx\plugins\
            // modFolder: MaskMod
            // directory: MaskMod\Masks\screammask.custommask
            // fullPath:  C:\...\BepInEx\plugins\MaskMod\Masks\screammask.png
            
            // file directory
            // plugin directory
            // mod directory = file directory - plugin directory
            // texture path = mod directory + TextureOverridePath
            string pluginPath = Paths.PluginPath + "\\";
            string modDirectory = MaskFilePath.Substring(pluginPath.Length);
            string modFolder = modDirectory.Substring(0, modDirectory.IndexOf('\\', 1));
            fullPath = Path.Combine(pluginPath, modFolder, path);
            return File.Exists(fullPath);
        }
    }
}
