using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_SetPlayerChengHaoId")]
    public class IS_SetPlayerChengHaoId : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数
            int id = command.GetInt(0);

            // 获取玩家对象
            var player = PlayerEx.Player;

            player.SetChengHaoId(id);

            // 直接设置功法等级

            callback?.Invoke();
        }
    }
}