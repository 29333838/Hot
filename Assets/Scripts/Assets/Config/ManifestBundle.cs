using System.Collections.Generic;
using UnityEngine;

namespace Assets.Config
{
    /// <summary>
    /// Json
    /// </summary>
    public class Manifest : ScriptableObject
    {
        public string version;
        public List<ManifestBundle> bundleList = new();
    }

    public struct ManifestBundle
    {
        public string name;
        public string md5Num;
        public long size;
        public string subFilePath;
        public string[] assetStrs;
        public string[] depStrs;
    }
}