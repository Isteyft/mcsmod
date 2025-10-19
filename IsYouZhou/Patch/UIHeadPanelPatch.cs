using HarmonyLib;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(UIHeadPanel), "OnTieJianBtnClick")]
    public class UIHeadPanelPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(UIHeadPanel __instance)
        {
            if (__instance.BtnCanClick())
            {
                DialogAnalysis.StartTestDialogEvent("RunLua*幽州工具#打开铁剑", null);
                //UIJianLingPanel.OpenPanel();
                //DialogAnalysis.GetInt();
            }
            return false;
        }
    }
}
