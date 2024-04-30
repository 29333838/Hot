using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wxy.Res.Config;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    /// <summary>
    /// 发布模式下加载AB
    /// 在此模块下需要确保包存在，所以优先使用默认资源
    /// </summary>
    public class ResCore
    {
        
        private Dictionary<string, Bundle> _bundleDic = new();
        private AssetBundleManifest _mainfest = null;
        private AssetBundle _mainBundle;
        
        public ResCore()
        {
            _mainBundle = AssetBundle.LoadFromFile(  MainName); //主包地址
            _mainfest = _mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        private static string MainName
        {
            get
            {
#if UNITY_IOS
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#else
                return "PC";
#endif
            }
        }
        
        
       

//         private AssetBundle Init()
//         {
//             //Application.persistentDataPath+"/"
//             _mainBundle = AssetBundle.LoadFromFile(ResCfg.AB_RES_PATH_Local + MainName)?? AssetBundle.LoadFromFile(ResCfg.AB_RES_PATH_Local2 + MainName);
//             if (_mainBundle == null)
//             {
// #if UNITY_EDITOR
//                 _mainBundle = AssetDatabase.LoadAssetAtPath<>()
// #endif
//                 throw new Exception("obj is not find");
//             }
//         }
      
        

        // public T LoadAsset<T>(string bundlePath, string resName) where T : Object
        // {
        //     Bundle bundle = LoadBundle(bundlePath);
        //     return bundle.TryLoadAsset<T>(resName);
        // }
        // public Object LoadAsset(string bundlePath, string resName,Type type)
        // {
        //     Bundle bundle = LoadBundle(bundlePath);
        //     return bundle.TryLoadAsset(resName,type);
        // }
        // public void DisposeAllBundle(bool unloadAllRes)
        // {
        //     foreach (var bundle in _bundleDic.Values)
        //     {
        //         bundle.Dispose(unloadAllRes);
        //     }
        //     _bundleDic.Clear();
        //     _mainBundle.Unload(true);
        // }
    }
}