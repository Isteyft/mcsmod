using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class ModConfigUtils
    {
        public static string GetGlobalSavePath()
        {
            return Paths.GetNewSavePath() + "/";
        }

        public static string GetCurrentSavePath()
        {
            return GetGlobalSavePath() + YSNewSaveSystem.NowAvatarPathPre + "/";
        }

        // 获取配置文件完整路径
        private static string GetConfigFilePath()
        {
            return Path.Combine(GetGlobalSavePath(), "IsConfig.json");
        }

        // 读取整个配置文件（带空文件处理）
        public static Dictionary<string, object> LoadConfig()
        {
            string filePath = GetConfigFilePath();
            string directory = Path.GetDirectoryName(filePath);

            // 确保目录存在
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 文件不存在时创建空文件
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "{}");
                return new Dictionary<string, object>();
            }

            // 空文件处理
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0)
            {
                File.WriteAllText(filePath, "{}");
                return new Dictionary<string, object>();
            }

            string json = File.ReadAllText(filePath);
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return result ?? new Dictionary<string, object>();
        }

        // 保存整个配置文件（带锁防止冲突）
        private static readonly object _fileLock = new object();
        private static void SaveConfig(Dictionary<string, object> config)
        {
            if (config == null) return;

            string filePath = GetConfigFilePath();
            string directory = Path.GetDirectoryName(filePath);

            // 确保目录存在
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);

            lock (_fileLock)
            {
                File.WriteAllText(filePath, json);
            }
        }

        // 添加/更新配置属性
        public static void SetConfigProperty(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            if (value == null) return;

            Dictionary<string, object> config = LoadConfig();
            config[key] = value;
            SaveConfig(config);
        }

        // 读取配置属性
        // 读取配置属性（默认返回字符串，其他类型转为字符串）
        public static string GetConfigProperty(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            Dictionary<string, object> config = LoadConfig();

            if (config.TryGetValue(key, out object value))
            {
                try
                {
                    // 处理JObject嵌套情况
                    if (value is JObject jObj)
                    {
                        return jObj.ToString(Formatting.None);
                    }

                    // 处理JArray情况
                    if (value is JArray jArray)
                    {
                        return jArray.ToString(Formatting.None);
                    }

                    // 其他类型直接转为字符串
                    return value?.ToString() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        // 保留泛型方法作为可选功能（如果需要获取特定类型）
        public static T GetConfigProperty<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            Dictionary<string, object> config = LoadConfig();

            if (config.TryGetValue(key, out object value))
            {
                try
                {
                    // 处理JObject嵌套情况
                    if (value is JObject jObj)
                    {
                        return jObj.ToObject<T>();
                    }

                    // 处理JArray情况
                    if (value is JArray jArray)
                    {
                        return jArray.ToObject<T>();
                    }

                    // 其他类型转换
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        // 删除配置属性
        public static bool RemoveConfigProperty(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            Dictionary<string, object> config = LoadConfig();

            if (config.Remove(key))
            {
                SaveConfig(config);
                return true;
            }
            return false;
        }
    }
}