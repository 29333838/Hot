using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using Utils = Wxy.Utils.Utils;

/// <summary>
/// 启动lua的虚拟机
/// 没有做方法缓存
/// </summary>
[CSharpCallLua]
public class xLuaMgr : UnitySingleBase<xLuaMgr>
{
    private static string MAIN_NAME = "Main";
    private static string LUA_SCRIPT_PATH = Application.dataPath + "/lua/";
    private static string AB_PATH = Application.dataPath + "/AssetsPackage/";
    private LuaEnv _luaEnv;

    public void Begin()
    {
        InitLuaEnv();
        string t = $"require('{MAIN_NAME}')";
        _luaEnv.DoString(t);
        DoLuaOverMethod("main.Start()");
    }

    private void InitLuaEnv()
    {
        if (_luaEnv != null) return;
        _luaEnv = new LuaEnv();
        _luaEnv.AddLoader(ReLoadLuaFilePath);
    }

    public void DoLuaScript(string name)
    {
        InitLuaEnv();
        _luaEnv.DoString(name);
    }

    public void DoString(string content)
    {
        InitLuaEnv();
        _luaEnv.DoString(content);
    }
    private void DoLuaOverMethod(string methodName)
    {
        LuaFunction luaFunction = _luaEnv.Global.Get<LuaFunction>(methodName);
        if (luaFunction == null) return;
        luaFunction.Call();
    }

    private byte[] ReLoadLuaFilePath(ref string filePath)
    {
        string t = LUA_SCRIPT_PATH + filePath + ".lua";
        string t2 = Utils.ReplaceDotWithSlashExceptLast(t);
        if (!File.Exists(t2))
        {
            Debug.LogWarning(t2 + "<<重定向失败");
            return null;
        }
        return File.ReadAllBytes(t2);
    }

    private void Update()
    {
        DoLuaOverMethod("main.Update()");
    }
}