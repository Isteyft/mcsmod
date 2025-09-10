using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    [DialogEnvQuery("IS_GetBuffByType")]
    public class IS_GetBuffByType : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var buffType = context.GetMyArgs(0, 0);
            var target = context.GetMyArgs(1, 0);
            var avatar = target == 0 ? context.Env.player : context.Env.player.OtherAvatar;
            BuffMag buffMag = avatar.buffmag;
            var buffLists = buffMag.GetAllBuffByType(buffType);

            // 提取每个子数组的最后一位数
            List<int> lastNumbers = buffLists
                .Where(subList => subList.Count > 0) // 过滤空列表
                .Select(subList => subList.Last())   // 取最后一个元素
                .ToList();

            IsToolsMain.Log($"类型为{buffType}的Buff为: [{string.Join(", ", lastNumbers)}]");

            return lastNumbers;
        }
    }
}

//BuffMag buffMag = avatar.buffmag;
//var buffLists = buffMag.GetAllBuffByType(1);

//// 遍历并拼接字符串
//string result = "";
//int totalLists = buffLists.Count;
//result += $"总共有 {totalLists} 个子数组\n";

//for (int i = 0; i < buffLists.Count; i++)
//{
//    var subList = buffLists[i];
//    result += $"子数组 {i + 1}（共 {subList.Count} 个元素）: [";
//    for (int j = 0; j < subList.Count; j++)
//    {
//        result += subList[j];
//        if (j < subList.Count - 1) result += ", ";
//    }
//    result += "]\n";
//}

//IsToolsMain.Log(result);

//// 使用 LINQ 提取每个子数组的最后一位数
//List<int> lastNumbers = buffLists
//    .Where(subList => subList.Count > 0) // 过滤掉空列表
//    .Select(subList => subList.Last())    // 取最后一个元素
//    .ToList();

//// 输出结果
//string result1 = "提取的最后一位数: [" + string.Join(", ", lastNumbers) + "]";
//IsToolsMain.Log(result1);
