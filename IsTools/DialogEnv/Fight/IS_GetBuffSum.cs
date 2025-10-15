using System;
using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    [DialogEnvQuery("IS_GetBuffSum")]
    public class IS_GetBuffSum : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int buffId = context.GetMyArgs(0, 0);
            int target = context.GetMyArgs(1, 0);
            Avatar avatar = (target == 0) ? context.Env.player : context.Env.player.OtherAvatar;
            BuffMag buffMag = avatar.buffmag;
            int buffSum = buffMag.GetBuffSum(buffId);
            return buffSum;
        }
    }
}
