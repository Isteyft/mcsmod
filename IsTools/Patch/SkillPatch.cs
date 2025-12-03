using HarmonyLib;
using JSONClass;
using KBEngine;
using SkySwordKill.NextModEditor.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.IsTools.Patch
{
    [HarmonyPatch(typeof(GUIPackage.Skill))]
    public class SkillPatch
    {

        // 用于保存原始的AttackType，使用SkillID作为键
        private static Dictionary<int, List<int>> OriginalAttackTypes = new Dictionary<int, List<int>>();
        [HarmonyPatch("Puting")]
        [HarmonyPrefix]
        public static void Puting_Prefix(Entity _attaker, Entity _receiver, GUIPackage.Skill __instance, int type = 0, string uuid = "")
        {
            // 转换实体类型为Avatar
            KBEngine.Avatar attaker = (KBEngine.Avatar)_attaker;
            _skillJsonData skillJsonData = _skillJsonData.DataDict[__instance.SkillID];
            
            // 保存原始的AttackType
            int skillId = __instance.SkillID;
            if (!OriginalAttackTypes.ContainsKey(skillId))
            {
                OriginalAttackTypes[skillId] = new List<int>(skillJsonData.AttackType);
            }
            
            // 判断玩家是否拥有添加额外属性的buffseid
            if (attaker.buffmag.HasBuffSeid(373))
            {
                // 获取玩家buff里面含有373seid的buff
                foreach (List<int> list2 in attaker.buffmag.getBuffBySeid(373))
                {
                    // 执行完后释放资源
                    using (List<JSONObject>.Enumerator enumerator2 = jsonData.instance.BuffSeidJsonData[373][string.Concat(list2[2])]["value1"].list.GetEnumerator())
                        while (enumerator2.MoveNext())
                        {
                            int value1 = (int)enumerator2.Current.n;
                            if (!skillJsonData.AttackType.Contains(value1))
                            {
                                skillJsonData.AttackType.Add(value1);
                            }
                        }
                }
            }
        }
        [HarmonyPatch("Puting")]
        [HarmonyPostfix]
        public static void Puting_Postfix(Entity _attaker, Entity _receiver, GUIPackage.Skill __instance, int type = 0, string uuid = "")
        {
            // 恢复原始的AttackType
            int skillId = __instance.SkillID;
            if (OriginalAttackTypes.ContainsKey(skillId))
            {
                _skillJsonData skillJsonData = _skillJsonData.DataDict[skillId];
                skillJsonData.AttackType = OriginalAttackTypes[skillId];
                
                // 清理字典
                OriginalAttackTypes.Remove(skillId);
            }
        }
    }
}
