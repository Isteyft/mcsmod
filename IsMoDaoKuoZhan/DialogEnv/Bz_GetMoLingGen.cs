using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Utils;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.DialogEnv
{
    [DialogEnvQuery("Bz_GetMoLingGen")]
    public class Bz_GetMoLingGen : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            return LingGenUtil.GetMoLingGenQuanZhong();
        }
    }
    
}
