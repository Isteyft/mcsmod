using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using top.Isteyft.MCS.YouZhou.GamePlayer;
using top.Isteyft.MCS.YouZhou.UI;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou
{
    [BepInPlugin("top.Isteyft.MCS.YouZhou", "幽州", "1.0.0")]
    public class IsToolsMain : BaseUnityPlugin
    {
        public static IsToolsMain I;

        // Add these new fields to match the original Main class functionality
        private YouZhouUIManager m_UIManagerHandle;
        public static readonly string modPath;
        public static readonly string assetsPath;
        public static readonly string picturesPath;
        public static string dll = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static IsToolsMain()
        {
            DirectoryInfo parent = Directory.GetParent(typeof(IsToolsMain).Assembly.Location);
            modPath = parent?.FullName;
            assetsPath = Path.Combine(modPath, "BaizeAssets", "AssetBundle");
            picturesPath = Path.Combine(modPath, "BaizeAssets", "Sprite");
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

        private void Awake()
        {
            IsToolsMain.I = this;
            // Initialize UI Manager
            GameObject gameObject = new GameObject("YouZhouMOD");
            m_UIManagerHandle = gameObject.AddComponent<YouZhouUIManager>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            // Load necessary assets
            LoadAssetBundle();
            LoadPicture();
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
    }
}
