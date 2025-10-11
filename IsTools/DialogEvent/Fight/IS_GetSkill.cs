using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_GetSkill")]
    public class IS_GetSkill : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数
            int isplayer = command.GetInt(0, 0);

            // 获取玩家对象
            var player = PlayerEx.Player;
            if (isplayer == 1)
            {
                player = player.OtherAvatar;
            }
            player.UsedSkills.ForEach(skill =>
            {
                UIPopTip.Inst.Pop(skill.ToString());
                IsToolsMain.LogInfo(skill.ToString());
            });
            var a = player.UsedSkills[player.UsedSkills.Count - 1];

            callback?.Invoke();
        }
    }
}