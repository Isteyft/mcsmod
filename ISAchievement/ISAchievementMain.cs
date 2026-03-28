using BepInEx;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using top.Isteyft.MCS.IsTools.Util;
using top.Isteyft.MCS.ISAchievement.Data;

namespace top.Isteyft.MCS.ISAchievement
{
    [BepInDependency("top.Isteyft.MCS.IsTools")]
    [BepInPlugin("top.Isteyft.MCS.Achievement", "白泽的成就", "1.0.0")]
    public class ISAchievementMain : BaseUnityPlugin
    {

        public static ISAchievementMain I;

        private void Awake()
        {
            ISAchievementMain.I = this;

            //Log("加载awake");
            new Harmony("top.Isteyft.MCS.Achievement.Patch").PatchAll();
            LoadAllData();
        }

        private void LoadAllData()
        {
            List<DirectoryInfo> allModDirectory = WorkshopTool.GetAllModDirectory();
            foreach (DirectoryInfo directoryInfo in allModDirectory)
            {
                if (!WorkshopTool.CheckModIsDisable(directoryInfo.Name))
                {
                    LoadAchievement(directoryInfo.FullName + "/plugins/BaizeAssets/Achievement.json");
                }
            }
            DirectoryInfo directoryInfo2 = new DirectoryInfo(Application.dataPath + "/../本地Mod测试");
            if (directoryInfo2.Exists)
            {
                foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories())
                {
                    LoadAchievement(directoryInfo3.FullName + "/plugins/BaizeAssets/Achievement.json");
                }
            }
        }

        private void LoadAchievement(string path)
        {
            string value;
            if (JsonUtil.TryGetJson(path, out value))
            {
                ISAchievementMain.LogInfo("加载道具配置:" + path);
                List<AchievementData> collection = JsonConvert.DeserializeObject<List<AchievementData>>(value);
                AchievementData.data.AddRange(collection);
            }
        }

        #region 日志
        public static void Log(string message)
        {
            ISAchievementMain.I.Logger.LogInfo("==========[IsTools]==========");
            ISAchievementMain.I.Logger.LogInfo("");
            ISAchievementMain.I.Logger.LogInfo(message);
            ISAchievementMain.I.Logger.LogInfo("");
            ISAchievementMain.I.Logger.LogInfo("==========[IsTools]==========");
        }
        public static void LogInfo(string message)
        {
            ISAchievementMain.I.Logger.LogInfo(message);
        }

        public static void Error(object message)
        {
            ISAchievementMain.I.Logger.LogInfo("==========[错误]==========");
            ISAchievementMain.I.Logger.LogInfo("");
            ISAchievementMain.I.Logger.LogInfo(message);
            ISAchievementMain.I.Logger.LogInfo("");
            ISAchievementMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Warning(object message)
        {
            ISAchievementMain.I.Logger.LogInfo("==========[警告]==========");
            ISAchievementMain.I.Logger.LogInfo("");
            ISAchievementMain.I.Logger.LogInfo(message);
            ISAchievementMain.I.Logger.LogInfo("");
            ISAchievementMain.I.Logger.LogInfo("==========[IsTools]==========");
        }
        #endregion
    }
}
