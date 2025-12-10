using System;
using System.IO;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class JsonUtil
    {
        /// <summary>
        /// 原方法：根据 ID 获取套路名称，依赖未定义的 JSONObject 和 jsonData 类。
        /// 目前该方法未实现，需要您补充 JSONObject 和 jsonData 的定义后恢复逻辑。
        /// </summary>
        /// <param name="id">大道 ID</param>
        /// <returns>大道名称字符串，如果未找到则返回 null</returns>
        public static string GetLunDaoName(int id)
        {
            JSONObject jsonobject = jsonData.instance.WuDaoAllTypeJson.list.Find((JSONObject o) => o["id"].I == id);
            return (jsonobject != null) ? jsonobject["name1"].Str : null;
        }

        /// <summary>
        /// 检查文件是否存在，存在则读取全部内容返回，否则返回空字符串
        /// </summary>
        public static string GetJsonString(string path)
        {
            bool flag = File.Exists(path);
            string result;
            if (flag)
            {
                result = File.ReadAllText(path);
            }
            else
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// 尝试读取 JSON 文件，如果文件存在则返回 true 并输出内容，否则返回 false，str 为 ""
        /// </summary>
        public static bool TryGetJson(string path, out string str)
        {
            str = "";
            bool result;
            if (File.Exists(path))
            {
                str = File.ReadAllText(path);
                return true;
            }
            return false;
        }
    }
}