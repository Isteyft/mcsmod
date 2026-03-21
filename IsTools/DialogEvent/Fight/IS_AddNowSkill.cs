using GUIPackage;
using KBEngine;
using MaiJiu.MCS.HH.Util;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Custom.RealizeSeid;
using System;
using top.Isteyft.MCS.IsTools.Data;

namespace top.Isteyft.MCS.IsTools.DialogEvent.MenPai
{
    [DialogEvent("IS_AddNowSkill")]
    public class IS_AddNowSkill : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数;
            int isplayer = command.GetInt(0, 0);

            // 获取玩家对象
            var player = PlayerEx.Player;
            if (isplayer == 1)
            {
                player = player.OtherAvatar;
            }
            
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    if (!(FightNowSkills.NowSkill[i] == 0 || FightNowSkills.NowSkill[i] == null))
                    {
                        player.FightAddSkill(FightNowSkills.NowSkill[i], i, 10);
                    }
                } catch (Exception ex)
                {
                    IsToolsMain.Error(ex);
                }
            }

            callback?.Invoke();
        }
    }
}