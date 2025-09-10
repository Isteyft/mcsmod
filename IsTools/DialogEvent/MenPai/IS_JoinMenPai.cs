using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_JoinMenPai")]
    public class IS_JoinMenPai : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数
            int MenPaiId = command.GetInt(0);

            // 获取玩家对象
            var player = PlayerEx.Player;

            player.joinMenPai(MenPaiId);

            // 直接设置功法等级

            callback?.Invoke();
        }
    }
}