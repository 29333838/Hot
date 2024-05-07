using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using Wxy.Res.Config;
using Wxy.Res.Net;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    public enum ResMode
    {
        Local,
        Bundle
    }


    /// <summary>
    /// 资源加载模块
    /// 调试模式本地bundle
    /// 发布模式服务器bundle
    ///
    /// 1.设计打包策略 ok
    /// 2.做包引用计数 ok
    /// 3.管理包加载和卸载 
    /// 4.需要暂存加载好的游戏物体
    /// 5.由于是通过引用计数来计算依赖，那么可能存在循环依赖的问题，需要注意
    /// </summary>
    public class ResMgr : MonoBehaviour
    {
        private ResCore _resCore;
        private ResNetCore _resNetCore;
        [SerializeField] private ResMode _resMode = ResMode.Local;

        public void Awake()
        {
            _resCore = new ResCore(_resMode);
            // _resNetCore = new ResNetCore();
        }

        public Object LoadAsset(string objFilePath, Type type) => _resCore.LoadAsset(objFilePath, type);
        public T LoadAsset<T>(string objFilePath) where T : Object => (T)_resCore.LoadAsset(objFilePath, typeof(T));
        public void UnLoadAsset(string objFilePath) => _resCore.UnLoadAsset(objFilePath);

        public void GC(double maxUseSecond = 10, bool unloadAllLoadedObjects = true) =>
            _resCore.GC(maxUseSecond, unloadAllLoadedObjects);

        public void ShowInfo() => _resCore.ShowInfo();
    }
}