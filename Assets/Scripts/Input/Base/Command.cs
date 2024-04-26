using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wxy.InputAction
{
    /// <summary>
    /// 输入行为基类，约定一系列的方法行为
    /// </summary>
    public  class Command
    {
        protected virtual void Execute(GameObject t)
        {
        
        }

        protected virtual T Execute<T, U>(U u)
        {
            return default;
        }
    }
}
