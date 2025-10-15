using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Fight
{
    [DialogEnvQuery("IS_GetUsedSkillsId")]
    public class IS_GetUsedSkillsId : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var isplayer = context.GetMyArgs(0, 0);
            var player = PlayerEx.Player;
            if (isplayer == 1)
            {
                player = player.OtherAvatar;
            }

            return player.UsedSkills[player.UsedSkills.Count - 1]; // 返回总和
        }
    }
}