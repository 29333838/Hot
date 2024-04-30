using System.Collections.Generic;
using UnityEngine;

namespace Assets.Config
{
    public class Manifest : ScriptableObject
    {
        public string version;
        public List<ManifestBundle> bundleList = new();
    }

    public class ManifestBundle
    {
        public string name;
        public string md5Num;
        public string[] assetStrs;
        public string[] depStrs;
        public int size;
    }
}