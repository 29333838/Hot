using UnityEditor;
using UnityEngine;

namespace Wxy.Res.Editor
{
    [CreateAssetMenu(fileName = "BundleBuildSetting", menuName = "生成BundleBuildSetting")]
    public class BuildOptions : ScriptableObject
    {
        public string outputPath = "Assets/TestRes";
    }
}