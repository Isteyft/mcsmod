using BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Patch.TujianPatch;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine.SocialPlatforms.Impl;
using YSGame.TuJian;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Mod
{
    [DialogEvent("IS_CancelAchievement")]
    [DialogEvent("IS_取消成就")]
    public class IS_CancelAchievement : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var id = command.GetInt(0);

            if (AchievementData.IsSuccess(id))
            {
                AchievementData.successData.Remove(id);
                ModConfigUtils.SetConfigIntList("successAchievement", AchievementData.successData);
                UIPopTip.Inst.Pop($"「{AchievementData.GetAchievementId(id).AchievementTitle}」已经完成");
                TuJianDBPatch.TujianDBRefresh();
            }
            callback?.Invoke();
        }
    }
}
