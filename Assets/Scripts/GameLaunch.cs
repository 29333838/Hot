using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Wxy.Res;
using Wxy.Res.Config;


public class GameLaunch : MonoBehaviour
{
    private ResMgr _resMgr;

    void Start()
    {
        _resMgr = this.AddComponent<ResMgr>();
        // Instantiate(_resMgr.LoadAsset<GameObject>("Assets/ABRes/Prefab/Test.prefab"));
        // _resMgr.TryUpdateLocalABFromServer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _resMgr.UnLoadAsset(("Assets/ABRes/Prefab/Test.prefab"));
            // _resMgr.ShowInfo();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _resMgr.GC();
            // _resMgr.ShowInfo();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Instantiate(_resMgr.LoadAsset<GameObject>(("Assets/ABRes/Prefab/Test.prefab")));
            // _resMgr.ShowInfo();
        }
    }
}