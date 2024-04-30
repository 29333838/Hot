using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using Wxy.Res.Config;
using Wxy.Res.Net;
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
    /// 5.由于是通过引用计数来计算依赖，那么可能存在循环依赖的问题，需要注意
    /// </summary>
    public class ResMgr : MonoBehaviour
    {
        private ResCore _resCore;
        private ResNetCore _resNetCore;
        public void Awake()
        {
            _resCore = new ResCore();
            _resNetCore = new ResNetCore();
        }

        // public T LoadAssets<T>(string bundleName, string resName) where T : Object =>
        //     _resCore.LoadAsset<T>(bundleName, resName);
        //
        // public Object LoadAssets(string bundleName, string resName, Type type) =>
        //     _resCore.LoadAsset(bundleName, resName, type);
        //
        // public void DisposeAllBundle(bool unloadAllRes = true) => _resCore.DisposeAllBundle(unloadAllRes);

        public void DownloadFormServer(string filePath, UnityAction<string> callBack = null)
        {
            StartCoroutine(_resNetCore.DownloadAsync(filePath, (fileContent) => { callBack?.Invoke(fileContent); }));
        }

        /// <summary>
        /// 尝试更新本地AB包资源
        /// </summary>
        public void TryUpdateLocalABFromServer()
        {
            DownloadFormServer(ResCfg.LOGO_URL_SERVER, (file) =>
            {
                //加载本地Logo文件
                using (StreamReader sr = new StreamReader(ResCfg.LOGO_PATH))
                {
                    if (file.Equals(sr.ReadLine()))
                    {
                        return;
                    }
#if UNITY_EDITOR

                    Debug.Log("<color=green>资源开始更新</color>");
#endif
                    //保存日志文件
                    using (StreamWriter sw = new StreamWriter(ResCfg.LOGO_PATH))
                    {
                        sw.Write(file);
                    }
                }

                //下载比对文件 得到需要更新的文件
                Dictionary<string, ResJson> resDic = new();
                DownloadFormServer(ResCfg.ABCMP_URL_SERVER, (file) =>
                {
                    //得到最终在本地需要保留的文件
                    Dictionary<string, ResJson> dic = new();
                    //得到服务器文件
                    Dictionary<string, ResJson> serverDic =
                        JsonConvert.DeserializeObject<Dictionary<string, ResJson>>(file);
                    //读写本地比对文件并比较
                    using (StreamReader sr = new StreamReader(ResCfg.ABCMP_PATH))
                    {
                        var localDic = JsonConvert.DeserializeObject<Dictionary<string, ResJson>>(sr.ReadLine());
                        var resDic = CmpAndDelete(serverDic, localDic);
                        //从服务器开始下载ab包并写入到对应地址去
                        foreach (var child in resDic)
                        {
                            //多协程下载ab包*******************
                            StartCoroutine(_resNetCore.DownloadABAsync(child.Value.reFilePath, child.Value.subFilePath, (t) =>
                            {
#if UNITY_EDITOR
                                Debug.Log($"<color=green>{child.Value.reFilePath+"下载完成"}</color>");
#endif
                            }));
                        }
                    }

                    //保存比较文件到本地
                    using (StreamWriter sw = new StreamWriter(ResCfg.ABCMP_PATH))
                    {
                        sw.Write(file);
                    }
                });
            });
        }
        private Dictionary<string, ResJson> CmpAndDelete(Dictionary<string, ResJson> serverDic,
            Dictionary<string, ResJson> localDic)
        {
            Dictionary<string, ResJson> resDic = new();
            foreach (var key in serverDic.Keys)
            {
                if (!localDic.ContainsKey(key))
                {
                    resDic.Add(serverDic[key].fileName, serverDic[key]);
                }
            }

            foreach (var key in localDic.Keys)
            {
                if (!resDic.ContainsKey(key))
                {
                    //需要删除本地的ab包
                    File.Delete(resDic[key].subFilePath);
                }
                //不处理md5一样的ab包
                else if (serverDic[key].md5Num == resDic[key].md5Num)
                {
                    resDic.Remove(key);
                }
            }

            return resDic;
        }
    }
}