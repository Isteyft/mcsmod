using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.IO;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetConfig")]
    public class IS_GetConfig : IDialogEnvQuery
    {
        private static List<DirectoryInfo> _mods;

        public object Execute(DialogEnvQueryContext context)
        {
            string key = context.GetMyArgs(0, 0).ToString();
            return ModConfigUtils.GetConfigProperty(key);
        }
    }
}