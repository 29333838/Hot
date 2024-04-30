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
       // _resMgr.TryUpdateLocalABFromServer();
    }   
}