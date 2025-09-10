using Fungus;
using HarmonyLib;
using JSONClass;
using KBEngine;
using PaiMai;
using SkySwordKill.Next;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Lua;
using SkySwordKill.NextMoreCommand.Custom.RealizeSeid;
using System;
using System.Collections.Generic;
using top.Isteyft.MCS.IsTools.Util;
using XLua;

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
                            string fileName = __instance.getSeidJson(nowSeid)["value1"].Str;
                            string funcName = __instance.getSeidJson(nowSeid)["value2"].Str;
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
                case 368:
                {
                    int key = flag[1];
                    List<JSONObject> list = __instance.getSeidJson(nowSeid)["value1"].list;
                    foreach (JSONObject __i2 in list)
                    {
                        if (_skillJsonData.DataDict[key].AttackType.FindAll((int a) => a == __i2.I).Count > 0)
                        {
                            // 找到匹配项，是不符合条件的属性
                            return false; // 不触发后续
                        }
                    }
                        return true;
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
            string str = instance.getSeidJson(seid)["value1"].Str;
            DialogAnalysis.StartTestDialogEvent(str, null);
        }

        //buff护盾，基本没用，全靠buff的patch
        private static void ListRealizeSeid366(int seid, Avatar avatar, List<int> buffInfo, IReadOnlyList<int> flag, Buff instance)
        {
        }
    }
}