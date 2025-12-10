using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime.Tasks;
using BepInEx;
using HarmonyLib;
using Newtonsoft.Json;
using top.Isteyft.MCS.YouZhou.GameData;
using top.Isteyft.MCS.YouZhou.GamePlayer;
using top.Isteyft.MCS.YouZhou.Scene;
using top.Isteyft.MCS.YouZhou.UI;
using UnityEngine;
using YZSceneManeger = top.Isteyft.MCS.YouZhou.Scene.YZSceneManeger;

namespace top.Isteyft.MCS.YouZhou
{
    [BepInDependency("skyswordkill.plugin.NextMoreCommands")]
    [BepInDependency("skyswordkill.plugin.Next")]
    [BepInDependency("MaiJiu.MCS.HH")]
    [BepInDependency("top.Isteyft.MCS.IsTools")]
    [BepInDependency("top.Isteyft.MCS.IsMoDaoKuoZhanMain")]
    [BepInPlugin("top.Isteyft.MCS.YouZhou", "幽州", "1.0.0")]
    public class IsToolsMain : BaseUnityPlugin
    {
        public static IsToolsMain I;

        // Add these new fields to match the original Main class functionality
        private YouZhouUIManager m_UIManagerHandle;
        public static readonly string modPath;
        public static readonly string assetsPath;
        public static readonly string picturesPath;
        // 添加地图数据路径常量
        public static readonly string mapConfigPath;
        public static string dll = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // 添加地图数据字典
        public static Dictionary<int, MapMoveData> MapMoveDatas { get; private set; }
        static IsToolsMain()
        {
            DirectoryInfo parent = Directory.GetParent(typeof(IsToolsMain).Assembly.Location);
            modPath = parent?.FullName;
            assetsPath = Path.Combine(modPath, "BaizeAssets", "AssetBundle");
            picturesPath = Path.Combine(modPath, "BaizeAssets", "Sprite");
            mapConfigPath = Path.Combine(modPath, "BaizeAssets", "MapMoveConfig.json");
        }
        #region 日志
        public static void Log(string message)
        {
            IsToolsMain.I.Logger.LogInfo("==========[IsTools]==========");
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo(message);
            IsToolsMain.I.Logger.LogInfo("");
            IsToolsMain.I.Logger.LogInfo("==========[IsTools]==========");
        }
        public static void LogInfo(string message)
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
        // Add UIManagerHandle property
        public YouZhouUIManager UIManagerHandle
        {
            get { return m_UIManagerHandle; }
        }
        public static YZData YouZhouData { get; set; } = new YZData();
        private void Awake()
        {
            IsToolsMain.I = this;
            // Initialize UI Manager
            GameObject gameObject = new GameObject("YouZhouMOD");
            m_UIManagerHandle = gameObject.AddComponent<YouZhouUIManager>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            YouZhouData = new YZData();

            // Load necessary assets
            LoadAssetBundle();
            LoadPicture();
            LoadMapMoveConfig();
            //Log("加载awake");
            new Harmony("top.Isteyft.MCS.YouZhou.Patch").PatchAll();
            base.gameObject.AddComponent<YZSceneManeger>();
        }

        // Add these methods to support asset loading
        private void LoadAssetBundle()
        {
            if (!Directory.Exists(assetsPath))
                return;

            foreach (var file in Directory.GetFiles(assetsPath))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(file);
                if (assetBundle != null)
                {
                    GameObject[] assets = assetBundle.LoadAllAssets<GameObject>();
                    if (assets != null)
                    {
                        foreach (GameObject gameObject in assets)
                        {
                            m_UIManagerHandle.prefabBank.TryAdd(gameObject.name, gameObject, "");
                        }
                    }
                }
            }
        }

        private void LoadPicture()
        {
            if (!Directory.Exists(picturesPath))
                return;

            foreach (var file in Directory.GetFiles(picturesPath))
            {
                byte[] fileData = File.ReadAllBytes(file);
                Texture2D texture = new Texture2D(300, 300);
                texture.LoadImage(fileData);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                m_UIManagerHandle.spriteBank.TryAdd(Path.GetFileName(file), sprite, "");
            }
        }
        // 添加地图配置加载方法
        private void LoadMapMoveConfig()
        {
            IsToolsMain.Log("开始加载配置1");
            try
            {
                if (!File.Exists(mapConfigPath))
                {
                    Error($"地图移动配置文件不存在: {mapConfigPath}");
                    MapMoveDatas = new Dictionary<int, MapMoveData>();
                    return;
                }

                string json = File.ReadAllText(mapConfigPath);
                var config = JsonConvert.DeserializeObject<MapMoveConfig>(json);

                MapMoveDatas = new Dictionary<int, MapMoveData>();
                foreach (var kvp in config.mapMoveDatas)
                {
                    if (int.TryParse(kvp.Key, out int key))
                    {
                        MapMoveDatas[key] = kvp.Value;
                    }
                    else
                    {
                        IsToolsMain.Warning($"无效的地图节点ID: {kvp.Key}");
                    }
                    IsToolsMain.LogInfo($"节点 {kvp.Key}: 可移动={kvp.Value.canmove}, 连接=[{string.Join(",", kvp.Value.canmoveIndex)}]");
                }

                IsToolsMain.LogInfo($"成功加载 {MapMoveDatas.Count} 个地图节点的移动配置");
            }
            catch (Exception ex)
            {
                IsToolsMain.Error($"加载地图移动配置失败: {ex.Message}");
                MapMoveDatas = new Dictionary<int, MapMoveData>();
            }
        }
    }
}
