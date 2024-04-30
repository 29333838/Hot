using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Wxy.Res.Config;

namespace Wxy.Res.Net
{
    /// <summary>
    /// 1.从服务器更新资源到本地
    /// 2.提供断点续传
    /// 3.提供实时下载
    /// </summary>
    public class ResNetCore
    {
        /// <summary>
        /// 从服务器下载普通资源
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="callBacks">string 是从服务端得到的数据</param>
        /// <returns></returns>
        public IEnumerator DownloadAsync(string fileUrl, UnityAction<string> callBacks = null,
            [CallerMemberName] string memberName = "")
        {
            string res = string.Empty;
            UnityWebRequest www = UnityWebRequest.Get(fileUrl);
            www.timeout = 10;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("服务器错误" + www.result);
                yield break;
            }
#if UNITY_EDITOR
            Debug.Log($"<color=green>{memberName + "调用下载接口"}</color>");
#endif
            callBacks?.Invoke(www.downloadHandler.text);
        }

        /// <summary>
        /// 从服务器下载AB包
        /// </summary>
        /// <param name="abUrl"></param>
        /// <param name="targetPath"></param>
        /// <param name="callBacks"></param>
        /// <returns></returns>
        public IEnumerator DownloadABAsync(string abUrl, string targetPath, UnityAction<string> callBacks)
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(abUrl);
            www.timeout = 10;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
#if UNITY_EDITOR
                Debug.LogError("服务器错误" + www.result);
#endif
                yield break;
            }

            byte[] abBytes = www.downloadHandler.data;
            //存储ab包到目的文件路径
            File.WriteAllBytes(targetPath, abBytes);
            callBacks?.Invoke(abUrl);
        }
    }
}