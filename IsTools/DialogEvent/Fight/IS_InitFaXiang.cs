using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSGame;
using top.Isteyft.MCS.IsTools;
using UnityEngine;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;
using SkySwordKill.NextMoreCommand.Custom.RealizeSeid;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Fight
{
    [DialogEvent("IS_InitFaXiang")]
    public class IS_InitFaXiang : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0: 玩家, 1: NPC
            int i = command.GetInt(1, 0); // 预制体id
            int mode = command.GetInt(2, 0); // 预制体id
            KBEngine.Avatar avatar = AvatarUtils.GetAvatar(target);
            bool isPlayer = AvatarUtils.isPlayer(avatar);
            var isAvatarSkl = FaXiangUtils.isAvatarSkl(i);
            // 获取当前玩家对象
            UnityEngine.GameObject avatar10 = FaXiangUtils.GetAvatarSklPosition(isPlayer);
            // 实例化预制体
            UnityEngine.GameObject avatarInstance = UnityEngine.Object.Instantiate(FaXiangUtils.GetFaXiangPrefab(i));

            avatarInstance.transform.SetParent(avatar10.transform);
            // 召唤模式
            FaXiangUtils.InitFaXiangPosition(avatarInstance, isPlayer, mode);

            avatarInstance.transform.SetAsFirstSibling();
            if (isAvatarSkl)
            {
                FaXiangUtils.InitAvatarSklFace(avatarInstance, i);
            }

            callback?.Invoke();
        }
    }
}
