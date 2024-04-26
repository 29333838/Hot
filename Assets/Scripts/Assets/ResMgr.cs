using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    /// <summary>
    /// 资源加载模块
    /// 调试模式本地bundle
    /// 发布模式服务器bundle
    ///
    /// 1.设计打包策略
    /// 2.做包引用计数
    /// 3.管理包加载和卸载
    /// 4.需要暂存加载好的游戏物体
    /// </summary>
    public class ResMgr
    {
        private ResCore _resCore;
        public static string PathURL => ResCore.PathURL;
        public ResMgr()
        {
            _resCore = new ResCore();
        }
        public T LoadAssets<T>(string bundleName, string resName)  where T : Object => _resCore.LoadAsset<T>(bundleName, resName);
        public Object LoadAssets(string bundleName, string resName, Type type) => _resCore.LoadAsset(bundleName, resName, type);
        public void DisposeAllBundle(bool unloadAllRes = true)
        {
            _resCore.DisposeAllBundle(unloadAllRes);
        }
    }
}