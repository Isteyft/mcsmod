using HarmonyLib;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab;

namespace top.Isteyft.MCS.YouZhou.Patch.Tab
{
    [HarmonyPatch(typeof(WuDaoSlot))]
    public class WuDaoSlotPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Study")]
        public static bool Prefix_Study(WuDaoSlot __instance)
        {
            //UIPopTip.Inst.Pop($"{__instance.Id}", PopTipIconType.叹号);
            if (__instance.Id == 1100)
            {
                if (DialogAnalysis.GetInt("幽州-堕入魔道")==1)
                {
                    return true;
                }
                UIPopTip.Inst.Pop("未堕入魔道，无法感悟。", PopTipIconType.叹号);
                return false;
            }
            return true;
        }
    }
}
