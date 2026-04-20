using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace top.Isteyft.MCS.JiuZhou.Patch
    {
    /// <summary>
    /// 对UINPCQingJiao类进行功能修改的Harmony补丁类
    /// </summary>
    [HarmonyPatch(typeof(UINPCQingJiao))]
    public class UINPCQingJiaoPatch
    {
        /// <summary>
        /// 技能品质与所需好感度的映射字典
        /// </summary>
        private static Dictionary<int, int> FavorDict = new Dictionary<int, int>
        {
            { 1, 5 },  // 品质1需要好感度5
            { 2, 6 },  // 品质2需要好感度6
            { 3, 8 }   // 品质3需要好感度8
        };
        /// <summary>
        /// 功法技能槽点击的自定义处理
        /// </summary>
        private static void GongFaSlotAction(UIIconShow slot, bool isMiChuan, bool isShiFu, int qingJiaoType, int pinJie, JSONObject skill, UINPCData npc)
        {
            if (slot.IsLingWu)
            {
                UIPopTip.Inst.Pop("你已经领悟这个功法了", PopTipIconType.叹号);
                return;
            }

            UINPCJiaoHu.Inst.QingJiaoName = skill["name"].Str.RemoveNumber();

            if (isMiChuan && !isShiFu)
            {
                // 检查学习秘传功法所需的声望
                if (PlayerEx.GetShengWang(750) < 1000)
                {
                    UIPopTip.Inst.Pop("你的幽州声望不足以学习此功法", PopTipIconType.叹号);
                    UINPCJiaoHu.Inst.IsQingJiaoShiBaiSW = true;
                    return;
                }
            }

            // 检查好感度要求
            if (npc.FavorLevel < UINPCQingJiaoPatch.FavorDict[pinJie])
            {
                UIPopTip.Inst.Pop("你的好感度不足以学习此功法", PopTipIconType.叹号);
                return;
            }

            // 检查情分(关系点数)要求
            int qingFenCost = NPCEx.GetQingFenCost(skill, true);
            if (npc.QingFen < qingFenCost)
            {
                UIPopTip.Inst.Pop("你们的情分不足以学习此功法", PopTipIconType.叹号);
                UINPCJiaoHu.Inst.IsQingJiaoShiBaiQF = true;
                return;
            }

            // 成功学习技能
            NPCEx.AddQingFen(npc.ID, -qingFenCost, false);
            UINPCJiaoHu.Inst.JiaoHuItemID = UINPCQingJiaoSkillData.GongFaItemDict[skill["Skill_ID"].I];
            PlayerEx.Player.addItem(UINPCJiaoHu.Inst.JiaoHuItemID, 1, null, true);
            UINPCJiaoHu.Inst.IsQingJiaoChengGong = true;
            NpcJieSuanManager.inst.isUpDateNpcList = true;
        }

        /// <summary>
        /// 神通技能槽点击的自定义处理
        /// </summary>
        private static void ShenTongSlotAction(UIIconShow slot, bool isMiChuan, bool isShiFu, int qingJiaoType, int pinJie, JSONObject skill, UINPCData npc)
        {
            if (slot.IsLingWu)
            {
                UIPopTip.Inst.Pop("你已经领悟这个神通了", PopTipIconType.叹号);
                return;
            }

            UINPCJiaoHu.Inst.QingJiaoName = skill["name"].Str.RemoveNumber();

            if (isMiChuan && npc.IsNingZhouNPC && !isShiFu)
            {
                // 检查学习秘传神通所需的声望
                if (PlayerEx.GetShengWang(750) < 1000)
                {
                    UIPopTip.Inst.Pop("你的幽州声望不足以学习此神通", PopTipIconType.叹号);
                    UINPCJiaoHu.Inst.IsQingJiaoShiBaiSW = true;
                    return;
                }
            }

            // 检查好感度要求
            if (npc.FavorLevel < UINPCQingJiaoPatch.FavorDict[pinJie])
            {
                UIPopTip.Inst.Pop("你的好感度不足以学习此神通", PopTipIconType.叹号);
                return;
            }

            // 检查情分(关系点数)要求
            int qingFenCost = NPCEx.GetQingFenCost(skill, false);
            if (npc.QingFen < qingFenCost)
            {
                UIPopTip.Inst.Pop("你们的情分不足以学习此神通", PopTipIconType.叹号);
                UINPCJiaoHu.Inst.IsQingJiaoShiBaiQF = true;
                return;
            }

            // 成功学习技能
            NPCEx.AddQingFen(npc.ID, -qingFenCost, false);
            UINPCJiaoHu.Inst.JiaoHuItemID = UINPCQingJiaoSkillData.SkillItemDict[skill["Skill_ID"].I];
            PlayerEx.Player.addItem(UINPCJiaoHu.Inst.JiaoHuItemID, 1, null, true);
            UINPCJiaoHu.Inst.IsQingJiaoChengGong = true;
            NpcJieSuanManager.inst.isUpDateNpcList = true;
        }

        /// <summary>
        /// CreateQingJiaoGongFaSlot方法的前置补丁
        /// </summary>
        [HarmonyPatch("CreateQingJiaoGongFaSlot")]
        [HarmonyPrefix]
        public static bool CreateQingJiaoGongFaSlot_Prefix(UINPCQingJiao __instance, UINPCQingJiaoSkillData.SData data)
        {
            JSONObject skill = jsonData.instance.StaticSkillJsonData.list.Find((JSONObject s) => s["id"].I == data.ID);
            int qingJiaoType = skill["qingjiaotype"].I;

            if (qingJiaoType != 70)
            {
                return true;
            }

            // 创建技能槽UI
            UIIconShow icon = UnityEngine.Object.Instantiate<GameObject>(__instance.IconPrefab, __instance.RTList[data.Quality - 1]).GetComponent<UIIconShow>();
            icon.SetStaticSkill(data.ID, true);
            icon.CanDrag = false;

            // 检查是否是秘传功法且玩家不是师父
            bool isMiChuan = qingJiaoType == 70;
            UINPCData npc = Traverse.Create(__instance).Field("npc").GetValue<UINPCData>();
            bool isShiFu = PlayerEx.IsTheather(npc.ID);

            if (isMiChuan && !isShiFu)
            {
                // 检查声望要求
                if (PlayerEx.GetShengWang(750) < 1000)
                {
                    icon.SetBuChuan(); // 标记为不可传授
                }
            }

            // 设置点击事件
            UnityAction action = delegate ()
            {
                UINPCQingJiaoPatch.GongFaSlotAction(icon, isMiChuan, isShiFu, qingJiaoType, data.Quality, skill, npc);
            };

            icon.OnClick = (UnityAction<PointerEventData>)Delegate.Combine(icon.OnClick, (UnityAction<PointerEventData>)delegate
            {
                USelectBox.Show("是否确定请教此功法？", action);
            });

            return false;
        }

        /// <summary>
        /// CreateQingJiaoShenTongSlot方法的前置补丁
        /// </summary>
        [HarmonyPatch("CreateQingJiaoShenTongSlot")]
        [HarmonyPrefix]
        public static bool CreateQingJiaoShenTongSlot_Prefix(UINPCQingJiao __instance, UINPCQingJiaoSkillData.SData data)
        {
            JSONObject skill = jsonData.instance._skillJsonData.list.Find((JSONObject s) => s["id"].I == data.ID);
            int qingJiaoType = skill["qingjiaotype"].I;

            if (qingJiaoType != 70)
            {
                return true;
            }

            // 创建技能槽UI
            UIIconShow icon = UnityEngine.Object.Instantiate<GameObject>(__instance.IconPrefab, __instance.RTList[data.Quality - 1]).GetComponent<UIIconShow>();
            icon.SetSkill(data.ID, true, 1);
            icon.CanDrag = false;

            UINPCData npc = Traverse.Create(__instance).Field("npc").GetValue<UINPCData>();
            bool isMiChuan = qingJiaoType == 70;
            bool isShiFu = PlayerEx.IsTheather(npc.ID);

            if (isMiChuan && !isShiFu)
            {
                // 检查声望要求
                if (PlayerEx.GetShengWang(750) < 1000)
                {
                    icon.SetBuChuan(); // 标记为不可传授
                }
            }

            // 设置点击事件
            UnityAction action = delegate ()
            {
                UINPCQingJiaoPatch.ShenTongSlotAction(icon, isMiChuan, isShiFu, qingJiaoType, data.Quality, skill, npc);
            };

            icon.OnClick = (UnityAction<PointerEventData>)Delegate.Combine(icon.OnClick, (UnityAction<PointerEventData>)delegate
            {
                USelectBox.Show("是否确定请教此神通？", action);
            });

            return false;
        }

    }
}