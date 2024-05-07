using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wxy.Res
{
    /// <summary>
    /// 资源组
    /// </summary>
    public class ResGroup
    {
        private readonly string _name;
        private readonly ResMgr _resMgr;
        private readonly List<string> _groupList = new();
        //存储
        private HashSet<Object> unityObjSet = new();
        
        public ResGroup(string name,ResMgr _resMgr)
        {
            this._name = name;
            this._resMgr = _resMgr;
        }

        public Object LoadAsset(string path,Type type)
        {
            if (_resMgr == null) throw new Exception("resMgr is null");
            return _resMgr.LoadAsset(path, type);
        }
        
        // public GameObject LoadPrefab(string path)
        // {
        //     GameObject obj = LoadAsset(path, typeof(GameObject)) as GameObject;
        //     return obj;
        // }
        
    }
}