using GUIPackage;
using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    /// <summary>
    /// 获取上一次的战斗结果 (1: 常规 2: 战斗胜利 3: 战斗失败 4: 战斗逃跑)
    /// </summary>
    [DialogEnvQuery("IS_GetFightResult")]
    public class IS_GetFightResult : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {

            return GlobalValue.GetTalk(1);
        }
    }
}
