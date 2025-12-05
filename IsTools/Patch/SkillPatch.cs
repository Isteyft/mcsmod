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
//  [HarmonyPatch(typeof(GUIPackage.Skill))]
//  public class SkillPatch
//  {

//      // 用于保存原始的AttackType，使用SkillID作为键
//      private static Dictionary<int, List<int>> OriginalAttackTypes = new Dictionary<int, List<int>>();
//      [HarmonyPatch("Puting")]
//      [HarmonyPrefix]
//      public static void Puting_Prefix(Entity _attaker, Entity _receiver, GUIPackage.Skill __instance, int type = 0, string uuid = "")
//      {
//            // 转换实体类型为Avatar
//            KBEngine.Avatar attaker = (KBEngine.Avatar)_attaker;
//            // _skillJsonData skillJsonData = _skillJsonData.DataDict[__instance.SkillID];
//            JSONObject attackTypeList = jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"];

//            // 保存原始的AttackType
//            int skillId = __instance.SkillID;
//            if (!OriginalAttackTypes.ContainsKey(skillId))
//            {
//                OriginalAttackTypes[skillId] = new List<int>(jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].ToList());
//            }
//            attackTypeList.ToList().ForEach(e => IsToolsMain.Log("初始属性" + e));

//            // 判断玩家是否拥有添加额外属性的buffseid
//            if (attaker.buffmag.HasBuffSeid(373))
//          {
//              // 获取玩家buff里面含有373seid的buff
//              foreach (List<int> list2 in attaker.buffmag.getBuffBySeid(373))
//              {
//                  // 执行完后释放资源
//                  using (List<JSONObject>.Enumerator enumerator2 = jsonData.instance.BuffSeidJsonData[373][string.Concat(list2[2])]["value1"].list.GetEnumerator())
//                      while (enumerator2.MoveNext())
//                      {
//                          int value1 = (int)enumerator2.Current.n;
//                          if (!attackTypeList.ToList().Contains(value1))
//                          {
//                                IsToolsMain.LogInfo("add属性："+value1);
//                                // skillJsonData.AttackType.Add(value1);
//                                jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].Add(value1);
//                          }
//                      }
//                    jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].ToList().ForEach(e => IsToolsMain.LogInfo("增加属性"+e));
//              }
//          }
//      }
//      [HarmonyPatch("Puting")]
//      [HarmonyPostfix]
//      public static void Puting_Postfix(Entity _attaker, Entity _receiver, GUIPackage.Skill __instance, int type = 0, string uuid = "")
//      {
//          // 恢复原始的AttackType
//          int skillId = __instance.SkillID;
//          if (OriginalAttackTypes.ContainsKey(skillId))
//          {
//                JSONObject attackTypeList = jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)];
//                jsonData.instance.skillJsonData[string.Concat(__instance.SkillID)]["AttackType"].ToList().ForEach(e => IsToolsMain.LogInfo("设置为初始属性" + e));
//                attackTypeList.SetField("AttackType", OriginalAttackTypes[skillId].ToString());
                
//              // 清理字典
//                OriginalAttackTypes.Remove(skillId);
//          }
//      }
//  }
//    [HarmonyPatch(typeof(KBEngine.Buff))]
//    public class BuffPatch
//    {
//        [HarmonyPrefix]
//        [HarmonyPatch("ListRealizeSeid72")]
//        private static void ListRealizeSeid72(int seid, KBEngine.Avatar avatar, List<int> buffInfo, List<int> flag, KBEngine.Buff __instance)
//        {
//            int num = flag[1];
//            // 遍历输出 AttackType
//            JSONObject attackTypeList = jsonData.instance.skillJsonData[string.Concat(num)]["AttackType"];
//            for (int i = 0; i < attackTypeList.Count; i++)
//            {
//                IsToolsMain.LogInfo("抵挡属性" + attackTypeList[i].n);
//            }
//        }
//    }
//}
