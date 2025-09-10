using Bag;
using DG.Tweening.Plugins.Core;
using HarmonyLib;
using IsMoXinXiangJuan;
using KBEngine;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    public class WuDaoPatch
    {
        public static bool HasWuDaoSkill(int SeidID)
        {
            List<SkillItem> allWuDaoSkills = PlayerEx.Player.wuDaoMag.GetAllWuDaoSkills();

            foreach (SkillItem skillItem in allWuDaoSkills)
            {
                bool flag = skillItem.itemId == SeidID;
                if (flag)
                {
                    return true;
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(Tools))]
        public class ToolsPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Tools.CalcLingWuOrTuPoTime))]
            static void Postfix_CalcLingWuOrTuPoTime_StudyTime(ref int __result)
            {
                if (HasWuDaoSkill(2601))
                {
                    __result = Mathf.CeilToInt(__result / 1.3f);
                }
                if (HasWuDaoSkill(2611))
                {
                    __result = Mathf.CeilToInt(__result / 1.5f);
                }
            }
        }

        //[HarmonyPatch(typeof(UIBiGuan))]
        //public class UIBiGuanPatch
        //{
        //    [HarmonyPostfix]
        //    [HarmonyPatch("getBiguanSpeed")]
        //    static void Postfix_getBiguanSpeed_XiulianSpeed(ref float __result)
        //    {
        //        if (HasWuDaoSkill(2601))
        //        {
        //            __result = Mathf.CeilToInt(__result / 1.3f);
        //        }
        //        if (HasWuDaoSkill(2611))
        //        {
        //            __result = Mathf.CeilToInt(__result / 1.5f);
        //        }
        //    }

        //    [HarmonyPatch(typeof(UIBiGuanXiuLianPanel))]
        //    public class UIBiGuanXiuLianPanelPatch
        //    {
        //        [HarmonyPostfix]
        //        [HarmonyPatch("GetBiguanSpeed")]
        //        static void Postfix_getBiguanSpeed_XiulianSpeed2(ref float __result)
        //        {
        //            if (HasWuDaoSkill(2601))
        //            {
        //                __result = Mathf.CeilToInt(__result / 1.3f);
        //            }
        //            if (HasWuDaoSkill(2611))
        //            {
        //                __result = Mathf.CeilToInt(__result / 1.5f);
        //            }
        //        }
        //    }
        //}



        [HarmonyPatch(typeof(KBEngine.Avatar))]
        public class UIBiGuanPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch("levelUp")]
            public static void levelUp_Postfix()
            {
                IsMoXinXiangJuanMain.Log("突破");
                ushort level = Tools.instance.getPlayer().level;
                ushort num = level;
                if (num == 7)
                {
                    if (HasWuDaoSkill(2612))
                    {
                        IsMoXinXiangJuanMain.Log("心道金丹");
                        DialogAnalysis.StartTestDialogEvent("RunLua*魔心相眷_工具#加血量2612", null);
                    }
                }
            }
        }

    }
}
