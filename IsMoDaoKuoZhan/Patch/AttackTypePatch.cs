using HarmonyLib;
using Bag;
using KBEngine;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    [HarmonyPatch(typeof(SlotBase))]
    public class AttackTypePatch
    {
        // 修改技能拖拽逻辑，对 AttackType 11 做特殊处理
        [HarmonyPatch("CanDrag")]
        [HarmonyPostfix]
        public static void CanDrag_Postfix(SlotBase __instance, ref bool __result)
        {
            // 如果原逻辑已允许拖拽，则不再处理
            if (__result) return;

            // 检查技能是否是 ActiveSkill 且 AttackType 包含 11
            if (__instance.Skill is ActiveSkill activeSkill &&
                activeSkill.AttackType.Contains(11))
            {
                // 特殊条件：玩家等级 > 10 时允许拖拽
                //Avatar player = PlayerEx.Player;
                //if (player != null && player.level > 10)
                //{
                //    __result = true;
                //    IsMoDaoKuoZhanMain.LogInfo($"允许拖拽 AttackType 11 的技能（玩家等级：{player.level}）");
                //}
                __result = true;
            }
        }

        // 修改技能伤害计算
        //[HarmonyPatch(typeof(ActiveSkill), "GetDamage")]
        //[HarmonyPostfix]
        //public static void GetDamage_Postfix(ActiveSkill __instance, ref int __result)
        //{
        //    if (__instance.AttackType.Contains(11))
        //    {
        //        // 对 AttackType 11 的技能伤害提升 20%
        //        __result = (int)(__result * 1.2f);
        //        IsMoDaoKuoZhanMain.LogInfo($"AttackType 11 技能伤害提升至：{__result}");
        //    }
        //}
    }
}