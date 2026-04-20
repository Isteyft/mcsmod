using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.TianNan.Scene;
using UnityEngine;

namespace top.Isteyft.MCS.TianNan
{
    [BepInDependency("skyswordkill.plugin.NextMoreCommands")]
    [BepInDependency("skyswordkill.plugin.Next")]
    [BepInDependency("MaiJiu.MCS.HH")]
    [BepInPlugin("top.Isteyft.MCS.TianNan", "天南", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public static Main I;
        public static string dll = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        #region 日志打印
        public static void Log(object message)
        {
            Main.I.Logger.LogInfo("==========[IsTools]==========");
            Main.I.Logger.LogInfo("");
            Main.I.Logger.LogInfo(message);
            Main.I.Logger.LogInfo("");
            Main.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void LogInfo(object message)
        {
            Main.I.Logger.LogInfo(message);
        }

        public static void Error(object message)
        {
            Main.I.Logger.LogInfo("==========[错误]==========");
            Main.I.Logger.LogInfo("");
            Main.I.Logger.LogInfo(message);
            Main.I.Logger.LogInfo("");
            Main.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Warning(object message)
        {
            Main.I.Logger.LogInfo("==========[警告]==========");
            Main.I.Logger.LogInfo("");
            Main.I.Logger.LogInfo(message);
            Main.I.Logger.LogInfo("");
            Main.I.Logger.LogInfo("==========[IsTools]==========");
        }
        #endregion

        public void Awake()
        {
            Main.I = this;
            new Harmony("top.Isteyft.MCS.YouZhou.Patch").PatchAll();
            base.gameObject.AddComponent<SceneManeger>();
        }
    }

}
