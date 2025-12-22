using Fungus;
using Google.Protobuf.WellKnownTypes;
using HarmonyLib;
using JSONClass;
using KBEngine;
using PaiMai;
using SkySwordKill.Next;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Lua;
using SkySwordKill.NextMoreCommand.Utils;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine.Networking.Types;
using XLua;
using System.Timers;
using YSGame;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.Patch.SeidPatch
{
    [HarmonyPatch(typeof(Buff))]
    public class BuffSeidPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("loopRealizeSeid")]
        public static bool Prefix_loopRealizeSeid_AddSeid(
            int seid,
            Entity _avatar,
            List<int> buffInfo,
            List<int> flag,
            Buff __instance)
        {
            var avatar = (Avatar)_avatar;

            switch (seid)
            {
                case 360:
                    ListRealizeSeid360(seid, avatar, flag, __instance);
                    return false;
                case 361:
                    ListRealizeSeid361(seid, avatar, __instance);
                    return false;
                case 362:
                    ListRealizeSeid362(seid, avatar, __instance);
                    return false;
                case 363:
                    ListRealizeSeid363(seid, avatar, flag, __instance);
                    return false;
                case 364:
                    ListRealizeSeid364(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 365:
                    ListRealizeSeid365(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 366:
                    ListRealizeSeid366(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 368:
                    ListRealizeSeid368(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 369:
                    ListRealizeSeid369(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 370:
                    ListRealizeSeid370(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 371:
                    ListRealizeSeid371(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 372:
                    ListRealizeSeid372(seid, avatar, buffInfo, flag, __instance);
                    return false;
                case 373:
                    ListRealizeSeid373(seid, avatar, buffInfo, flag, __instance);
                    return false;
                default:
                    return true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("CanRealizeSeid")]
        public static bool CanRealizeSeid_Prefix(ref bool __result, Buff __instance, Avatar _avatar, List<int> flag, int nowSeid, BuffLoopData buffLoopData = null, List<int> buffInfo = null)
        {
            if (nowSeid != 367) return true;
            switch (nowSeid)
            {
                case 367:
                {
                        try
                        {
                            JSONObject seidJson = __instance.getSeidJson(nowSeid);
                            string fileName = seidJson["value1"].Str;
                            string funcName = seidJson["value2"].Str;
                            if (_avatar != PlayerEx.Player)
                            {
                                if (seidJson.HasField("value3") && seidJson.HasField("value4"))
                                {
                                    fileName = seidJson["value3"].Str;
                                    funcName = seidJson["value4"].Str;
                                }
                            }
                            DialogEnvironment env = new DialogEnvironment();
                            LuaEnv luaEnv = Main.Lua.LuaEnv;
                            // 2. 创建 luaUtil 类的实例，并传入 LuaEnv
                            var luaTool = new luaUtil(luaEnv);
                            // 假设 DialogCommand 有公共构造函数
                            // 直接指定指令头和参数（跳过解析）
                            var command = new DialogCommand(
                                commandHead: "RunLua",             // 指令类型
                                paramList: new[] { fileName, funcName },   // 参数数组
                                bindEventData: null,            // 关联数据（可选）
                                isEnd: false                    // 是否结束对话
                            );
                            // 3. 调用实例方法 RunFunc
                            object[] o = luaTool.RunFuncHasResult("lib/dialogResult", "runEventResult", new object[]
                            {
                                fileName,
                                funcName,
                                command,
                                env,
                                //new Action(() => { }),
                                null
                        });
                            if (!(bool)o[0])
                            {
                                return false; //不继续执行
                            }
                            return true;
                        }
                        catch {
                            IsToolsMain.LogInfo("lua返回值出错!不继续执行逻辑!");
                            return false; 
                        }
                    }
                default:
                    return true;
            }
        }

            // <summary>
            /// SEID 360 修改实现：随机从buff数组中获得相应的buff数量
            /// </summary>
        private static void ListRealizeSeid360(int seid, Avatar avatar, IReadOnlyList<int> flag, Buff instance)
        {
            // 从配置获取参数
            List<int> buffIds = instance.getSeidJson(seid).GetFieldList("value1");    // Buff ID列表
            List<int> buffCounts = instance.getSeidJson(seid).GetFieldList("value2"); // Buff层数列表
            // 参数校验
            if (buffIds.Count == 0 || buffCounts.Count == 0 || buffIds.Count != buffCounts.Count)
            {
                return;
            }
            // 创建随机数生成器
            System.Random random = new System.Random();
            // 随机选择一个buff索引
            int randomIndex = random.Next(0, buffIds.Count);
            // 应用随机选择的buff
            avatar.spell.addBuff(buffIds[randomIndex], buffCounts[randomIndex]);
        }
        //去掉所有y灵气
        private static void ListRealizeSeid361(int seid, Avatar avatar, Buff instance)
        {
            int i = instance.getSeidJson(seid)["value1"].I;
            RoundManager.instance.removeCard(avatar, avatar.crystal[i], i);
        }
        //去掉x点y灵气
        private static void ListRealizeSeid362(int seid, Avatar avatar, Buff instance)
        {
            JSONObject seidJson = instance.getSeidJson(seid);
            int i = seidJson["value1"].I;
            int i2 = seidJson["value2"].I;
            RoundManager.instance.removeCard(avatar, i, i2);
        }
        private static void ListRealizeSeid363(int seid, Avatar avatar, IReadOnlyList<int> flag, Buff instance)
        {
            // 从配置获取参数
            int buffId1 = instance.getSeidJson(seid).GetFieldInt("value1");    // Buff ID列表
            List<int> buffList1 = instance.getSeidJson(seid).GetFieldList("value2"); // Buff层数列表
            List<int> buffCounts1 = instance.getSeidJson(seid).GetFieldList("value3"); // Buff层数列表
            int buffId2 = instance.getSeidJson(seid).GetFieldInt("value4");    // Buff ID列表
            List<int> buffList2 = instance.getSeidJson(seid).GetFieldList("value5"); // Buff层数列表
            List<int> buffCounts2 = instance.getSeidJson(seid).GetFieldList("value6"); // Buff层数列表
            BuffMag buffMag = avatar.buffmag;
            // 参数校验
            if (buffMag.HasBuff(buffId1))
            {
                for (int i = 0; i < buffList1.Count; i++)
                {
                    avatar.spell.addBuff(buffList1[i], buffCounts1[i]);
                }
            }
            else if (buffMag.HasBuff(buffId2))
            {
                for (int i = 0; i < buffList2.Count; i++)
                {
                    avatar.spell.addBuff(buffList2[i], buffCounts2[i]);
                }
            }
        }
        /// <summary>
        /// SEID 364 拥有全部bufflist1后获得bufflist2
        /// </summary>
        private static void ListRealizeSeid364(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            // 从配置获取参数
            List<int> buffList1 = instance.getSeidJson(seid).GetFieldList("value1"); // 需要检查的Buff列表
            List<int> buffCounts1 = instance.getSeidJson(seid).GetFieldList("value2"); // Buff层数列表
            List<int> buffList2 = instance.getSeidJson(seid).GetFieldList("value3"); // 全部满足后获得的Buff列表
            List<int> buffCounts2 = instance.getSeidJson(seid).GetFieldList("value4"); // Buff层数列表

            BuffMag buffMag = avatar.buffmag;

            // 检查是否拥有全部buffList1中的buff
            bool hasAllBuffs = true;
            List<int> missingBuffs = new List<int>();
            List<int> missingBuffCounts = new List<int>();

            for (int i = 0; i < buffList1.Count; i++)
            {
                if (!buffMag.HasBuff(buffList1[i]))
                {
                    hasAllBuffs = false;
                    missingBuffs.Add(buffList1[i]);
                    missingBuffCounts.Add(buffCounts1[i]);
                }
            }

            if (!hasAllBuffs)
            {
                // 随机获取一个缺失的buff
                if (missingBuffs.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, missingBuffs.Count);
                    avatar.spell.addBuff(missingBuffs[randomIndex], missingBuffCounts[randomIndex] * buffInfo[1]);
                }
            }
            else
            {
                // 拥有全部buffList1，添加所有buffList2中的buff
                for (int i = 0; i < buffList2.Count; i++)
                {
                    avatar.spell.addBuff(buffList2[i], buffCounts2[i]);
                }
            }
        }
        //运行next命令
        private static void ListRealizeSeid365(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            JSONObject seidJson = instance.getSeidJson(seid);
            string str = seidJson["value1"].Str;
            string str2 = str;
            if (seidJson.HasField("value2"))
            {
                if (seidJson["value2"].Str != "" && seidJson["value2"].Str != null)
                {
                    str2 = seidJson["value2"].Str;
                }
            }
            if (avatar == PlayerEx.Player)
            {
                DialogAnalysis.StartTestDialogEvent(str, null);
            }
            else
            {
                DialogAnalysis.StartTestDialogEvent(str2, null);
            }
        }

        //buff护盾，基本没用，全靠buff的patch
        private static void ListRealizeSeid366(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
        }
        // 释放上一次技能
        private static void ListRealizeSeid368(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            int id = flag[1];
            if (RoundManager.instance.NowSkillUsedLingQiSum > 0)
            {
                GUIPackage.Skill skill = new GUIPackage.Skill(id, 0, 10);
                bool attackType = "SkillAttack".Equals(_skillJsonData.DataDict[id].script);
                Tools.AddQueue(delegate
                {
                    RoundManager.instance.NowUseLingQiType = UseLingQiType.释放技能后消耗;
                    skill.PutingSkill(avatar, attackType ? avatar.OtherAvatar : avatar, 0);
                    RoundManager.instance.NowUseLingQiType = UseLingQiType.None;
                    YSFuncList.Ints.Continue();
                });
            }
        }
        //运行next命令

        private static void ListRealizeSeid369(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            int i = instance.getSeidJson(seid)["value1"].I;
            bool isPlayer = AvatarUtils.isPlayer(avatar);
            var isAvatarSkl = FaXiangUtils.isAvatarSkl(i);
            // 获取当前玩家对象
            UnityEngine.GameObject avatar10 = FaXiangUtils.GetAvatarSklPosition(isPlayer);
            // 实例化预制体
            UnityEngine.GameObject avatarInstance = UnityEngine.Object.Instantiate(FaXiangUtils.GetFaXiangPrefab(i));

            avatarInstance.transform.SetParent(avatar10.transform);

            FaXiangUtils.InitFaXiangPosition(avatarInstance, isPlayer, 0);

            avatarInstance.transform.SetAsFirstSibling();
            if (isAvatarSkl)
            {
                FaXiangUtils.InitAvatarSklFace(avatarInstance, i);
            }

        }

        // 按照结算伤害获得buff
        private static void ListRealizeSeid370(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            JSONObject seidJson = instance.getSeidJson(seid);
            int buffId = seidJson["value1"].I;    // Buff ID列表
            int num = (int)((double)flag[0] / 100.0 * (double)seidJson["value2"].I * (double)buffInfo[1]);
            avatar.spell.addBuff(buffId, num);
        }
        // 第一个是根据治疗量获得buff
        //每获得x点治疗获得y层buff地形式
        private static void ListRealizeSeid371(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            JSONObject seidJson = instance.getSeidJson(seid);
            List<int> buffIds = seidJson.GetFieldList("value1");    // Buff ID列表
            List<int> buffCounts = seidJson.GetFieldList("value2");    // Buff数量列表
            int count = seidJson.GetField("value3").I;    // 每多少
            int num = flag[0];      //治疗量
            
            // 遍历buffIds和buffCounts列表，为每个Buff ID添加对应的数量
            for (int i = 0; i < buffIds.Count && i < buffCounts.Count; i++)
            {
                int buffId = buffIds[i];
                int buffCount = buffCounts[i];
                int num2 = 0;
                if (num / count > 1)
                {
                    num2 = -(num / count) * buffCount * buffInfo[1];  // 治疗量 x buff数量 x 目前buff层数
                   // IsToolsMain.LogInfo($"371, {buffId}, {num2}");
                    avatar.spell.addBuff(buffId, num2);
                }
            }
        }
        // 神剑诀plus
        private static void ListRealizeSeid372(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            JSONObject seidJson = instance.getSeidJson(seid);
            //int num = flag[0];
            // 目标0
            Avatar targetAvatar = instance.getTargetAvatar(seid, avatar);
            // X Buff ID  真伤 目标
            int i = seidJson["value1"].I;    
            // 目标1
            Avatar target1 = avatar;
            if (seidJson["target1"].I != 1)
            {
                target1 = avatar.OtherAvatar;
            }
            // 目标1的buff数量  每拥有x层buff
            int buffSum = target1.buffmag.GetBuffSum(seidJson["value2"].I);
            int i2 = seidJson["value3"].I;    // 层数
            targetAvatar.spell.addBuff(i, buffSum + i2);
            // 【剑气】将在触发后被移除，但每次造成剑系技能伤害时，额外使目标受到等同于其【易伤】层数+14点真实伤害。
        }
        // 增加释放技能的属性
        private static void ListRealizeSeid373(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
            JSONObject seidJson = instance.getSeidJson(seid);
            int buffType = seidJson["value1"].I;
            var buffLists = avatar.buffmag.GetAllBuffByType(buffType);

            // 提取每个子数组的最后一位数
            List<int> lastNumbers = buffLists
                .Where(subList => subList.Count > 0) // 过滤空列表
                .Select(subList => subList.Last())   // 取最后一个元素
                .ToList();
            int i = seidJson["value2"].I;
            
            // 定义需要额外触发onBuffTick的buffid数组（移到循环外部，避免重复创建）
            int[] extraTriggerBuffIds = new int[] { 250000, 250003 };
            
            int num;
            foreach (int i2 in lastNumbers)
            {
                num = i * buffInfo[1];
                for (int j = 0; j < num; j++)
                {
                    foreach (List<int> item in avatar.OtherAvatar.buffmag.getBuffByID(i2))
                    {
                        avatar.OtherAvatar.spell.onBuffTick(avatar.OtherAvatar.bufflist.IndexOf(item));
                        // 当i2在extraTriggerBuffIds数组中时，额外触发一次onBuffTick
                        if (Array.Exists(extraTriggerBuffIds, id => id == i2))
                        {
                            avatar.OtherAvatar.spell.onBuffTick(avatar.OtherAvatar.bufflist.IndexOf(item));
                        }
                    }
                }
            }

        }
        // 增加释放技能的属性
        //private static void ListRealizeSeid373(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        //{
        //}
    }
}