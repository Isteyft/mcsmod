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

namespace top.Isteyft.MCS.IsTools.DialogEvent.Fight
{
    [DialogEvent("IS_RotationPFaXiang")]
    public class IS_RotationPFaXiang : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0: 玩家, 1: NPC
            float x = command.GetFloat(1, 0f); 
            float y = command.GetFloat(2, 0f);
            float z = command.GetFloat(3, 0f); 

            FaXiangUtils.UpdateGameObjectRotation(FaXiangUtils.GetAvatarSkl(target == 0 ? true : false), x, y, z);

            callback?.Invoke();
        }
    }
}
