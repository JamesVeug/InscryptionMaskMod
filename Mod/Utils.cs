using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MaskMod
{
    public static class Utils
    {
        public static Texture2D GetTextureFromPath(string path)
        {
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.PluginDirectory, path));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            return tex;
        }
        
        
        /// <summary>
        /// Returns a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivatePropertyValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            PropertyInfo pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Returns a private Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivateFieldValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)fi.GetValue(obj);
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
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }

        /// <summary>
        /// Set a private Property Value on a given Object. Uses Reflection.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">the value to set</param>
        /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
        public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            fi.SetValue(obj, val);
        }
        
        public static void ToFadeMode(this Material material)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
        }
    }
    
    public static class StandardShaderUtils
     {
         public enum BlendMode
         {
             Opaque,
             Cutout,
             Fade,
             Transparent
         }
     
         public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
         {
             switch (blendMode)
             {
                 case BlendMode.Opaque:
                     standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                     standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                     standardShaderMaterial.SetInt("_ZWrite", 1);
                     standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                     standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                     standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                     standardShaderMaterial.renderQueue = -1;
                     break;
                 case BlendMode.Cutout:
                     standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                     standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                     standardShaderMaterial.SetInt("_ZWrite", 1);
                     standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                     standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                     standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                     standardShaderMaterial.renderQueue = 2450;
                     break;
                 case BlendMode.Fade:
                     standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                     standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                     standardShaderMaterial.SetInt("_ZWrite", 0);
                     standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                     standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                     standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                     standardShaderMaterial.renderQueue = 3000;
                     break;
                 case BlendMode.Transparent:
                     standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                     standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                     standardShaderMaterial.SetInt("_ZWrite", 0);
                     standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                     standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                     standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                     standardShaderMaterial.renderQueue = 3000;
                     break;
             }
     
         }
     }
}