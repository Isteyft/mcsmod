using GUIPackage;
using HarmonyLib;
using KBEngine;
using SkySwordKill.Next;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine.Networking.Types;
using WXB;
using XLua;

namespace top.Isteyft.MCS.IsTools.Patch.SeidPatch
{
    [HarmonyPatch(typeof(GUIPackage.Skill))]
    internal class SkillSeidPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("realizeSeid")]
        public static bool Prefix_realizeSeid_AddSeid(GUIPackage.Skill __instance, int seid, Entity _attaker, List<int> damage, Entity _receiver, int type)
        {
            Avatar attaker = (Avatar)_attaker;
            Avatar receiver = (Avatar)_receiver;
            switch (seid)
            {
                case 360:
                    realizeSeid360(__instance, seid, attaker, damage, receiver);
                    return false;
                case 361:
                    realizeSeid361(__instance, seid, attaker, damage, receiver);
                    return false;
                case 362:
                    realizeSeid362(__instance, seid, attaker, damage, receiver);
                    return false;
                case 363:
                    realizeSeid363(__instance, seid, attaker, damage, receiver);
                    return false;
                case 364:
                    realizeSeid364(__instance, seid, attaker, damage, receiver);
                    return false;
                case 365:
                    realizeSeid365(__instance, seid, attaker, damage, receiver);
                    return false;
                case 366:
                    realizeSeid366(__instance, seid, attaker, damage, receiver);
                    return false;
                case 367:
                    realizeSeid367(__instance, seid, attaker, damage, receiver);
                    return false;
                case 368:
                    realizeSeid368(__instance, seid, attaker, damage, receiver);
                    return false;
                default:
                    return true;
            }
        }
        private static void realizeSeid360(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            // 获取seid对应的JSON配置
            JSONObject seidJson = __instance.getSeidJson(seid);

            // 获取三个配置数组
            JSONObject buffIds = seidJson["value1"];         // buff ID数组
            JSONObject requiredLayers = seidJson["value2"]; // 每多少层触发一次 (Y)
            JSONObject damageAdds = seidJson["value3"];     // 每次触发增加的伤害 (Z)
                                                            // 新增value4处理，默认为0（自身）
            int target = 1;
            if (seidJson.HasField("value4"))
            {
                target = seidJson["value4"].I;
            }

            // 初始总伤害
            int totalDamage = damage[0];

            // 确保三个数组长度一致
            if (buffIds.Count != requiredLayers.Count || buffIds.Count != damageAdds.Count)
            {
                IsToolsMain.Error($"配置数组长度不一致！buffIds:{buffIds.Count}, requiredLayers:{requiredLayers.Count}, damageAdds:{damageAdds.Count}");
                return;
            }

            // 遍历每个buff配置
            for (int i = 0; i < buffIds.Count; i++)
            {
                // 获取当前配置项
                int buffId = buffIds[i].I;
                int triggerLayer = requiredLayers[i].I;  // 每Y层触发一次
                int damageAdd = damageAdds[i].I;        // 每次增加Z伤害
                int currentLayer;
                if (target < 2)
                {
                    // 获取攻击者当前buff总层数
                    currentLayer = attaker.buffmag.GetBuffSum(buffId);
                }
                else
                {
                    currentLayer = receiver.buffmag.GetBuffSum(buffId);
                }


                // 计算可以触发多少次 (currentLayer / Y)
                int triggerTimes = currentLayer / triggerLayer;

                if (triggerTimes > 0)
                {
                    // 增加总伤害 = 触发次数 × 每次增加的伤害
                    totalDamage += triggerTimes * damageAdd;

                    //IsToolsMain.Log($"buff效果触发: ID={buffId}, 当前层数={currentLayer}, " +
                    //                   $"每{triggerLayer}层增加{damageAdd}伤害, 触发{triggerTimes}次, " +
                    //                   $"总增加{triggerTimes * damageAdd}伤害");
                }
            }

            // 更新最终伤害值
            damage[0] = totalDamage;
        }
        private static void realizeSeid361(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            // 获取seid对应的JSON配置
            JSONObject seidJson = __instance.getSeidJson(seid);

            // 获取三个配置数组
            JSONObject buffIds = seidJson["value1"];         // buff ID数组
            JSONObject requiredLayers = seidJson["value2"]; // 每多少层触发一次 (Y)
            JSONObject damageAdds = seidJson["value3"];    // 每次触发增加的伤害 (Z)

            // 初始总伤害
            int totalDamage = damage[0];

            // 确保三个数组长度一致
            if (buffIds.Count != requiredLayers.Count || buffIds.Count != damageAdds.Count)
            {
                IsToolsMain.Error($"配置数组长度不一致！buffIds:{buffIds.Count}, requiredLayers:{requiredLayers.Count}, damageAdds:{damageAdds.Count}");
                return;
            }

            // 遍历每个buff配置
            for (int i = 0; i < buffIds.Count; i++)
            {
                // 获取当前配置项
                int buffId = buffIds[i].I;
                int triggerLayer = requiredLayers[i].I;  // 每Y层触发一次
                int damageAdd = damageAdds[i].I;        // 每次增加Z伤害

                // 获取攻击者当前buff总层数
                int currentLayer = attaker.buffmag.GetBuffSum(buffId);

                // 计算可以触发多少次 (currentLayer / Y)
                int triggerTimes = currentLayer / triggerLayer;

                if (triggerTimes > 0)
                {
                    // 增加总伤害
                    totalDamage += triggerTimes * damageAdd;

                    // 消耗总层数 (triggerTimes * Y)
                    int totalConsume = triggerTimes * triggerLayer;

                    // 获取所有该buff的实例
                    List<List<int>> buffInstances = attaker.buffmag.getBuffByID(buffId);

                    // 从最新的buff开始消耗
                    int remainingToConsume = totalConsume;
                    for (int j = buffInstances.Count - 1; j >= 0 && remainingToConsume > 0; j--)
                    {
                        List<int> buffInstance = buffInstances[j];
                        int buffRounds = buffInstance[1]; // 剩余回合数

                        if (buffRounds >= remainingToConsume)
                        {
                            // 消耗部分层数
                            buffInstance[1] -= remainingToConsume;
                            remainingToConsume = 0;

                            // 如果回合数减到0，移除buff
                            if (buffInstance[1] <= 0)
                            {
                                attaker.spell.removeBuff(buffInstance);
                            }
                        }
                        else
                        {
                            // 消耗整个buff实例
                            remainingToConsume -= buffRounds;
                            attaker.spell.removeBuff(buffInstance);
                        }
                    }

                    //IsToolsMain.Log($"buff效果触发: ID={buffId}, 当前层数={currentLayer}, 每{triggerLayer}层增加{damageAdd}伤害, 触发{triggerTimes}次, 总消耗{totalConsume}层");
                }
            }

            // 更新最终伤害值
            damage[0] = totalDamage;
        }
        private static void realizeSeid362(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            // 获取技能配置中的value1值
            int value1 = __instance.getSeidJson(seid)["value1"].I;

            // 计算原始伤害
            int originalDamage = damage[0];

            // 计算加成伤害：value1/100 * 原始伤害（使用浮点运算避免整数除法问题）
            float damageMultiplier = value1 / 100f;
            int bonusDamage = (int)(originalDamage * damageMultiplier);

            // 计算最终伤害
            int totalDamage = originalDamage + bonusDamage;

            // 更新伤害值
            damage[0] = totalDamage;
        }

        private static void realizeSeid363(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            JSONObject seidJson = __instance.getSeidJson(seid);
            string str = seidJson["value1"].Str;
            string str2 = str;
            if (seidJson.HasField("value2"))
            {
                if (seidJson["value2"].Str != "" && seidJson["value2"].Str != null)
                {
                    str2 = seidJson["value2"].Str;
                }
            }
            if (attaker == PlayerEx.Player)
            {
                DialogAnalysis.StartTestDialogEvent(str, null);
            }
            else
            {
                DialogAnalysis.StartTestDialogEvent(str2, null);
            }

        }
        private static void realizeSeid364(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            try
            {
                JSONObject seidJson = __instance.getSeidJson(seid);
                string fileName = seidJson["value1"].Str;
                string funcName = seidJson["value2"].Str;
                if (attaker != PlayerEx.Player)
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
                    damage[2] = 1; //不继续执行
                }
            }
            catch
            {
                IsToolsMain.LogInfo("lua返回值出错!不继续执行逻辑!");
                damage[2] = 1;
            }
        }

        private static void realizeSeid365(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            JSONObject seidJson = __instance.getSeidJson(seid);
            string fileName = seidJson["value1"].Str;
            string funcName = seidJson["value2"].Str;
            if (attaker != PlayerEx.Player)
            {
                if (seidJson.HasField("value3") && seidJson.HasField("value4"))
                {
                    fileName = seidJson["value3"].Str;
                    funcName = seidJson["value4"].Str;
                }
            }
            int totalDamage = damage[0];
            try
            {
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
                //IsToolsMain.LogInfo(o[0].ToString());
                //Type valueType = o[0].GetType();
                //IsToolsMain.LogInfo($"具体类型: {valueType.Name}");
                damage[0] = totalDamage + +Convert.ToInt32(o[0]);
            }
            catch
            {
                IsToolsMain.LogInfo("lua返回值出错!");
            }
        }
        private static void realizeSeid366(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            int i = __instance.getSeidJson(seid)["value1"].I;

            // 生成 1-100 的随机数
            int randomValue = new System.Random().Next(1, 101);

            // 如果随机数 <= i，则触发效果（damage[2] = 1）
            if (randomValue <= i)
            {
                {
                    damage[2] = 1;
                }
            }
        }

        private static void realizeSeid367(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            JSONObject seidJson = __instance.getSeidJson(seid);
            string fileName = seidJson["value1"].Str;
            string funcName = seidJson["value2"].Str;
            if (attaker != PlayerEx.Player)
            {
                if (seidJson.HasField("value3") && seidJson.HasField("value4"))
                {
                    fileName = seidJson["value3"].Str;
                    funcName = seidJson["value4"].Str;
                }
            }
            int totalDamage = damage[0];
            try
            {
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
                //IsToolsMain.LogInfo(o[0].ToString());
                //Type valueType = o[0].GetType();
                //IsToolsMain.LogInfo($"具体类型: {valueType.Name}");
                damage[0] = totalDamage + (totalDamage + Convert.ToInt32(o[0]))/100;
            }
            catch
            {
                IsToolsMain.LogInfo("lua返回值出错!");
            }
        }

        private static void realizeSeid368(GUIPackage.Skill __instance, int seid, Avatar attaker, List<int> damage, Avatar receiver)
        {
            JSONObject seidJson = __instance.getSeidJson(seid);
            string fileName = seidJson["value1"].Str;
            string funcName = seidJson["value2"].Str;
            if (attaker != PlayerEx.Player)
            {
                if (seidJson.HasField("value3") && seidJson.HasField("value4"))
                {
                    fileName = seidJson["value3"].Str;
                    funcName = seidJson["value4"].Str;
                }
            }
            int totalDamage = damage[0];
            try
            {
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
                //IsToolsMain.LogInfo(o[0].ToString());
                //Type valueType = o[0].GetType();
                //IsToolsMain.LogInfo($"具体类型: {valueType.Name}");
                int count = Convert.ToInt32(o[0]);
                int skillId = seidJson["value5"].I;  //神通ID
                int skillDamage = seidJson["value6"].I;  //技能伤害
                for (int j = 0; j < count; j++)
                {
                    if (__instance.LateDamages == null)
                    {
                        __instance.LateDamages = new List<LateDamage>();
                    }
                    if (__instance.skill_ID == 12508)
                    {
                        int buffSum = attaker.buffmag.GetBuffSum(skillDamage);
                        __instance.LateDamages.Add(new LateDamage
                        {
                            SkillId = skillId,
                            Damage = buffSum
                        });
                    }
                    else if (__instance.SkillID == 12513)
                    {
                        __instance.LateDamages.Add(new LateDamage
                        {
                            SkillId = skillId,
                            Damage = DuanTiFightManager.Inst.LeiDamage
                        });
                    }
                    else if (__instance.SkillID == 1170)
                    {
                        int damage2 = GlobalValue.Get(skillDamage, "unknow");
                        __instance.LateDamages.Add(new LateDamage
                        {
                            SkillId = skillId,
                            Damage = damage2
                        });
                    }
                    else
                    {
                        __instance.LateDamages.Add(new LateDamage
                        {
                            SkillId = skillId,
                            Damage = skillDamage
                        });
                    }
                }
            }
            catch
            {
                IsToolsMain.LogInfo("lua返回值出错!");
            }
        }
    }
}
