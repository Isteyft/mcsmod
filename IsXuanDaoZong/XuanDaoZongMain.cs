using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;

namespace top.Isteyft.MCS.XuanDaoZong
{
    [BepInPlugin("top.Isteyft.MCS.XuanDaoZong", "XuanDaoZong", "1.0.0")]
    public class XuanDaoZongMain : BaseUnityPlugin
    {
        public static XuanDaoZongMain I;

        public static void Log(string message)
        {
            XuanDaoZongMain.I.Logger.LogInfo("==========[IsTools]==========");
            XuanDaoZongMain.I.Logger.LogInfo("");
            XuanDaoZongMain.I.Logger.LogInfo(message);
            XuanDaoZongMain.I.Logger.LogInfo("");
            XuanDaoZongMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Error(object message)
        {
            XuanDaoZongMain.I.Logger.LogInfo("==========[错误]==========");
            XuanDaoZongMain.I.Logger.LogInfo("");
            XuanDaoZongMain.I.Logger.LogInfo(message);
            XuanDaoZongMain.I.Logger.LogInfo("");
            XuanDaoZongMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Warning(object message)
        {
            XuanDaoZongMain.I.Logger.LogInfo("==========[警告]==========");
            XuanDaoZongMain.I.Logger.LogInfo("");
            XuanDaoZongMain.I.Logger.LogInfo(message);
            XuanDaoZongMain.I.Logger.LogInfo("");
            XuanDaoZongMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        private void Awake()
        {
            XuanDaoZongMain.I = this;
            new Harmony("top.Isteyft.MCS.XuanDaoZong.Patch").PatchAll();
        }

    }
}
