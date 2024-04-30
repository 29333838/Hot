using UnityEngine;

namespace Wxy.Res.Config
{
    public static class ResCfg
    {
        //-----Unity本地地址
        public static string AB_RES_PATH_Local = Application.persistentDataPath + "/";//本地根路径
        public static string AB_RES_PATH_Local2 = Application.streamingAssetsPath + "/";//本地根路径
        
        public static string ABCMP_PATH = Application.dataPath + "/Scripts/Assets/Config/";
        public static string ABCMP_NAME = "ABCmp.txt";
        public static string LOGO_PATH = Application.dataPath + "/Scripts/Assets/Config/Logo.txt";
        

        //----Unity服务器地址
        public static string AB_RES_PATH_SERVER = "D:/http_server/";//服务端根路径
        public static string SERVER_URL = "http://10.85.35.36:8091/http_server/";
        public static string LOGO_URL_SERVER = SERVER_URL + "Logo.txt";
        public static string ABCMP_URL_SERVER = SERVER_URL + ABCMP_NAME;
    }

    public struct ResJson
    {
        public string fileName;
        public string md5Num;
        public long fileLength;
        public string subFilePath;//完整路径
        public string reFilePath;//相对路径

        public ResJson(string fileName, string md5Num, long fileLength, string subFilePath, string reFilePath)
        {
            this.fileName = fileName;
            this.md5Num = md5Num;
            this.fileLength = fileLength;
            this.subFilePath = subFilePath;
            this.reFilePath = reFilePath;
        }
    }
}