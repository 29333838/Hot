using UnityEngine;

namespace Wxy.InputAction
{
    /// <summary>
    /// 跳跃
    /// </summary>
    public class JumpCommand : Command
    {
        protected override void Execute(GameObject t)
        {
            Debug.Log("进行跳跃");
        }
    }
}