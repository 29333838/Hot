using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    /// <summary>
    /// 发布模式下加载AB
    /// </summary>
    public class ResCore
    {
        //子类沙盒：全局bundle 在bundle加载的时候会优先寻找依赖包
        private Dictionary<string, Bundle> _bundleDic = new();
        private AssetBundleManifest _mainfest = null;
        private AssetBundle _mainBundle;
        
        public ResCore()
        {
            _mainBundle = AssetBundle.LoadFromFile(PathURL + MainName); //主包地址
            _mainfest = _mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        private string MainName
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

        public static string PathURL => Application.persistentDataPath + "/";
        
        public Bundle TyrGetBundle(string path)
        {
            if (_bundleDic.TryGetValue(path, out var bundle)) return bundle;
            return null;
        }
        
        public Bundle LoadBundle(string path)
        {
            //是否存在这个bundle
            if (_bundleDic.TryGetValue(path, out Bundle tbundle))
            {
                return tbundle;
            }
            //生成bundle
            var bundle = new Bundle(_mainfest,path,this);
            _bundleDic.Add(path,bundle);
            return bundle;
        }

        public T LoadAsset<T>(string bundlePath, string resName) where T : Object
        {
            Bundle bundle = LoadBundle(bundlePath);
            return bundle.TryLoadAsset<T>(resName);
        }
        public Object LoadAsset(string bundlePath, string resName,Type type)
        {
            Bundle bundle = LoadBundle(bundlePath);
            return bundle.TryLoadAsset(resName,type);
        }
        public void DisposeAllBundle(bool unloadAllRes)
        {
            foreach (var bundle in _bundleDic.Values)
            {
                bundle.Dispose(unloadAllRes);
            }
            _bundleDic.Clear();
            _mainBundle.Unload(true);
        }
        
    }
}