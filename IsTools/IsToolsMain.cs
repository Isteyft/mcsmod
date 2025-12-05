using BepInEx;
using HarmonyLib;
using SkySwordKill.Next;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using top.Isteyft.MCS.IsTools.ModPatch;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine;
using WXB;
using XCharts;

namespace top.Isteyft.MCS.IsTools
{
    [BepInDependency("skyswordkill.plugin.NextMoreCommands")]
    [BepInDependency("skyswordkill.plugin.Next")]
    [BepInPlugin("top.Isteyft.MCS.IsTools", "白泽工具库", "1.0.0")]
    public class IsToolsMain : BaseUnityPlugin
    {
        public static IsToolsMain I;
        public static List<DirectoryInfo> UsingMods = new List<DirectoryInfo>();
        public static List<DirectoryInfo> MyMods = new List<DirectoryInfo>();
        public static List<DirectoryInfo> AllMods = new List<DirectoryInfo>();
        public static string dll = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static Dictionary<string, AssetBundle> AssetBundles = new Dictionary<string, AssetBundle>();
        public const string EffectPath = "/plugins/BaizeAssets/Effect";
        public static Dictionary<string, AssetBundle> EffectAssetBundles = new Dictionary<string, AssetBundle>();
        #region 日志打印
        public static void Log(object message)
        {
            IsToolsMain.I.Logger.LogInfo("==========[IsTools]==========");
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo(message);
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void LogInfo(object message)
        {
            IsToolsMain.I.Logger.LogInfo(message);
        }

        public static void Error(object message)
        {
            IsToolsMain.I.Logger.LogInfo("==========[错误]==========");
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo(message);
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Warning(object message)
        {
            IsToolsMain.I.Logger.LogInfo("==========[警告]==========");
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo(message);
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo("==========[IsTools]==========");
        }
        #endregion
        private void Awake()
        {
            IsToolsMain.I = this;
            new Harmony("top.Isteyft.MCS.IsTools.Patch").PatchAll();
            LoadAssetBundles();
            LoadAllScene();
            Harmony.CreateAndPatchAll(typeof(EffectPatch), null);
        }

        private void Start()
        {
            if (ModUtil.CheckModActive("2929474347"))
            {
                LogInfo("检测到更多NPC信息");
                Harmony.CreateAndPatchAll(typeof(MoreNPCInfoPatch), null);
            }
            Harmony.CreateAndPatchAll(typeof(LianQiNamePatch), null);
            Harmony.CreateAndPatchAll(typeof(LianQiLastNamePatch), null);
        }

        private void LoadAllScene()
        {
            List<DirectoryInfo> allModDirectory = WorkshopTool.GetAllModDirectory();
            string text = "";
            foreach (DirectoryInfo directoryInfo in allModDirectory)
            {
                if (!WorkshopTool.CheckModIsDisable(directoryInfo.Name))
                {
                    IsToolsMain.AllMods.Add(directoryInfo);
                    IsToolsMain.UsingMods.Add(directoryInfo);
                    LoadEffectAssetBundles(directoryInfo.FullName + "/plugins/BaizeAssets/Effect");
                }
            }
            DirectoryInfo directoryInfo2 = new DirectoryInfo(Application.dataPath + "/../本地Mod测试");
            if (directoryInfo2.Exists)
            {
                foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories())
                {
                    IsToolsMain.MyMods.Add(directoryInfo3);
                    IsToolsMain.AllMods.Add(directoryInfo3);
                    LoadEffectAssetBundles(directoryInfo3.FullName + "/plugins/BaizeAssets/Effect");
                }
            }
        }

        private void LoadAssetBundles()
        {
            string path = dll + "/BaizeAssets";
            string[] files = Directory.GetFiles(path, "*.ab");
            foreach (string filename in files)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                LogInfo("载入AssetBundle:" + fileNameWithoutExtension);
                AssetBundle value = AssetBundle.LoadFromFile(filename);
                AssetBundles.Add(fileNameWithoutExtension, value);
            }
        }

        private void LoadEffectAssetBundles(string path)
        {
            //  判断目录是否存在
            if (!Directory.Exists(path)) return;
            // 获取目录下的文件
            string[] files = Directory.GetFiles(path);
            if (files == null) return;

            foreach (string filePath in files)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(filePath);
                if (ab == null) continue;
                // 获取ab包里面的所有资源
                string[] assetNames = ab.GetAllAssetNames();
                if (assetNames.Length > 1)
                {
                    foreach (string assetPath in assetNames)
                    {
                        // 获取所有AB包的文件
                        EffectAssetBundles[Path.GetFileNameWithoutExtension(assetPath)] = ab;
                    }
                }
                else if (assetNames.Length == 1)
                {
                    //如果只有1个资源，只加载一次
                    EffectAssetBundles[Path.GetFileNameWithoutExtension(assetNames[0])] = ab;
                }
            }
        }
    }
}
