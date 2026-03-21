using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_ClearSkill")]
    public class IS_ClearSkill : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数
            int index = command.GetInt(0, 0);
            int isplayer = command.GetInt(1, 0);

            // 获取玩家对象
            var player = PlayerEx.Player;
            if (isplayer == 1)
            {
                player = player.OtherAvatar;
            }
            int endIndex = ((JieDanManager.instence == null) ? 10 : 6);
            player.FightClearSkill(index, endIndex);

            callback?.Invoke();
        }
    }
}