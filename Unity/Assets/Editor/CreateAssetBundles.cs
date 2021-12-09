using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles : MonoBehaviour
{
    [MenuItem("Assets/Bundle AssetBundles")]
    static void BuildAllAssetBundles(){
        string assetBundleDirectory = "Assets/StreamingAssets";
        if(!Directory.Exists(Application.streamingAssetsPath)){
            Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);

    }
}
