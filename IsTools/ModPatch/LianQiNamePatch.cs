using BepInEx;
using HarmonyLib;
using JSONClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;
using Ventulus;

namespace top.Isteyft.MCS.IsTools.ModPatch
{
    public class LianQiNamePatch
    {
        public static List<_FaBaoFirstNameJsonData> FaBaoFirstNameJsonDatas = new List<_FaBaoFirstNameJsonData>();

        public static void Init()
        {
            List<DirectoryInfo> AllMods = IsToolsMain.AllMods;
            foreach (DirectoryInfo directoryInfo in AllMods)
            {
                string path = directoryInfo.FullName + "/plugins/BaizeAssets/config/LianQiFirstName.json";
                string jsonString = JsonUtil.GetJsonString(path);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, _FaBaoFirstNameJsonData>>(jsonString);

                    if (dict != null)
                    {
                        foreach (var entry in dict.Values)
                        {
                            if (entry != null)
                            {
                                FaBaoFirstNameJsonDatas.Add(entry);
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(_FaBaoFirstNameJsonData), "InitDataDict")]
        [HarmonyPostfix]
        public static void PostfixFaBaoFirstNameJsonData()
        {
            LianQiNamePatch.Init();
            try
            {
                int addedCount = 0;

                foreach (var pluginData in LianQiNamePatch.FaBaoFirstNameJsonDatas)
                {
                    if (pluginData == null)
                        continue;

                    // 检查是否已经存在相同 id
                    if (_FaBaoFirstNameJsonData.DataDict.ContainsKey(pluginData.id))
                    {
                        IsToolsMain.LogInfo($"跳过重复 ID 的插件数据：ID = {pluginData.id}");
                        continue;
                    }

                    // 添加到主项目的数据字典和列表
                    if (!_FaBaoFirstNameJsonData.DataDict.ContainsKey(pluginData.id))
                    {
                        _FaBaoFirstNameJsonData.DataDict.Add(pluginData.id, pluginData);
                        _FaBaoFirstNameJsonData.DataList.Add(pluginData);
                        addedCount++;
                    }
                }

                IsToolsMain.LogInfo($"插件数据合并完成，共添加了 {addedCount} 条数据到主项目的 FaBaoFirstName 数据中。");
            }
            catch (Exception ex)
            {
                PreloadManager.LogException("在 PostfixFaBaoFirstNameJsonData 合并插件数据时发生异常！");
                PreloadManager.LogException(ex.ToString());
            }
        }
    }

    public class LianQiLastNamePatch
    {
        public static List<_FaBaoLastNameJsonData> FaBaoLastNameJsonDatas = new List<_FaBaoLastNameJsonData>();

        public static void Init()
        {
            List<DirectoryInfo> AllMods = IsToolsMain.AllMods;
            foreach (DirectoryInfo directoryInfo in AllMods)
            {
                string path = directoryInfo.FullName + "/plugins/BaizeAssets/config/LianQiLastName.json";
                string jsonString = JsonUtil.GetJsonString(path);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    //_FaBaoLastNameJsonData jsonData = JsonConvert.DeserializeObject<_FaBaoLastNameJsonData>(jsonString);
                    //if (jsonData != null)
                    //{
                    //    FaBaoLastNameJsonDatas.Add(jsonData);
                    //}
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, _FaBaoLastNameJsonData>>(jsonString);

                    if (dict != null)
                    {
                        foreach (var entry in dict.Values)
                        {
                            if (entry != null)
                            {
                                FaBaoLastNameJsonDatas.Add(entry);
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(_FaBaoLastNameJsonData), "InitDataDict")]
        [HarmonyPostfix]
        public static void PostfixFaBaoFirstNameJsonData()
        {
            LianQiLastNamePatch.Init();
            try
            {
                int addedCount = 0;

                foreach (var pluginData in LianQiLastNamePatch.FaBaoLastNameJsonDatas)
                {
                    if (pluginData == null)
                        continue;

                    // 检查是否已经存在相同 id
                    if (_FaBaoLastNameJsonData.DataDict.ContainsKey(pluginData.id))
                    {
                        IsToolsMain.LogInfo($"跳过重复 ID 的插件数据：ID = {pluginData.id}");
                        continue;
                    }

                    // 添加到主项目的数据字典和列表
                    if (!_FaBaoLastNameJsonData.DataDict.ContainsKey(pluginData.id))
                    {
                        _FaBaoLastNameJsonData.DataDict.Add(pluginData.id, pluginData);
                        _FaBaoLastNameJsonData.DataList.Add(pluginData);
                        addedCount++;
                    }
                }

                IsToolsMain.LogInfo($"插件数据合并完成，共添加了 {addedCount} 条数据到主项目的 FaBaoLastName 数据中。");
            }
            catch (Exception ex)
            {
                PreloadManager.LogException("在 PostfixFaBaoLastNameJsonData 合并插件数据时发生异常！");
                PreloadManager.LogException(ex.ToString());
            }
        }
    }
}