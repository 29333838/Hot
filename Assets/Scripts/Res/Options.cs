using UnityEngine;

namespace Wxy.Res
{
    public class Options
    {
        public static AssetBundle LoadBundle(string filePath)
        {
            return AssetBundle.LoadFromFile(filePath);
        }
    }
}