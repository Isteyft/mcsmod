using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Player
{
    [DialogEnvQuery("IS_HasKeZhanTime")]
    public class IS_HasKeZhanTime : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            string scenes = context.GetMyArgs(0, "");
            KBEngine.Avatar player = Tools.instance.getPlayer();
            bool hasTime = player.zulinContorl.HasTime(scenes);
            return hasTime;
        }
    }
}
