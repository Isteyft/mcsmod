using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    [DialogEnvQuery("IS_GetBuffByTypeSum")]
    public class IS_GetBuffByTypeSum : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var buffType = context.GetMyArgs(0, 0);
            var target = context.GetMyArgs(1, 0);
            var avatar = target == 0 ? context.Env.player : context.Env.player.OtherAvatar;
            BuffMag buffMag = avatar.buffmag;
            var buffLists = buffMag.GetAllBuffByType(buffType);

            List<int> secondLastNumbers = buffLists
                .Where(subList => subList.Count >= 2)
                .Select(subList => subList[subList.Count - 2])
                .ToList();

            // 计算总和
            int sum = secondLastNumbers.Sum();

            IsToolsMain.Log($"类型为{buffType}的Buff为: [{string.Join(", ", secondLastNumbers)}]\n" +
                $"类型为{buffType}的Buff数量总和为: {sum}");

            return sum; // 返回总和
        }
    }
}