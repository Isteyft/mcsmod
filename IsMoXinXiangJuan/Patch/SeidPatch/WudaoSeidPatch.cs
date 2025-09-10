using HarmonyLib;
using IsMoXinXiangJuan;
using KBEngine;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch.SeidPatch
{
    [HarmonyPatch(typeof(WuDaoMag))]
    public class WudaoSeidPatch
    {

        [HarmonyPatch("addWuDaoSkill")]
        [HarmonyPrefix]
        public static bool Prefix_WuDaoMag_addWuDaoSkill(WuDaoMag __instance, int WuDaoType, int type)
        {
            switch (type)
            {
                case 2601:
                    {
                        //Tools.instance.getPlayer()._xinjin += 30;
                        //UIPopTip.Inst.Pop("心境增加30");
                        //IsToolsMain.Log("1");
                        DialogAnalysis.StartTestDialogEvent("RunLua*魔心相眷_工具#加心境2601", null);
                        return true;
                    }
                case 2611:
                    {
                        DialogAnalysis.StartTestDialogEvent("RunLua*魔心相眷_工具#加心境2611", null);
                        return true;
                    }
                case 2612:
                    {
                        IsMoXinXiangJuanMain.Log("2612,魔心相眷-开始结婴触发器开启");
                        DialogAnalysis.StartTestDialogEvent("SetTrigger*魔心相眷-开始结丹触发#1", null);
                        return true;
                    }
                case 2622:
                    {
                        IsMoXinXiangJuanMain.Log("2622,魔心相眷-开始结婴触发器开启");
                        DialogAnalysis.StartDialogEvent("魔心相眷-开始结婴触发器开启", null);
                        return true;
                    }
                case 2632:
                    {
                        IsMoXinXiangJuanMain.Log("26232,魔心相眷-开始始化神触发器开启");
                        DialogAnalysis.StartTestDialogEvent("SetTrigger*魔心相眷-开始化神触发#1", null);
                        return true;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

    }
}
