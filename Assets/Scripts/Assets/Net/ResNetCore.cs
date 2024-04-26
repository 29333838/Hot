using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Wxy.Res.Net
{
    /// <summary>
    /// 1.从服务器更新资源到本地
    /// 2.提供断点续传
    /// 3.提供实时下载
    /// </summary>
    public class ResNetCore
    {
        public static void UpdateABResFromCloud(string uri)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri,0);
        }
        
        IEnumerator Download(string url)
        {
            //ab包的地址
            AssetBundle ab ;
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                // yield return request.SendWebRequest();
                request.SendWebRequest();
                while (!request.isDone)
                {
                    Debug.Log(request.downloadProgress);
                    // textProgress.text = (request.downloadProgress * 100).ToString("F0") + "%";
                    yield return null;
                }
                if (request.isDone)
                {
                    Debug.Log(100);
                    //textProgress.text = 100 + "%";
                }
                ab = (request.downloadHandler as DownloadHandlerAssetBundle)?.assetBundle;
                if (ab == null)
                {
                    Debug.LogError("AB包为空");
                    yield break;
                }
            }
            //ab.Unload(false); 
            Debug.Log("成功");
        }
    }
}