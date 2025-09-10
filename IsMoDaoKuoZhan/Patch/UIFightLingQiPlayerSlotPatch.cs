//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using YSGame.Fight;

//namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
//{
//    [HarmonyPatch(typeof(UIFightLingQiPlayerSlot))]
//    public class UIFightLingQiPlayerSlotPatch
//    {
//        [HarmonyPrefix]
//        [HarmonyPatch("UseSkillMoveLingQi")]
//        public static bool PrefixUseSkillMoveLingQi(ref UIFightLingQiSlot __instance)
//        {
//            // 如果当前处于释放技能准备灵气阶段且灵气类型不是魔，直接返回true
//            if (UIFightPanel.Inst.UIFightState == UIFightState.释放技能准备灵气阶段 && __instance.LingQiType != LingQiType.魔)
//            {
//                return true;
//            }

//            // 如果当前不在释放技能准备灵气阶段，直接返回false
//            if (UIFightPanel.Inst.UIFightState != UIFightState.释放技能准备灵气阶段)
//            {
//                return false;
//            }

//            // 如果缓存灵气控制器中存在该灵气类型
//            if (UIFightPanel.Inst.CacheLingQiController.GetNowCacheTongLingQi().ContainsKey(__instance.LingQiType))
//            {
//                UIFightPanel.Inst.CacheLingQiController.GetTongLingQiSlot(__instance.LingQiType).UseSkillMoveLingQi();
//                return false;
//            }

//            // 如果当前灵气数量大于0
//            if (__instance.LingQiCount > 0)
//            {
//                UIFightLingQiCacheSlot targetLingQiSlot = UIFightPanel.Inst.CacheLingQiController.GetTargetLingQiSlot(__instance.LingQiType);

//                // 如果没有多余槽位
//                if (targetLingQiSlot == null)
//                {
//                    UIPopTip.Inst.Pop("没有多余槽位", PopTipIconType.叹号);
//                    return false;
//                }

//                int num = targetLingQiSlot.LimitCount - targetLingQiSlot.LingQiCount;

//                // 如果目标灵气槽已满
//                if (num == 0)
//                {
//                    UIPopTip.Inst.Pop("此灵气已经足够", PopTipIconType.叹号);
//                    return false;
//                }

//                // 如果当前灵气数量大于或等于目标槽的剩余容量
//                if (__instance.LingQiCount >= num)
//                {
//                    __instance.LingQiCount -= num;
//                    targetLingQiSlot.LingQiCount += num;
//                    return false;
//                }
//                else
//                {
//                    UIPopTip.Inst.Pop("灵气不足", PopTipIconType.叹号);
//                    return false;
//                }
//            }

//            return false;
//        }
//    }
//}
