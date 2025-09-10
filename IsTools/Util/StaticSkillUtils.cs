using JSONClass;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.Util
{
    public static class StaticSkillUtils
    {
        /// <summary>
        /// 通过功法流水号（Skill_ID）获取唯一ID（id）
        /// </summary>
        /// <param name="staticSkillId">功法流水号（Skill_ID）</param>
        /// <returns>唯一ID（id），未找到返回-1</returns>
        public static int GetStaticSkillUniqueId(int staticSkillId)
        {
            using (var enumerator = StaticSkillJsonData.DataList
                .Where(item => item.Skill_ID == staticSkillId)
                .GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return enumerator.Current.id;
                }
            }

            //IsToolsMain.Warning($"功法唯一ID获取失败：未找到 Skill_ID = {staticSkillId} 的功法");
            return -1;
        }

        /// <summary>
        /// 获取所有匹配的功法唯一ID（适用于同一Skill_ID有多个等级的情况）
        /// </summary>
        /// <param name="staticSkillId">功法流水号（Skill_ID）</param>
        /// <returns>所有匹配的唯一ID列表</returns>
        public static List<int> GetAllStaticSkillUniqueIds(int staticSkillId)
        {
            var matchedSkills = StaticSkillJsonData.DataList
                .Where(item => item.Skill_ID == staticSkillId)
                .Select(item => item.id)
                .ToList();

            if (!matchedSkills.Any())
            {
                //IsToolsMain.Warning($"功法唯一ID获取失败：未找到 Skill_ID = {staticSkillId} 的功法");
            }

            return matchedSkills;
        }

        /// <summary>
        /// 通过功法ID获取等级（Skill_Lv）
        /// </summary>
        /// <param name="staticSkillId">功法唯一ID（id）</param>
        /// <returns>等级（Skill_Lv），未找到返回-1</returns>
        public static int GetSkillLevelById(int staticSkillId)
        {
            using (var enumerator = StaticSkillJsonData.DataList
                .Where(item => item.id == staticSkillId)
                .GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return enumerator.Current.Skill_Lv;
                }
            }

            //IsToolsMain.Warning($"功法等级获取失败：未找到 id = {staticSkillId} 的功法");
            return -1;
        }
    }
}