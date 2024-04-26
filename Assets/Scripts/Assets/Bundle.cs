using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wxy.Res;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    /// <summary>
    /// AB包资源
    /// 1.记录他
    /// </summary>
    public class Bundle
    {
        private AssetBundleManifest _manifest;
        private AssetBundle _assetBundle;
        private string _filePath;
        private ResCore _resCore;
        public bool isLoaded;
        private int _ref;
        private Dictionary<string, Bundle> _depBundleDic = new(); //该bundle所依赖的其他bundle
        private bool isRoot;
        private Dictionary<string, Object> _resDic = new();

        internal Bundle(AssetBundleManifest manifest, string filePath, ResCore resCore)
        {
            this._manifest = manifest;
            this._filePath = filePath;
            this._resCore = resCore;
            LoadDepBundle();
        }

        private void Retain()
        {
            _ref++;
        }

        private void Release()
        {
            _ref--;
        }

        private void LoadDepBundle(bool isRoot = false)
        {
            this.isRoot = isRoot;
            var allDependencies = _manifest.GetAllDependencies(_filePath);//记载所有依赖资源
            foreach (var v in allDependencies)
            {
                //双方引用数+1
                Bundle bundle = _resCore.TyrGetBundle(v);
                if (bundle == null)
                {
                    bundle = new Bundle(_manifest, v, _resCore);
                    _depBundleDic.Add(v, bundle);
                }
                //两个bundle双方的引用数+1
                bundle.Retain();
                Retain();
            }

            _assetBundle = AssetBundle.LoadFromFile(_filePath);
        }

        /// <summary>
        /// 是不是能清理自己
        /// </summary>
        public bool TryClean(bool unloadAllRes = true)
        {
            if (_ref > 0)
            {
                return false;
            }
            _assetBundle.Unload(unloadAllRes);
            _assetBundle = null;
            foreach (var vBundle in _depBundleDic.Values)
            {
                vBundle.Release();
                vBundle.TryClean();
            }
            _depBundleDic.Clear();
            _depBundleDic = null;
            _resDic.Clear();
            _resDic = null;
            return true;
        }
        /// <summary>
        /// 直接销毁该bundle
        /// </summary>
        public void Dispose(bool unloadAllRes = true) {
            _ref = 0;
            TryClean(unloadAllRes);
        }

        public T TryLoadAsset<T>(string resName) where T : Object
        {
            if (_resDic.TryGetValue(resName, out var obj)) return (T)obj;
            Object assets = _assetBundle.LoadAsset<T>(resName);
            _resDic.Add(resName,assets);
            return (T)assets;
        }
        public Object TryLoadAsset(string resName,Type type)
        {
            if (_resDic.TryGetValue(resName, out var obj)) return obj;
            Object assets = _assetBundle.LoadAsset(resName,type);
            _resDic.Add(resName,assets);
            return _assetBundle.LoadAsset(resName,type);
        }
    }
}