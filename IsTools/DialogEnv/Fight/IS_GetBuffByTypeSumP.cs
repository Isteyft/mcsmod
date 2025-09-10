using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    [DialogEnvQuery("IS_GetBuffByTypeSumP")]
    public class IS_GetBuffByTypeSumP : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var buffType = context.GetMyArgs(0, 0);
            var target = context.GetMyArgs(1, 0);
            var buffsToExcludeStr = context.GetMyArgs(2, string.Empty);

            var avatar = target == 0 ? context.Env.player : context.Env.player.OtherAvatar;
            BuffMag buffMag = avatar.buffmag;
            var buffLists = buffMag.GetAllBuffByType(buffType);

            // 提取每个子数组的倒数第二个数
            List<int> secondLastNumbers = buffLists
                .Where(subList => subList.Count >= 2)
                .Select(subList => subList[subList.Count - 2])
                .ToList();

            // 处理要排除的buff列表
            if (!string.IsNullOrEmpty(buffsToExcludeStr))
            {
                // 获取所有buff ID（每个子数组的最后一个元素）
                var buffIds = buffLists
                    .Where(subList => subList.Count > 0)
                    .Select(subList => subList.Last())
                    .ToList();

                // 分割字符串并转换为整数列表
                var excludeBuffs = buffsToExcludeStr.Split(',')
                    .Select(s => int.TryParse(s.Trim(), out var num) ? num : -1)
                    .Where(num => num != -1)
                    .ToList();

                // 创建索引列表，标记哪些buff需要被排除
                var indexesToExclude = buffIds
                    .Select((buffId, index) => excludeBuffs.Contains(buffId) ? index : -1)
                    .Where(index => index != -1)
                    .ToList();

                // 过滤掉需要排除的buff对应的倒数第二个数值
                secondLastNumbers = secondLastNumbers
                    .Where((_, index) => !indexesToExclude.Contains(index))
                    .ToList();
            }

            // 计算总和
            int sum = secondLastNumbers.Sum();

            IsToolsMain.Log($"类型为{buffType}的Buff为: [{string.Join(", ", secondLastNumbers)}]\n" +
                $"类型为{buffType}的Buff数量总和为: {sum}");

            return sum; // 返回总和
        }
    }
}