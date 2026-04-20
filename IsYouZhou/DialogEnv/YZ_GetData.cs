using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.JiuZhou.DialogEnv
{
    [DialogEnvQuery("YZ_GetData")]
    public class YZ_GetData : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var dataName = context.GetMyArgs(0, "");

            return IsToolsMain.YouZhouData.Data[dataName];
        }
    }
}