using HarmonyLib;
using MaiJiu.MCS.HH.Data;
using MaiJiu.MCS.HH.Patch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;
using top.Isteyft.MCS.YouZhou.GameData;
using top.Isteyft.MCS.YouZhou.Scene;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(YSNewSaveSystem))]
    public class YSNewSavePatch
    {
        public static string SavePath
        {
            get
            {
                string getNowSavePath = YSNewSaveSystemPatch.GetNowSavePath;
                string str = "/MCSDataSaveHelper";
                return getNowSavePath + str;
            }
        }
        [HarmonyPatch("SaveAvatar")]
        [HarmonyPostfix]
        public static void AfterSave()
        {
            if (AllMapBase.Inst != null)
            {
                IsToolsMain.YouZhouData.Data["ActiveTasks"] = string.Join(",", AllMapBase.activeTasks);
                IsToolsMain.YouZhouData.Data["ActiveShijians"] = string.Join(",", AllMapBase.activeShijians);
            }

            // 保存LuDing数组数据
            if (IsToolsMain.YouZhouData != null)
            {
                // 示例：将LuDing数组转换为JSON字符串保存
                if (IsToolsMain.YouZhouData.Data.TryGetValue("LuDing", out string luDingStr))
                {
                    var luDing = JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(luDingStr);
                    IsToolsMain.YouZhouData.Data["LuDing"] = JsonConvert.SerializeObject(luDing, Formatting.Indented);
                }
                else
                {
                    // 初始化空数组
                    var luDing = new List<Dictionary<string, int>>();
                    IsToolsMain.YouZhouData.Data["LuDing"] = JsonConvert.SerializeObject(luDing, Formatting.Indented);
                }

                try
                {
                    DataManager.SaveData(IsToolsMain.YouZhouData.Data, "/YZData.json", Formatting.Indented);
                } catch (Exception e)
                {
                    IsToolsMain.Error(e);
                }
                IsToolsMain.LogInfo("保存幽州存档");
            }
        }

        [HarmonyPatch("LoadSave")]
        [HarmonyPostfix]
        public static void AfterLoad()
        {
            string jsonString = JsonUtil.GetJsonString(YSNewSaveSystemPatch.GetNowSavePath + "/YZData.json");
            if (jsonString != "")
            {
                IsToolsMain.YouZhouData.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

                // 加载静态的activeTasks和activeShijians
                if (IsToolsMain.YouZhouData.Data != null)
                {
                    // 清空现有数据
                    AllMapBase.activeTasks.Clear();
                    AllMapBase.activeShijians.Clear();

                    // 加载activeTasks
                    if (IsToolsMain.YouZhouData.Data.TryGetValue("ActiveTasks", out string tasksStr) && !string.IsNullOrEmpty(tasksStr))
                    {
                        AllMapBase.activeTasks = tasksStr.Split(',')
                            .Where(x => int.TryParse(x, out _))
                            .Select(int.Parse)
                            .ToList();
                    }

                    // 加载activeShijians
                    if (IsToolsMain.YouZhouData.Data.TryGetValue("ActiveShijians", out string shijiansStr) && !string.IsNullOrEmpty(shijiansStr))
                    {
                        AllMapBase.activeShijians = shijiansStr.Split(',')
                            .Where(x => int.TryParse(x, out _))
                            .Select(int.Parse)
                            .ToList();
                    }

                    // 加载LuDing数组数据
                    if (IsToolsMain.YouZhouData.Data.TryGetValue("LuDing", out string luDingStr) && !string.IsNullOrEmpty(luDingStr))
                    {
                        var luDing = JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(luDingStr);
                        // 可以在这里将数据存储到全局变量中供其他地方使用
                    }
                }

                IsToolsMain.LogInfo("读取新幽州存档");
            }
        }
    }
}
