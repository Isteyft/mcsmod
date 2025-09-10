//using HarmonyLib;
//using JSONClass;
//using KBEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
//{
//    [HarmonyPatch(typeof(GUIPackage.Skill))]
//    public class SkillPatch
//    {
//        [HarmonyPrefix]
//        [HarmonyPatch("CanUse")]
//        public static bool PrefixCanUse(ref Entity _attaker, ref Entity _receiver, ref bool showError, ref string uuid, ref GUIPackage.Skill __instance, ref SkillCanUseType __result)
//        {
//            showError = false;
//            uuid = "";
//            Avatar avatar = (Avatar)_attaker;
//            Avatar avatar2 = (Avatar)_receiver;

//            // 检查技能冷却
//            if (__instance.CurCD != 0f)
//            {
//                if (avatar.isPlayer() && showError)
//                {
//                    UIPopTip.Inst.Pop(Tools.getStr("shangweilengque"), PopTipIconType.叹号);
//                }
//                __result = SkillCanUseType.尚未冷却不能使用;
//                return false;
//            }

//            // 检查角色是否死亡
//            if (avatar.HP <= 0 || avatar2.HP <= 0)
//            {
//                __result = SkillCanUseType.角色死亡不能使用;
//                return false;
//            }

//            // 检查角色状态
//            if (avatar.state == 1 || avatar2.state == 1)
//            {
//                __result = SkillCanUseType.角色死亡不能使用;
//                return false;
//            }

//            // 检查是否为自己的回合
//            if (avatar.state != 3)
//            {
//                __result = SkillCanUseType.非自己回合不能使用;
//                return false;
//            }

