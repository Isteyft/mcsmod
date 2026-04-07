using BepInEx;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine;

namespace top.Isteyft.MCS.CGView
{
    [BepInDependency("skyswordkill.plugin.Next")]
    [BepInDependency("top.Isteyft.MCS.IsTools")]
    [BepInPlugin("top.Isteyft.MCS.CGView", "CG鉴赏", "1.0.0")]
    public class CGViewMain : BaseUnityPlugin
    {
        public static CGViewMain I { get; private set; }
        public static List<string> ModMP4Paths = new List<string>();

        private void Start()
        {
            I = this;
            new Harmony("top.Isteyft.MCS.CGView").PatchAll();
            Logger.LogInfo("开始加载CG配置。");
            LoadAllScene();
        }
        private void LoadAllScene()
        {
            List<DirectoryInfo> allModDirectory = WorkshopTool.GetAllModDirectory();
            foreach (DirectoryInfo directoryInfo in allModDirectory)
            {
                if (!WorkshopTool.CheckModIsDisable(directoryInfo.Name))
                {
                    LoadCG(directoryInfo.FullName + "/plugins/BaizeAssets/config/CG.json");
                }
            }
            DirectoryInfo directoryInfo2 = new DirectoryInfo(Application.dataPath + "/../本地Mod测试");
            if (directoryInfo2.Exists)
            {
                foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories())
                {
                    LoadCG(directoryInfo3.FullName + "/plugins/BaizeAssets/config/CG.json");
                }
            }
            Logger.LogInfo($"加载{CGData.data.Count}个CG");
        }

        private void LoadCG(string path)
        {
            string value;
            if (JsonUtil.TryGetJson(path, out value))
            {
                Logger.LogInfo("加载CG配置:" + path);
                List<CGData> collection = JsonConvert.DeserializeObject<List<CGData>>(value);
                CGData.data.AddRange(collection);
            }
        }
    }
}
