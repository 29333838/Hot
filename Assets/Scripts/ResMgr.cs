// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using XLua;
// using Object = UnityEngine.Object;
//
// /// <summary>
// /// ab包热更新资源管理器
// /// </summary>
// [LuaCallCSharp]
// public class ResMgr : UnitySingleBase<ResMgr>
// {
//     public T GetRes<T>(string name) where T : Object
//     {
//         T res = null;
// #if UNITY_EDITOR
//         string path = "Assests/AssetsPackage/" + name;
//         res = AssetDatabase.LoadAssetAtPath<T>(path);
// #endif
//         return res;
//     }
//
//     public GameObject GetRes(string name, Type type)
//     {
//         GameObject res = null;
// #if UNITY_EDITOR
//         string path = "Assests/AssetsPackage/" + name;
//         res = (GameObject)AssetDatabase.LoadAssetAtPath(path, type);
// #endif
//
//         return res;
//     }
//
//     public GameObject Test(string filePath)
//     {
//         GameObject res = null;
// #if UNITY_EDITOR
//         string path = "Assets/AssetsPackage/" + filePath;
//         print(path);
//         res = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//         if (res == null)
//         {
//             return null;
//         }
//         print(res.name);
// #endif
//         return res;
//     }
// }