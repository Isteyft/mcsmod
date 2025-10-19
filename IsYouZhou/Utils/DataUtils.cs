using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.YouZhou.Utils
{
    public class DataUtils
    {
        // 存储已处理文件名的列表
        public static List<string> files = new List<string>();

        /// <summary>
        /// 将JSON数据保存到指定文件中
        /// </summary>
        /// <param name="fileName">目标文件名</param>
        /// <param name="json">要保存的JSON对象</param>
        public static void SaveData(string fileName, JSONObject json)
        {
            // 如果保存系统中不存在该文件名，则添加新条目
            if (!YSNewSaveSystem.saveJSONObject.ContainsKey(fileName))
            {
                YSNewSaveSystem.saveJSONObject.Add(fileName, new JSONObject());
            }

            // 执行保存操作
            YSNewSaveSystem.Save(fileName, json, true);

            // 如果文件列表中没有该文件名，则添加到列表
            if (!DataUtils.files.Contains(fileName))
            {
                DataUtils.files.Add(fileName);
            }
        }

        /// <summary>
        /// 从指定文件加载JSON数据
        /// </summary>
        /// <param name="fileName">要加载的文件名</param>
        /// <param name="jSONObject">用于接收数据的JSON对象</param>
        /// <returns>成功加载返回true，否则返回false</returns>
        public static bool LoadData(string fileName, JSONObject jSONObject)
        {
            // 从保存系统加载JSON对象
            JSONObject jsonobject = YSNewSaveSystem.LoadJSONObject(fileName, true);

            // 检查加载的数据是否有效
            if (jsonobject != null && jsonobject.keys != null)
            {
                // 将加载的数据复制到目标对象
                foreach (string key in jsonobject.keys)
                {
                    jSONObject.SetField(key, jsonobject[key]);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将任意对象序列化为JSON并保存到文件
        /// </summary>
        /// <param name="T">要保存的对象</param>
        /// <param name="fileName">目标文件名</param>
        /// <param name="formatting">JSON格式化选项（默认缩进）</param>
        public static void SaveData(object T, string fileName, Formatting formatting = Formatting.Indented)
        {
            if (T != null)
            {
                // 序列化对象为JSON字符串
                string contents = JsonConvert.SerializeObject(T, formatting);
                // 写入文件
                File.WriteAllText(YSNewSaveSystemPatch.GetNowSavePath + fileName, contents);
            }
        }

        /// <summary>
        /// 从文件加载JSON数据并反序列化为指定类型对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="fileName">要加载的文件名</param>
        /// <param name="t">用于接收数据的对象引用</param>
        public static void LoadData<T>(string fileName, ref T t)
        {
            string jsonContent;
            // 尝试读取JSON文件内容
            if (JsonUtil.TryGetJson(YSNewSaveSystemPatch.GetNowSavePath + fileName, out jsonContent))
            {
                // 反序列化为目标类型
                t = JsonConvert.DeserializeObject<T>(jsonContent);
            }
        }
    }


    public class YSNewSaveSystemPatch
    {
        public static string GetNowSavePath
        {
            get
            {
                return string.Concat(new string[]
                {
                    Paths.GetNewSavePath(),
                    "/",
                    YSNewSaveSystem.NowAvatarPathPre
                });
            }
        }
    }
}