//            // 检查技能使用次数限制
//            if (avatar.buffmag.HasBuffSeid(83))
//            {
//                foreach (List<int> list in avatar.buffmag.getBuffBySeid(83))
//                {
//                    int value = BuffSeidJsonData83.DataDict[list[2]].value1;
//                    if (avatar.fightTemp.NowRoundUsedSkills.Count >= value && jsonData.instance.Buff[list[2]].CanRealized(avatar, null, list))
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop(Tools.getStr("yidaodaozuida"), PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.超过最多使用次数不能使用;
//                        return false;
//                    }
//                }
//            }

//            // 检查技能附加条件
//            foreach (int num in _skillJsonData.DataDict[__instance.skill_ID].seid)
//            {
//                // 检查Buff层数
//                if (num == 45 || num == 46 || num == 47)
//                {
//                    JSONObject seidJson = __instance.getSeidJson(num);
//                    int requiredBuffID = seidJson["value1"].I;
//                    int requiredBuffCount = seidJson["value2"].I;

//                    if (!avatar.buffmag.HasBuff(requiredBuffID) || avatar.buffmag.getBuffByID(requiredBuffID)[0][1] < requiredBuffCount)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop(Tools.getStr("Buffcengshubuzu"), PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.Buff层数不足无法使用;
//                        return false;
//                    }
//                }

//                // 检查多个Buff条件
//                if (num == 57)
//                {
//                    JSONObject seidJson = __instance.getSeidJson(num);
//                    JSONObject requiredBuffIDs = seidJson["value1"];
//                    JSONObject requiredBuffCounts = seidJson["value2"];

//                    for (int i = 0; i < requiredBuffIDs.Count; i++)
//                    {
//                        if (!avatar.buffmag.HasBuff(requiredBuffIDs[i].I) || avatar.buffmag.getBuffByID(requiredBuffIDs[i].I)[0][1] < requiredBuffCounts[i].I)
//                        {
//                            if (avatar.isPlayer() && showError)
//                            {
//                                UIPopTip.Inst.Pop(Tools.getStr("Buffcengshubuzu"), PopTipIconType.叹号);
//                            }
//                            __result = SkillCanUseType.Buff层数不足无法使用;
//                            return false;
//                        }
//                    }
//                }

//                // 检查遁速
//                if (num == 93 && avatar.dunSu < avatar2.dunSu)
//                {
//                    if (avatar.isPlayer() && showError)
//                    {
//                        UIPopTip.Inst.Pop("遁速需大于敌人", PopTipIconType.叹号);
//                    }
//                    __result = SkillCanUseType.遁速不足无法使用;
//                    return false;
//                }

//                // 检查血量
//                if (num == 76)
//                {
//                    int requiredHP = __instance.getSeidJson(num)["value1"].I;
//                    if (avatar.HP > requiredHP)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop(Tools.getStr("xieliangdiyu").Replace("{X}", requiredHP.ToString()), PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.血量太高无法使用;
//                        return false;
//                    }
//                }

//                // 检查Buff类型
//                if (num == 109)
//                {
//                    int requiredBuffType = __instance.getSeidJson(num)["value1"].I;
//                    if (!avatar.buffmag.HasXTypeBuff(requiredBuffType))
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop(Tools.getStr("Buffcengshubuzu"), PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.Buff层数不足无法使用;
//                        return false;
//                    }
//                }

//                // 检查寿元
//                if (num == 111)
//                {
//                    int requiredShouYuan = __instance.getSeidJson(num)["value1"].I;
//                    if (avatar.isPlayer() && (ulong)(avatar.shouYuan - avatar.age) - (ulong)requiredShouYuan <= 0UL)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop("寿元不足无法使用", PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.寿元不足无法使用;
//                        return false;
//                    }
//                    else if (!avatar.isPlayer() && NpcJieSuanManager.inst.GetNpcShengYuTime(Tools.instance.MonstarID) - requiredShouYuan <= 0)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop("寿元不足无法使用", PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.寿元不足无法使用;
//                        return false;
//                    }
//                }

//                // 检查Buff种类
//                if (num == 140)
//                {
//                    JSONObject requiredBuffIDs = __instance.getSeidJson(num)["value1"];
//                    int count = requiredBuffIDs.Count;
//                    int foundCount = 0;

//                    for (int i = 0; i < count; i++)
//                    {
//                        if (avatar.buffmag.HasBuff(requiredBuffIDs[i].I))
//                        {
//                            foundCount++;
//                        }
//                    }

//                    if (foundCount < count)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop("需要的buff不足无法释放", PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.Buff种类不足无法使用;
//                        return false;
//                    }
//                }

//                // 检查血量百分比
//                if (num == 145)
//                {
//                    int requiredHPPercentage = __instance.getSeidJson(num)["value1"].I;
//                    if ((float)avatar.HP / (float)avatar.HP_Max * 100f > requiredHPPercentage)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop(Tools.getStr("xieliangdiyu").Replace("{X}", requiredHPPercentage.ToString()), PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.血量太高无法使用;
//                        return false;
//                    }
//                }

//                // 检查Buff回合数
//                if (num == 147)
//                {
//                    JSONObject seidJson = __instance.getSeidJson(num);
//                    int buffID = seidJson["value1"].I;
//                    int requiredRound = seidJson["value2"].I;
//                    int actualRound = __instance.getTargetAvatar(147, _attaker as Avatar).buffmag.GetBuffRoundByID(buffID);

//                    if (!Tools.symbol(seidJson["panduan"].str, actualRound, requiredRound))
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop("未满足释放条件", PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.Buff层数不足无法使用;
//                        return false;
//                    }
//                }

//                // 检查神识
//                if (num == 163)
//                {
//                    int requiredShenShi = __instance.getSeidJson(num)["value1"].I;
//                    if (avatar.shengShi <= requiredShenShi)
//                    {
//                        if (avatar.isPlayer() && showError)
//                        {
//                            UIPopTip.Inst.Pop(string.Format("神识大于{0}才能使用", requiredShenShi), PopTipIconType.叹号);
//                        }
//                        __result = SkillCanUseType.神识不足无法使用;
//                        return false;
//                    }
//                }
//            }

//            // 检查物品附加条件
//            if (__instance.ItemAddSeid != null)
//            {
//                List<JSONObject> list2 = __instance.ItemAddSeid.list;
//                if (list2.Count > 1)
//                {
//                    foreach (JSONObject item in list2)
//                    {
//                        if (item["id"].I == 145 && (float)avatar.HP / (float)avatar.HP_Max * 100f > item["value1"].n)
//                        {
//                            if (avatar.isPlayer() && showError)
//                            {
//                                UIPopTip.Inst.Pop(string.Format("生命值{0}%以下才能使用", item["value1"].n), PopTipIconType.叹号);
//                            }
//                            __result = SkillCanUseType.血量太高无法使用;
//                            return false;
//                        }
//                    }
//                }
//            }

//            // 检查本回合是否使用过其他技能
//            foreach (int num3 in _skillJsonData.DataDict[__instance.skill_ID].seid)
//            {
//                if (num3 == 68 && avatar.fightTemp.NowRoundUsedSkills.Count > 0)
//                {
//                    if (avatar.isPlayer() && showError)
//                    {
//                        UIPopTip.Inst.Pop(Tools.getStr("canNotUseSkill1"), PopTipIconType.叹号);
//                    }
//                    __result = SkillCanUseType.本回合使用过其他技能无法使用;
//                    return false;
//                }
//            }

//            // 检查灵气是否足够
//            foreach (KeyValuePair<int, int> keyValuePair in __instance.getSkillCast(avatar))
//            {
//                if (avatar.cardMag.HasNoEnoughNum(keyValuePair.Key, keyValuePair.Value))
//                {
//                    if (avatar.isPlayer() && showError)
//                    {
//                        UIPopTip.Inst.Pop(Tools.getStr("lingqikapaibuzu"), PopTipIconType.叹号);
//                    }
//                    __result = SkillCanUseType.灵气不足无法使用;
//                    return false;
//                }
//            }

//            // 检查同系灵气是否足够
//            List<int> list3 = __instance.getremoveCastNum(avatar);
//            Dictionary<int, int> dictionary = new Dictionary<int, int>();
//            foreach (KeyValuePair<int, int> keyValuePair2 in __instance.skillSameCast)
//            {
//                bool found = false;
//                for (int m = 0; m < list3.Count; m++)
//                {
//                    if (list3[m] - keyValuePair2.Value >= 0 && !dictionary.ContainsKey(m))
//                    {
//                        dictionary.Add(m, keyValuePair2.Value);
//                        found = true;
//                        break;
//                    }
//                }
//                if (!found)
//                {
//                    if (avatar.isPlayer() && showError)
//                    {
//                        UIPopTip.Inst.Pop(Tools.getStr("tongxilinqikapaibuzu"), PopTipIconType.叹号);
//                    }
//                    __result = SkillCanUseType.灵气不足无法使用;
//                    return false;
//                }
//            }

//            // 如果所有条件都满足
//            __result = SkillCanUseType.可以使用;
//            return false;
//        }
//    }
//}
