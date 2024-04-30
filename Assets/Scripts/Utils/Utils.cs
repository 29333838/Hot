using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Wxy.Core
{
    public static class Utils
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

        /// <summary>
        /// 根据文件生成md5码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] md5Info = md5.ComputeHash(fileStream);
                fileStream.Close();

                StringBuilder res = new StringBuilder();
                for (int i = 0; i < md5Info.Length; i++)
                {
                    res.Append(md5Info[i].ToString("x2"));
                }

                return res.ToString();
            }
        }
        /// <summary>
        /// 获取资源的最近一层文件夹路径
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static string GetNearestFolder(string assetPath)
        {
            string folderPath = Path.GetDirectoryName(assetPath);
            string folderName = Path.GetFileName(folderPath);
            return folderName;
        }
        /// <summary>
        /// 得到时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddTicks((long)DateTime.UtcNow.Ticks);
            return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}