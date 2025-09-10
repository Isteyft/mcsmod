using BepInEx;
using HarmonyLib;
using SkySwordKill.NextMoreCommand.Patchs;
using top.Isteyft.MCS.IsMoXinXiangJuan.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMoXinXiangJuan
{
    [BepInPlugin("top.Isteyft.MCS.IsMoXinXiangJuan", "IsMoXinXiangJuan", "1.0.0")]
    public class IsMoXinXiangJuanMain : BaseUnityPlugin
    {
        public static IsMoXinXiangJuanMain I;

        public static void Log(string message)
        {
            IsMoXinXiangJuanMain.I.Logger.LogInfo("==========[IsTools]==========");
            IsMoXinXiangJuanMain.I.Logger.LogInfo("");
            IsMoXinXiangJuanMain.I.Logger.LogInfo(message);
            IsMoXinXiangJuanMain.I.Logger.LogInfo("");
            IsMoXinXiangJuanMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Error(object message)
        {
            IsMoXinXiangJuanMain.I.Logger.LogInfo("==========[错误]==========");
            IsMoXinXiangJuanMain.I.Logger.LogInfo("");
            IsMoXinXiangJuanMain.I.Logger.LogInfo(message);
            IsMoXinXiangJuanMain.I.Logger.LogInfo("");
            IsMoXinXiangJuanMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        public static void Warning(object message)
        {
            IsMoXinXiangJuanMain.I.Logger.LogInfo("==========[警告]==========");
            IsMoXinXiangJuanMain.I.Logger.LogInfo("");
            IsMoXinXiangJuanMain.I.Logger.LogInfo(message);
            IsMoXinXiangJuanMain.I.Logger.LogInfo("");
            IsMoXinXiangJuanMain.I.Logger.LogInfo("==========[IsTools]==========");
        }

        private void Awake()
        {
            IsMoXinXiangJuanMain.I = this;
            new Harmony("top.Isteyft.MCS.IsMoXinXiangJuan.Patch").PatchAll();
            PlayerSetRandomFaceRandomAvatarPatch.OnSetSpineAvatar += NMCPatch.SpineSpecialSet;
        }
    }
}
