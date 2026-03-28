using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_UseSkill")]
    public class IS_UseSkill : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数
            int isplayer = command.GetInt(0, 0);
            int skillId = command.GetInt(0, 0);

            // 获取玩家对象
            var player = PlayerEx.Player;
            if (isplayer == 1)
            {
                player = player.OtherAvatar;
            }
            player.spell.spellSkill(skillId);

            callback?.Invoke();
        }
    }
}