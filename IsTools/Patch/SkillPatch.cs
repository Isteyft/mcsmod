//using Google.Protobuf.WellKnownTypes;
//using HarmonyLib;
//using JSONClass;
//using KBEngine;
//using SkySwordKill.NextModEditor.Mod;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace top.Isteyft.MCS.IsTools.Patch
//{
//    [HarmonyPatch(typeof(GUIPackage.Skill))]
//    public class SkillPatch
//    {

//        // 用于保存原始的AttackType，使用SkillID作为键
//        private static Dictionary<int, List<int>> OriginalAttackTypes = new Dictionary<int, List<int>>();
//        [HarmonyPatch("Puting")]
//        [HarmonyPrefix]
//        public static void Puting_Prefix(Entity _attaker, Entity _receiver, GUIPackage.Skill __instance, int type = 0, string uuid = "")
//        {
//            // 调试日志：记录函数调用信息
//            int skillId = __instance.SkillID;
//            IsToolsMain.LogInfo($"[SkillPatch] Puting_Prefix 被调用 - 技能ID: {skillId}, 类型: {type}, UUID: {uuid}");
            
//            // 转换实体类型为Avatar
//            KBEngine.Avatar attaker = (KBEngine.Avatar)_attaker;
//            IsToolsMain.LogInfo($"[SkillPatch] 攻击者: {attaker.name} (ID: {attaker.id}), 接收者:");
            
//            // _skillJsonData skillJsonData = _skillJsonData.DataDict[__instance.SkillID];
//            JSONObject attackTypeList = jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"];
//            IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} 初始AttackType数量: {attackTypeList.Count}");

//            // 保存原始的AttackType
//            if (!OriginalAttackTypes.ContainsKey(skillId))
//            {
//                List<int> originalTypes = new List<int>(jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].ToList());
//                OriginalAttackTypes[skillId] = originalTypes;
//                IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 保存原始AttackType: [{string.Join(", ", originalTypes)}]");
//            }
//            else
//            {
//                IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 原始AttackType已存在于缓存中");
//            }
            
//            // 记录初始属性
//            List<int> initialTypes = attackTypeList.ToList();
//            IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 初始AttackType: [{string.Join(", ", initialTypes)}]");

//            // 判断玩家是否拥有添加额外属性的buffseid
//            bool hasBuffSeid373 = attaker.buffmag.HasBuffSeid(373);
//            IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 玩家是否拥有buffseid 373: {hasBuffSeid373}");
            
//            if (hasBuffSeid373)
//            {
//                // 获取玩家buff里面含有373seid的buff
//                List<List<int>> buffList = attaker.buffmag.getBuffBySeid(373).ToList();
//                IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 找到 {buffList.Count} 个含有seid 373的buff");
                
//                foreach (List<int> list2 in buffList)
//                {
//                    // 记录buff详情
//                    IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 处理buff: [{string.Join(", ", list2)}]");
                    
//                    // 执行完后释放资源
//                    JSONObject value1List = jsonData.instance.BuffSeidJsonData[373][string.Concat(list2[2])]["value1"];
//                    IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 从buff中获取到 {value1List.Count} 个额外属性值");
                    
//                    using (List<JSONObject>.Enumerator enumerator2 = value1List.list.GetEnumerator())
//                    {
//                        int addedCount = 0;
//                        while (enumerator2.MoveNext())
//                        {
//                            int value1 = (int)enumerator2.Current.n;
//                            List<int> currentTypes = attackTypeList.ToList();
                            
//                            if (!currentTypes.Contains(value1))
//                            {
//                                IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 添加额外属性: {value1}");
//                                // skillJsonData.AttackType.Add(value1);
//                                jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].Add(value1);
//                                addedCount++;
//                            }
//                            else
//                            {
//                                IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 属性 {value1} 已存在，跳过添加");
//                            }
//                        }
                        
//                        IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 本次buff处理共添加了 {addedCount} 个新属性");
//                    }
                    
//                    // 记录增加属性后的结果
//                    List<int> updatedTypes = jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].ToList();
//                    IsToolsMain.LogInfo($"[SkillPatch] 技能 {skillId} - 更新后的AttackType: [{string.Join(", ", updatedTypes)}]");
//                }
//            }
//        }
//        [HarmonyPatch("Puting")]
//        [HarmonyPostfix]
//        public static void Puting_Postfix(Entity _attaker, Entity _receiver, GUIPackage.Skill __instance, int type = 0, string uuid = "")
//        {
//            // 恢复原始的AttackType
//            int skillId = __instance.SkillID;
//            if (OriginalAttackTypes.ContainsKey(skillId))
//            {
//                JSONObject attackTypeList = jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)];
//                jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].ToList().ForEach(e => IsToolsMain.LogInfo("设置为初始属性" + e));
//                attackTypeList.SetField("AttackType", OriginalAttackTypes[skillId].ToString());

//                // 清理字典
//                OriginalAttackTypes.Remove(skillId);
//            }
//        }
//    }
//    //[HarmonyPatch(typeof(KBEngine.Buff))]
//    //public class BuffPatch
//    //{
//    //    [HarmonyPrefix]
//    //    [HarmonyPatch("ListRealizeSeid72")]
//    //    private static void ListRealizeSeid72(int seid, KBEngine.Avatar avatar, List<int> buffInfo, List<int> flag, KBEngine.Buff __instance)
//    //    {
//    //        int num = flag[1];
//    //        // 遍历输出 AttackType
//    //        JSONObject attackTypeList = jsonData.instance.skillJsonData[string.Concat(num)]["AttackType"];
//    //        for (int i = 0; i < attackTypeList.Count; i++)
//    //        {
//    //            IsToolsMain.LogInfo("抵挡属性" + attackTypeList[i].n);
//    //        }
//    //    }
//    //}
//}
