using UnityEngine;

namespace Wxy.Res.Config
{
    /// <summary>
    /// 运行时的配置文件
    /// </summary>
    public static class ResCfg
    {
        //-----打包好的json文件路径
        public static string MANIFEST_JSON_LOCAL = Application.dataPath +"/TestRes/manifest.json";
        
        //-----Unity本地地址
        public static string AB_RES_PATH_Local = Application.persistentDataPath + "/";//本地根路径
        public static string AB_RES_PATH_Local2 = Application.streamingAssetsPath + "/";//本地默认资源根路径
        public static string ABCMP_PATH = Application.dataPath + "/Scripts/Assets/Config/";
        
        //----Unity服务器地址
        public static string AB_RES_PATH_SERVER = "D:/http_server/";//服务端根路径
        public static string SERVER_URL = "http://10.85.35.36:8091/http_server/";
    }
}