using UnityEngine;

namespace Wxy.Res.Config
{
    [CreateAssetMenu(menuName = "Wxy/ResLoadMode", fileName = "Assets/Scripts/Res/ResLoadMode")]
    public class ResLoadMode : ScriptableObject
    {
        public ResMode resMode = ResMode.Local;
    }
}