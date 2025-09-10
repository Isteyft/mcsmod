using BepInEx;
using HarmonyLib;
using SkySwordKill.NextMoreCommand.Patchs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.ModPatch.LunDaoPatch;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Utils;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain
{
    [BepInPlugin("top.Isteyft.MCS.IsMoDaoKuoZhanMain", "白泽的魔道扩展", "1.0.0")]
    public class IsMoDaoKuoZhanMain : BaseUnityPlugin
    {
        public static IsMoDaoKuoZhanMain I;
        public static string dll = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string picturesPath = Path.Combine(dll, "BaizeAssets", "Sprite");
        public BaizeUIManager Baize_UIManagerHandle;
        #region 日志输出
        public static void Log(string message)
        {
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("==========[白泽的魔道扩展]==========");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo(message);
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("==========[IsMoDaoKuoZhanMain]==========");
        }

        public static void LogInfo(string message)
        {
            IsMoDaoKuoZhanMain.I.Logger.LogInfo(message);
        }

        public static void Error(object message)
        {
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("==========[错误]==========");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo(message);
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("==========[IsMoDaoKuoZhanMain]==========");
        }

        public static void Warning(object message)
        {
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("==========[警告]==========");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo(message);
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("");
            IsMoDaoKuoZhanMain.I.Logger.LogInfo("==========[IsMoDaoKuoZhanMain]==========");
        }
        #endregion
        private void Awake()
        {
            IsMoDaoKuoZhanMain.I = this;
            new Harmony("top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch").PatchAll();
            GameObject gameObject = new GameObject("YouZhouMOD");
            Baize_UIManagerHandle = gameObject.AddComponent<BaizeUIManager>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            LoadPicture();
        }
        private void Start()
        {
            if (!ModUtil.CheckModActive("3018994631"))
            {
                Harmony.CreateAndPatchAll(typeof(LunDaoManagerPatch), null);
                Harmony.CreateAndPatchAll(typeof(LunDaoPanelPatch), null);
                Harmony.CreateAndPatchAll(typeof(LunDaoSuccessPatch), null);
                Harmony.CreateAndPatchAll(typeof(LunTiMagPatch), null);
                Harmony.CreateAndPatchAll(typeof(SelectLunTiPatch), null);
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
                Baize_UIManagerHandle.spriteBank.TryAdd(Path.GetFileName(file), sprite, "");
            }
        }

        public BaizeUIManager UIManagerHandle
        {
            get { return Baize_UIManagerHandle; }
        }
    }
}
