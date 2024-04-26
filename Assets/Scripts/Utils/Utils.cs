using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Wxy.Utils
{
    public class Utils
    {
        /// <summary>
        /// 替换所有.为/除了最后一个
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceDotWithSlashExceptLast(string input)
        {
            // 找到最后一个点的索引
            int lastIndex = input.LastIndexOf('.');

            // 如果未找到点，则直接返回原始字符串
            if (lastIndex == -1)
            {
                return input;
            }

            // 使用 StringBuilder 来构建替换后的字符串
            StringBuilder resultBuilder = new StringBuilder();

            // 遍历字符串，替换除最后一个点之外的所有点
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '.' && i != lastIndex)
                {
                    resultBuilder.Append('/');
                }
                else
                {
                    resultBuilder.Append(input[i]);
                }
            }

            // 返回替换后的字符串
            return resultBuilder.ToString();
        }
    }
}