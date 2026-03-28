using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.IO;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetAchievementSuccess")]
    public class IS_GetAchievementSuccess : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            int id = context.GetMyArgs(0, 0);
            return AchievementData.IsSuccess(id);
        }
    }
}