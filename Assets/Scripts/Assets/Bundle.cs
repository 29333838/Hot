using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Config;
using JetBrains.Annotations;
using UnityEngine;
using Wxy.Res;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    /// <summary>
    /// AB包资源
    /// 1.记录他
    /// 2.字典的key是相对路径
    /// </summary>
    public class Bundle
    {
        private AssetBundle _assetBundle;
        public List<Bundle> depBundleList = new();
        private Dictionary<string, Object> _resCacheDic = new();
        private ResCore _resCore;
        public bool isRoot;
        private int _ref;
        public bool isLoaded => _assetBundle != null;
        public ManifestBundle bundleCfg;
        public int Ref => _ref;
        public Bundle( ManifestBundle bundleCfg, ResCore resCore)
        {
            this.bundleCfg = bundleCfg;
            this._resCore = resCore;
        }

        internal void Retain()
        {
            _ref++;
        }

        internal void Release()
        {
            --_ref;
            if (this.isRoot)
            {
                int count = 0;
                foreach (var dp in depBundleList)
                {
                    if (dp.isRoot && dp.isLoaded && dp.depBundleList.Contains(this))
                    {
                        count++;
                    }
                }
                if (_ref == count)
                {
                    isRoot = false;
                    _ref -= count;
                }
            }
        }

        private void Check()
        {
            if (!isLoaded) throw new Exception("该bundle没有被加载");
        }

        internal void LoadBundle(bool isRoot = false)
        {
            this.isRoot = isRoot;
            _assetBundle ??= Options.LoadBundle(bundleCfg.subFilePath);
        }

        public Object LoadAsset(string objFilePath, Type type)
        {
            Check();
            if (_resCacheDic.ContainsKey(objFilePath))
            {
                return _resCacheDic[objFilePath];
            }
            Object obj = _assetBundle?.LoadAsset(objFilePath, type);
            if (obj == null)
            {
#if UNITY_EDITOR
                Debug.LogError("加载的物体为空");
#endif
                return null;
            }

            _resCacheDic.Add(objFilePath, obj);
            return obj;
        }

        public T LoadAsset<T>(string objName) where T : Object
        {
            Check();
            return _assetBundle?.LoadAsset<T>(objName);
        }

        public bool TryClean(bool unloadAllLoadedObjects = true)
        {
            if (_ref > 0) return false;
            //卸载全部资源
            _assetBundle.Unload(unloadAllLoadedObjects);
            _assetBundle = null;
            _resCacheDic.Clear();
            _ref = 0;
            return true;
        }
    }
}