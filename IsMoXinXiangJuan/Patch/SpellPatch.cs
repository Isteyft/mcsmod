using HarmonyLib;
using IsMoXinXiangJuan;
using JSONClass;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    [HarmonyPatch(typeof(Spell))]
    internal class SpellPatch
    {
        private const int REQUIRED_BUFF_ID = 52623;  // 必须拥有的Buff（道心通明）
        private const int SHIELD_BUFF_ID = 52621;    // 护盾Buff（道心）
        private const int SHIELD_REDUCE_BUFF_ID = 52619; // 护盾扣除Buff

        //[HarmonyPatch("addDBuff")]
        //[HarmonyPrefix]
        //private static bool AddDBuffPrefix(int buffid, ref int num, Spell __instance)
        //{
        //    KBEngine.Avatar avatar = (KBEngine.Avatar)__instance.entity;
        //    avatar.spell.addDBuff(buffid, num);
        //    return false; // 没有道心通明Buff，正常执行
        //}

        [HarmonyPatch("addBuff")]
        [HarmonyPrefix]
        private static bool AddBuffPrefix(int buffid, ref int num, Spell __instance)
        {
            try
            {
                // 1. 获取角色实体
                var avatar = (KBEngine.Avatar)__instance.entity;

                // 2. 检查是否拥有道心通明Buff
                if (avatar.buffmag.HasBuff(REQUIRED_BUFF_ID))
                {
                    int buffType = _BuffJsonData.DataDict[buffid].bufftype;
                    if (buffType == 1 || buffid == 30)
                    {
                        if (buffid==2 && avatar.buffmag.HasBuff(8038)) { return true; }
                        if (buffid == 30 && WudaoUtil.HasWuDaoSkill(2613)) { return true; }
                        // 3. 如果有道心通明Buff，直接抵消全部效果
                        //IsMoXinXiangJuanMain.Log(
                        //    $"道心通明生效，免疫Buff:\n" +
                        //    $"• 目标Buff: {buffid}\n"
                        //);
                        return false; // 阻止原方法执行（完全抵消）
                    }
                    if (buffid == SHIELD_BUFF_ID)
                    {
                        int Num1 = num;
                        num *= 2; // 护盾效果翻倍

                        //IsMoXinXiangJuanMain.Log(
                        //    $"护盾增益生效（道心通明）:\n" +
                        //    $"• 目标Buff: {SHIELD_BUFF_ID}(道心)\n" +
                        //    $"• 数量翻倍: {Num1} → {num}"
                        //);
                    }
                }
            }
            catch (Exception ex)
            {
                IsMoXinXiangJuanMain.Error($"Buff处理异常:\n{ex}");
            }
            return true; // 没有道心通明Buff，正常执行
        }
    }

    //    private static bool AddBuffPrefix(int buffid, ref int num, Spell __instance)
    //    {
    //        try
    //        {
    //            // 1. 获取角色实体
    //            var avatar = (Avatar)__instance.entity;

    //            // 2. 检查是否拥有触发条件Buff（52623）
    //            if (!avatar.buffmag.HasBuff(REQUIRED_BUFF_ID))
    //            {
    //                IsToolsMain.Log($"不存在道心通明buff");
    //                return true;
    //            }

    //            // ===== 新增逻辑开始 =====
    //            // 3. 特殊处理：如果要添加的是护盾Buff（52621），则数量翻倍
    //            if (buffid == SHIELD_BUFF_ID)
    //            {
    //                int Num1 = num;
    //                num *= 2; // 护盾效果翻倍

    //                IsToolsMain.Log(
    //                    $"护盾增益生效（道心通明）:\n" +
    //                    $"• 目标Buff: {SHIELD_BUFF_ID}(护盾)\n" +
    //                    $"• 数量翻倍: {Num1} → {num}"
    //                );

    //                return true; // 继续原方法，添加翻倍后的护盾
    //            }
    //            // ===== 新增逻辑结束 =====

    //            // 4. 检查护盾是否存在（用于后续的护盾抵消逻辑）
    //            if (!avatar.buffmag.HasBuff(SHIELD_BUFF_ID))
    //            {
    //                IsToolsMain.Log($"不存在道心buff");
    //                return true;
    //            }

    //            // 5. 只处理Buff类型为1的效果
    //            int buffType = _BuffJsonData.DataDict[buffid].bufftype;
    //            if (buffType != 1)
    //            {
    //                IsToolsMain.Log($"{buffid}不是类型1的buff");
    //                return true;
    //            }

    //            int originalNum = num;
    //            int shieldAmount = avatar.buffmag.GetBuffSum(SHIELD_BUFF_ID);

    //            // 6. 计算实际要消耗的护盾量
    //            int shieldUsed = Math.Min(num, shieldAmount);

    //            // 7. 减少要添加的Buff层数
    //            num -= shieldUsed;

    //            // 8. 扣除护盾层数
    //            if (shieldUsed > 0)
    //            {
    //                __instance.addBuff(SHIELD_REDUCE_BUFF_ID, shieldUsed);
    //            }

    //            IsToolsMain.Log(
    //                $"护盾消耗处理:\n" +
    //                $"• 目标Buff: {buffid}(类型1)\n" +
    //                $"• 原数量: {originalNum}\n" +
    //                $"• 消耗护盾: {shieldUsed}层\n" +
    //                $"• 最终数量: {num}\n" +
    //                $"• 剩余护盾: {shieldAmount - shieldUsed}层"
    //            );

    //            // 9. 如果完全被护盾抵消，则阻止添加原Buff
    //            if (num <= 0)
    //            {
    //                return false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            IsToolsMain.Error($"护盾处理异常:\n{ex}");
    //        }
    //        return true;
    //    }
    //}
}
