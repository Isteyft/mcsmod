using Bag;
using HarmonyLib;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    [HarmonyPatch(typeof(SlotBase))]
    public class AttackTypePatch
    {
        // 修改技能拖拽逻辑，对 AttackType 26 做特殊处理
        [HarmonyPatch("CanDrag")]
        [HarmonyPostfix]
        public static void CanDrag_Postfix(SlotBase __instance, ref bool __result)
        {
            // 如果原逻辑已允许拖拽，则不再处理
            if (__result) return;

            // 检查技能是否是 ActiveSkill 且 AttackType 包含 26
            if (__instance.Skill is ActiveSkill activeSkill &&
                activeSkill.AttackType.Contains(28))
            {
                //// 特殊条件：玩家等级 > 10 时允许拖拽
                //Avatar player = PlayerEx.Player;
                //if (player != null && player.level > 10)
                //{
                //    __result = true;
                //    IsMoXinXiangJuan.LogInfo($"允许拖拽 AttackType 26 的技能（玩家等级：{player.level}）");
                //}
                __result = true;
            }
        }
    }
}
