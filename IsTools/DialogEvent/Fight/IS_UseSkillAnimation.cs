using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YSGame;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_UseSkillAnimation")]
    public class IS_UseSkillAnimation : IDialogEvent
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
            Queue<UnityAction> queue = new Queue<UnityAction>();
            UnityAction item = delegate ()
            {
                ((UnityEngine.GameObject)player.renderObj).transform.GetChild(0).GetComponent<Animator>().Play("Punch", -1, 0f);
                YSFuncList.Ints.Continue();
            };
            queue.Enqueue(item);
            YSFuncList.Ints.AddFunc(queue);

            callback?.Invoke();
        }
    }
}