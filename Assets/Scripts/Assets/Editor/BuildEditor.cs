using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Assets.Config;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Wxy.Core;
using Wxy.Res.Config;
using static Wxy.Core.Utils;

namespace Wxy.Res.Editor
{
    public static class BuildEditor
    {
        private static string AB_BUILD_PATH_LOCAL = "Assets/ABRes/";
        private static string AB_PATH_LOCAL = "Assets/TestRes/";
        private static string OPTION_LOCAL = "Assets/BundleBuildSetting.asset";

        private static BuildAssetBundleOptions bbo = BuildAssetBundleOptions.ChunkBasedCompression |
                                                     BuildAssetBundleOptions
                                                         .AssetBundleStripUnityVersion |
                                                     BuildAssetBundleOptions.StrictMode |
                                                     BuildAssetBundleOptions.DisableLoadAssetByFileName |
                                                     BuildAssetBundleOptions
                                                         .DisableLoadAssetByFileNameWithExtension;

        private static string MAINFEST_NAME = "manifest.json";

        [MenuItem("Wfp/打包Bundle")]
        public static void BuildBundles()
        {
            //打好了AB包标识，等待生成ab包并通过ab依赖来得到最终的json文件
            var resDic = CreateABFileJson();
            string outputPath;
            BuildAssetBundleOptions options;
            BuildTarget targetPlatform;
            // var bundleBuildSetting = AssetDatabase.LoadAssetAtPath<BuildOptions>(OPTION_LOCAL);
            targetPlatform = EditorUserBuildSettings.activeBuildTarget;
            options = bbo;
            outputPath = AB_PATH_LOCAL;
            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, options, targetPlatform);
            var mainManifest = ScriptableObject.CreateInstance<Manifest>();
            mainManifest.version = SetVersionNumber();
            mainManifest.name = "wxy_bundle";
            ManifestBundle bundleRes;
            byte[] data;
            foreach (var name in assetBundleManifest.GetAllAssetBundles())
            {
                Debug.Log(name);
                bundleRes = new ManifestBundle();
                bundleRes.name = name;
                bundleRes.assetStrs = resDic[name].ToArray();
                bundleRes.md5Num = GetFileMD5(Path.Combine(outputPath, name));
                bundleRes.depStrs = assetBundleManifest.GetDirectDependencies(name);
                bundleRes.size = GetSize(resDic[name].ToArray());
                bundleRes.subFilePath = AB_PATH_LOCAL + name;
                mainManifest.bundleList.Add(bundleRes);
            }

            string path = outputPath + $"/{MAINFEST_NAME}";
            Debug.Log(path);
            using (StreamWriter sw = new StreamWriter(path))
            {
                var json = JsonConvert.SerializeObject(mainManifest);
                sw.Write(json);
                Debug.Log(json);
            }

            AssetDatabase.Refresh();
        }


        private static long GetSize(string[] fileStrs)
        {
            long size = 0;
            foreach (var v in fileStrs)
            {
                string file = v;
                if (!File.Exists(file))
                {
                    Debug.LogError($"不包含这个文件{file}");
                    return 0;
                }

                var fileInfo = new FileInfo(file);
                size += fileInfo.Length;
            }

            return size;
        }


        /// <summary>
        /// 设置ab包标识返回资源列表
        /// </summary>
        public static Dictionary<string, List<string>> CreateABFileJson()
        {
            string bundlePath = AB_BUILD_PATH_LOCAL;
            if (!Directory.Exists(bundlePath))
            {
                Debug.LogError("ab包资源路径不存在");
                return default;
            }

            Dictionary<string, List<string>> resDic = new();
            string[] filesName = Directory.GetFiles(bundlePath, "*", SearchOption.AllDirectories);
            foreach (var file in filesName)
            {
                if (Path.GetExtension(file).Equals(".meta")) continue;
                string file2 = file.Replace("\\", "/").Replace("//", "/");
                string name = SettingFlag(file2);
                Debug.Log(file2);
                var fileInfo = new FileInfo(file2);
                long fileSize = fileInfo.Length;
                if (!resDic.ContainsKey(name))
                {
                    resDic.Add(name, new List<string>());
                }

                resDic[name].Add(file2);
            }

            return resDic;
        }

        /// <summary>
        /// 设置ab包的标识名字
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private static string SettingFlag(string assetPath)
        {
            // 根据特定规则判断要给资源设置的 AssetBundle 名称
            string bundleName = GetNearestFolder(assetPath) ?? "DefaultBundle";
            // 设置资源的 AssetBundle 名称
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                importer.assetBundleName = bundleName;
            }

            AssetDatabase.Refresh();
            return bundleName.ToLower();
        }

        [MenuItem("Wfp/清除所有AB包标识移除无用AB包标识")]
        private static void ClearAssetBundleNames()
        {
            string directoryPath = AB_BUILD_PATH_LOCAL;
            // 获取指定路径下的所有文件
            string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                // 使用 AssetImporter 获取文件的 Asset Bundle 标识
                AssetImporter importer = AssetImporter.GetAtPath(file);
                if (importer != null && !string.IsNullOrEmpty(importer.assetBundleName))
                {
                    // 清除文件的 Asset Bundle 标识
                    importer.assetBundleName = null;
                }
            }

            // 获取所有 AssetBundle 名称
            string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            // 遍历每个 AssetBundle 名称
            foreach (string assetBundleName in allAssetBundleNames)
            {
                bool isUsed = false;
                // 获取包含该 AssetBundle 的所有文件路径
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                // 遍历每个文件路径，检查是否有文件使用该 AssetBundle
                foreach (string assetPath in assetPaths)
                {
                    // 使用 AssetImporter 获取文件的 Asset Bundle 标识
                    AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                    if (importer != null && importer.assetBundleName == assetBundleName)
                    {
                        isUsed = true;
                        break;
                    }
                }

                // 如果没有文件使用该 AssetBundle，则移除该标识
                if (!isUsed)
                {
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                }
            }

            Debug.Log("该文件下的所有AB包标识已经清除并移除无用的标识: " + directoryPath);
        }

        public static string SetVersionNumber()
        {
            return DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss");
        }
    }
}