using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    [DialogEnvQuery("IS_GetBuffByTypeP")]
    public class IS_GetBuffByTypeP : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var buffType = context.GetMyArgs(0, 0);
            var target = context.GetMyArgs(1, 0);
            var buffsToExcludeStr = context.GetMyArgs(2, string.Empty);

            var avatar = target == 0 ? context.Env.player : context.Env.player.OtherAvatar;
            BuffMag buffMag = avatar.buffmag;
            var buffLists = buffMag.GetAllBuffByType(buffType);

            // 提取每个子数组的最后一位数
            List<int> lastNumbers = buffLists
                .Where(subList => subList.Count > 0) // 过滤空列表
                .Select(subList => subList.Last())   // 取最后一个元素
                .ToList();

            // 处理要排除的buff列表
            if (!string.IsNullOrEmpty(buffsToExcludeStr))
            {
                // 分割字符串并转换为整数列表
                var excludeBuffs = buffsToExcludeStr.Split(',')
                    .Select(s => int.TryParse(s.Trim(), out var num) ? num : -1)
                    .Where(num => num != -1)
                    .ToList();

                // 过滤掉要排除的buff
                lastNumbers = lastNumbers
                    .Where(buffId => !excludeBuffs.Contains(buffId))
                    .ToList();
            }

            IsToolsMain.Log($"类型为{buffType}的Buff为: [{string.Join(", ", lastNumbers)}]");

            return lastNumbers;
        }
    }
}