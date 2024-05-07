using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Config;
using Newtonsoft.Json;
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
        
        //res->bundle
        private readonly Dictionary<string, Bundle> _assetDic = new();

        //filePath->bundle
        private readonly Dictionary<string, Bundle> _bundleDic = new();

        private ResMode _resMode ;

        public ResCore(ResMode resMode)
        {
            this._resMode = resMode;
            StringBuilder json = new StringBuilder();
            string vt;
            using (StreamReader reader = new StreamReader(ResCfg.MANIFEST_JSON_LOCAL))
            {
                while ((vt = reader.ReadLine()) != null)
                {
                    json.AppendLine(vt);
                }

                if (json.ToString().Length == 0 || json.ToString().Equals(string.Empty))
                {
                    Debug.LogError($"当前的json文件为空");
                    return;
                }
            }

            //读取配置文件
            Manifest manifestBundle = JsonConvert.DeserializeObject<Manifest>(json.ToString());

            foreach (var v in manifestBundle.bundleList)
            {
                var bundle = new Bundle(v, this);
                //按照资源路径和bundle进行关联
                foreach (var assetStr in v.assetStrs)
                {
                    _assetDic.Add(assetStr, bundle);
                }

                //按照ab包名字和bundle进行关联
                _bundleDic.Add(v.name, bundle);
            }

            //创建好Bundle以后对每个bundle的依赖进行构建
            foreach (var bundle in _bundleDic.Values)
            {
                var depBundleList = new List<Bundle>();
                foreach (var depFilePath in bundle.bundleCfg.depStrs)
                {
                    depBundleList.Add(_bundleDic[depFilePath]);
                }

                bundle.depBundleList = depBundleList;
            }
        }

        public Object LoadAsset(string objFilePath, Type type)
        {
            if (_resMode == ResMode.Local)
            {
                return AssetDatabase.LoadAssetAtPath(objFilePath,type);
            }
            var bundle = GetBundleWithObjFilePath(objFilePath);
            TryLoadBundle(bundle);
            return bundle.LoadAsset(objFilePath, type);
        }

        private Bundle GetBundleWithObjFilePath(string objFilePath) =>
            _assetDic.TryGetValue(objFilePath, out var bundle) ? bundle : null;

        private Bundle GetBundleWithBundleName(string bundleName) =>
            _bundleDic.TryGetValue(bundleName, out var bundle) ? bundle : null;

        private void TryLoadBundle(Bundle bundle)
        {
            if (bundle == null) return;
            bundle.Retain();
            if (bundle.isLoaded&& !bundle.isRoot)
            {
                bundle.isRoot = true;
                foreach (var depBundle in bundle.depBundleList)
                {
                    depBundle.Retain();
                }
            }
            else if (!bundle.isLoaded)
            {
                LoadBundle(bundle);
            }
        }

        private void LoadBundle(Bundle bundle)
        {
            if (bundle.isLoaded) return;
            bundle.LoadBundle(true);
            for (var i = 0; i < bundle.depBundleList.Count; i++)
            {
                var depBundle = bundle.depBundleList[i];
                depBundle.Retain();
                if (!depBundle.isLoaded) depBundle.LoadBundle();
            }
        }

        /// <summary>
        /// 通过资源释放来减少ab包的计数
        /// </summary>
        /// <param name="objFilePath"></param>
        public void UnLoadAsset(string objFilePath)
        {
            if (_resMode == ResMode.Local)
            {
                Debug.LogWarning("该模式是本地加载不支持资源卸载回收--UnloadAsset");
                return;
            }
            var bundle = GetBundleWithObjFilePath(objFilePath);
            bundle?.Release();
        }

        /// <summary>
        /// 资源回收,当对应ab包的引用为0的时候才会进行资源ab包的卸载
        /// </summary>
        /// <param name="maxUseSecond">一次gc的最大消耗时间</param>
        /// <param name="unloadAllLoadedObjects">是否卸载ab包对应的物体</param>
        public void GC(double maxUseSecond = 10, bool unloadAllLoadedObjects = true)
        {
            if (_resMode == ResMode.Local)
            {
                Debug.LogWarning("该模式是本地加载不支持资源回收--GC");
                return;
            }
            var startTime = Time.realtimeSinceStartupAsDouble;
            foreach (var bundle in _bundleDic.Values)
            {
                if (bundle.isLoaded && bundle.TryClean(unloadAllLoadedObjects))
                {
#if UNITY_EDITOR
                Debug.Log($"{bundle.bundleCfg.name}"+"被垃圾回收");    
#endif
                    var depBundleList = bundle.depBundleList;
                    for (var i = 0; i < depBundleList.Count; i++)
                    {
                        depBundleList[i].Release();
                        // depBundleList[i].TryClean();
                    }
                }

                if (Time.realtimeSinceStartupAsDouble - startTime > maxUseSecond) return;
            }
        }

#if UNITY_EDITOR
        public void ShowInfo()
        {
            foreach (var bundle in _bundleDic.Values)
            {
                if (bundle.isLoaded && bundle.Ref > 0)
                {
                    Debug.Log($"{bundle.bundleCfg.name}");
                }
            }
        }
#endif
    }
}