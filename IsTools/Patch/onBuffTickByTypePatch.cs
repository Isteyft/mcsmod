using DebuggingEssentials;
using Google.Protobuf.WellKnownTypes;
using HarmonyLib;
using JSONClass;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSGame;

namespace top.Isteyft.MCS.IsTools.Patch
{
    [HarmonyPatch(typeof(Spell), "onBuffTickByType", typeof(int))]
    public class onBuffTickByTypePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(int type, Spell __instance)
        {
            Avatar avatar = (Avatar)__instance.entity;
            BuffMag buffMag = avatar.buffmag;
            // 拥有379seid的buff后进入逻辑
            if (buffMag.HasBuffSeid(379))
            {
                // 获取当前角色身上的 Buff 总数
                int count = avatar.bufflist.Count;
                for (int i = 0; i < count; i++)
                {
                    // 获取当前buffid
                    int buffId = avatar.bufflist[i][2];
                    //IsToolsMain.LogInfo($"当前触发类型为:{type},Buff为:{buffId}");
                    if (jsonData.instance.BuffJsonData.ContainsKey(buffId.ToString()))
                    {
                        bool HasNewTigger = false;
                        //获取玩家buff里面含有379seid的buff
                        foreach (List<int> list2 in avatar.buffmag.getBuffBySeid(379))
                        {
                            //执行完后释放资源
                            using (List<JSONObject>.Enumerator enumerator2 = jsonData.instance.BuffSeidJsonData[379][string.Concat(list2[2])]["value1"].list.GetEnumerator())
                                while (enumerator2.MoveNext())
                                {
                                    // 判断seid里面的buffid是不是现在这个buffid
                                    if ((int)enumerator2.Current.n == buffId)
                                    {
                                        //IsToolsMain.Log($"判断逻辑中，当前触发类型为:{type},Buff为:{buffId}");
                                        // 判断当前触发的数据类型是不是value2的，是的话就触发
                                        if (jsonData.instance.BuffSeidJsonData[379][string.Concat(list2[2])]["value2"].I == type)
                                        {
                                            IsToolsMain.Log($"为{buffId}触发类型为{type}");
                                            HasNewTigger = true;
                                            avatar.spell.onBuffTick(i, new List<int>(), 0);
                                        } 
                                    }
                                }
                        }
                        // 原版的判断触发逻辑，如果没有触发379的逻辑就按照原版的触发
                        if (jsonData.instance.BuffJsonData[buffId.ToString()]["trigger"].I == type && !HasNewTigger)
                        {
                            IsToolsMain.Log($"原版逻辑：为{buffId}触发类型为{type}");
                            avatar.spell.onBuffTick(i, new List<int>(), 0);
                        }
                    }
                    else
                    {
                        IsToolsMain.Error(string.Format("Spell.onBuffTickByType({0})，BuffJsonData不存在buffid{1}", type, buffId));
                    }
                }
                Event.fireOut("UpdataBuff", Array.Empty<object>());
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Spell), "onBuffTickByType", typeof(int), typeof(List<int>))]
    public class onBuffTickByType2Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(int type, List<int> flag, Spell __instance)
        {
            try
            {
                Avatar avatar = (Avatar)__instance.entity;
                BuffMag buffMag = avatar.buffmag;
                // 拥有379seid的buff后进入逻辑
                if (buffMag.HasBuffSeid(379))
                {

                    // 【特殊类型处理】如果触发类型为 35 (推测为"闪避"相关)
                    if (type == 35)
                    {
                        // 设置全局闪避标志为 true
                        RoundManager.instance.IsShanBi = true;
                        // 播放闪避音效
                        MusicMag.instance.PlayEffectMusic("闪避");
                    }
                    // 字典用于记录已处理的 Buff ID 及其索引，防止重复处理
                    Dictionary<int, int> dictionary = new Dictionary<int, int>();
                    // 当前 Buff 列表的总数量
                    int count = avatar.bufflist.Count;

                    // 用于去重的临时列表，存储已遍历的 Buff 数据
                    List<List<int>> list = new List<List<int>>();

                    // 当前处理的 Buff 索引
                    int num = 0;
                    // 循环变量
                    int buff;

                    // 遍历角色身上的所有 Buff
                    for (buff = 0; buff < count; buff++)
                    {
                        // 【容错机制】如果在处理过程中 Buff 数量发生变化（例如 Buff 被添加或移除）
                        // 重置索引和计数，重新开始遍历，防止索引越界或遗漏
                        if (count != avatar.bufflist.Count)
                        {
                            buff = 0;
                            count = avatar.bufflist.Count;
                        }

                        // 【去重检查】如果当前 Buff 已经在本次循环中被处理过，则跳过
                        if (list.FindAll((List<int> a) => a == avatar.bufflist[buff]).Count > 0)
                        {
                            continue;
                        }

                        // 将当前 Buff 加入已处理列表
                        list.Add(avatar.bufflist[buff]);

                        // 新增判定
                        //bool HasNewTigger = false;
                        int newTriggerType = -1;
                        //获取玩家buff里面含有379seid的buff
                        foreach (List<int> list2 in avatar.buffmag.getBuffBySeid(379))
                        {
                            //执行完后释放资源
                            using (List<JSONObject>.Enumerator enumerator2 = jsonData.instance.BuffSeidJsonData[379][string.Concat(list2[2])]["value1"].list.GetEnumerator())
                                while (enumerator2.MoveNext())
                                {
                                    // 判断seid里面的buffid是不是现在这个buffid
                                    if ((int)enumerator2.Current.n == avatar.bufflist[buff][2])
                                    {
                                        newTriggerType = jsonData.instance.BuffSeidJsonData[379][string.Concat(list2[2])]["value2"].I;
                                        //IsToolsMain.Log($"判断逻辑中，当前触发类型为:{type},Buff为:{avatar.bufflist[buff][2]}");
                                        // 判断当前触发的数据类型是不是value2的，是的话就满足条件
                                        //if (newTriggerType == type)
                                        //{
                                        //    IsToolsMain.Log($"为{avatar.bufflist[buff][2]}触发类型为{type}");
                                        //    HasNewTigger = true;
                                        //}
                                    }
                                }
                        }
                        // 如果有新的seid就让新的seid来比较，否则让本来的seid来比较
                        int effectiveTrigger = (newTriggerType != -1) ? newTriggerType : _BuffJsonData.DataDict[avatar.bufflist[buff][2]].trigger;

                        if (effectiveTrigger != type)
                        {
                            continue; // 有效触发类型不匹配，跳出
                        }


                        // 【同 ID 去重】如果字典中已存在此 Buff ID
                        if (dictionary.ContainsKey(avatar.bufflist[buff][2]))
                        {
                            // 如果记录的索引不是当前索引，说明存在重复 Buff，跳过后续处理
                            if (dictionary[avatar.bufflist[buff][2]] != buff)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            // 如果是第一次遇到此 Buff ID，记录下来
                            dictionary.Add(avatar.bufflist[buff][2], buff);
                        }

                        // 记录当前 Buff ID
                        num = avatar.bufflist[buff][2];

                        // 【核心逻辑】调用具体的 Buff 处理方法
                        // 传入索引、标志列表和触发类型
                        avatar.spell.onBuffTick(buff, flag, type);

                        // 【装备特效处理 - 玩家端】如果角色是玩家且拥有特殊的"炼器"装备特效
                        if (avatar.isPlayer() && avatar.fightTemp.LianQiBuffEquipDictionary.Keys.Count > 0 && avatar.fightTemp.LianQiBuffEquipDictionary.ContainsKey(num))
                        {
                            // 获取该 Buff 对应的装备特效列表
                            List<JSONObject> list2 = avatar.fightTemp.LianQiBuffEquipDictionary[num].list;

                            // 遍历所有特效
                            for (int i = 0; i < list2.Count; i++)
                            {
                                // 特效ID 62: 生命值条件判断
                                // 只有当生命值百分比高于设定值时才触发后续效果
                                if (list2[i]["id"].I == 62)
                                {
                                    if ((float)avatar.HP / (float)avatar.HP_Max * 100f > list2[i]["value1"][0].n)
                                    {
                                        break; // 不满足条件，跳出循环
                                    }
                                    continue; // 满足条件，继续处理下一个特效
                                }

                                // 特效ID 1: 伤害/治疗效果
                                if (list2[i]["id"].I == 1)
                                {
                                    for (int j = 0; j < list2[i]["value1"].Count; j++)
                                    {
                                        // 对自身造成伤害 (负数通常代表治疗)
                                        // 参数: 攻击者, 受害者, 技能ID, 伤害值
                                        avatar.recvDamage(avatar, avatar, 18005, -list2[i]["value2"][j].I);
                                    }
                                }

                                // 特效ID 5: 添加 Buff
                                if (list2[i]["id"].I == 5)
                                {
                                    for (int k = 0; k < list2[i]["value1"].Count; k++)
                                    {
                                        // 给自己添加 Buff
                                        avatar.spell.addBuff(list2[i]["value1"][k].I, list2[i]["value2"][k].I);
                                    }
                                }

                                // 特效ID 17: 对目标添加 Buff
                                if (list2[i]["id"].I == 17)
                                {
                                    for (int l = 0; l < list2[i]["value1"].Count; l++)
                                    {
                                        // 给目标 (OtherAvatar) 添加 Buff
                                        avatar.OtherAvatar.spell.addBuff(list2[i]["value1"][l].I, list2[i]["value2"][l].I);
                                    }
                                }
                            }
                        }

                        // 【装备特效处理 - NPC端】处理 NPC 的炼器装备特效
                        // 逻辑与玩家端基本一致，只是数据来源不同
                        if (avatar.isPlayer() ||
                            RoundManager.instance.newNpcFightManager == null ||
                            RoundManager.instance.newNpcFightManager.LianQiBuffEquipDictionary.Keys.Count <= 0 ||
                            !RoundManager.instance.newNpcFightManager.LianQiBuffEquipDictionary.ContainsKey(num))
                        {
                            continue;
                        }

                        List<JSONObject> list3 = RoundManager.instance.newNpcFightManager.LianQiBuffEquipDictionary[num].list;
                        for (int m = 0; m < list3.Count; m++)
                        {
                            if (list3[m]["id"].I == 62)
                            {
                                if ((float)avatar.HP / (float)avatar.HP_Max * 100f > list3[m]["value1"][0].n)
                                {
                                    break;
                                }
                                continue;
                            }

                            if (list3[m]["id"].I == 1)
                            {
                                for (int n = 0; n < list3[m]["value1"].Count; n++)
                                {
                                    avatar.recvDamage(avatar, avatar, 18005, -list3[m]["value2"][n].I);
                                }
                            }

                            if (list3[m]["id"].I == 5)
                            {
                                for (int num2 = 0; num2 < list3[m]["value1"].Count; num2++)
                                {
                                    // 注意：这里使用的是 addDBuff (可能是显示 Buff)
                                    avatar.spell.addDBuff(list3[m]["value1"][num2].I, list3[m]["value2"][num2].I);
                                }
                            }

                            if (list3[m]["id"].I == 17)
                            {
                                for (int num3 = 0; num3 < list3[m]["value1"].Count; num3++)
                                {
                                    // 注意：这里使用的是 addDBuff (可能是显示 Buff)
                                    avatar.OtherAvatar.spell.addDBuff(list3[m]["value1"][num3].I, list3[m]["value2"][num3].I);
                                }
                            }
                        }
                    }

                    // 触发事件：通知 UI 或其他模块 Buff 状态已更新
                    Event.fireOut("UpdataBuff");
                    return false;
                }
                return true;
            } catch (Exception arg)
            {
                IsToolsMain.Error($"{arg}");
                return true;
            }
        }
   }
}
