using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Wxy.Res;


public class GameLaunch : MonoBehaviour
{
    private ResMgr _resMgr;
    void Start()
    {
        _resMgr = new ResMgr();
        Instantiate(_resMgr.LoadAssets<GameObject>(ResMgr.PathURL+"test", "Test"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(_resMgr.LoadAssets<GameObject>(ResMgr.PathURL+"test", "Test"));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _resMgr.DisposeAllBundle(true);
        }
    }
}