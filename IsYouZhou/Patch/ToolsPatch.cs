using HarmonyLib;
using KBEngine;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(Tools), "GetPlayerTitle")]
    public class ToolsPatch
    {
        [HarmonyPrefix]
        public static bool GetPlayerTitlePrefix(ref string __result)
        {
            Avatar player = PlayerEx.Player;
            //Tools.instance.getPlayer();
            if (player.menPai != 0)
            {
                return true;
            }
            if (DialogAnalysis.GetInt("幽州修士") != 1)
            {
                return true;
            }
            string shili = DialogAnalysis.GetStr("幽州家族");
            IsToolsMain.Log(shili);
            switch (shili)
            {
                case "朱":
                    {
                        __result = "朱家修士";
                        return false;
                    }
                case "苏":
                    {
                        __result = "苏家修士";
                        return false;
                    }
                case "白":
                    {
                        __result = "白家修士";
                        return false;
                    }
                case "安":
                    {
                        __result = "安家修士";
                        return false;
                    }
                case "任":
                    {
                        __result = "任家修士";
                        return false;
                    }
                default:
                    {
                        __result = "幽州散修";
                        return false;
                    }
            }
            return false;
        }
        //[HarmonyPostfix]
        //public static void GetPlayerTitlePostfix(string __result)
        //{
        //    IsToolsMain.Log(__result);
        //}
    }
}
