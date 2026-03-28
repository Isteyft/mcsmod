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
    [DialogEvent("IS_RotationFaXiang")]
    public class IS_RotationFaXiang : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0: 玩家, 1: NPC
            float x = command.GetFloat(1, 0f);
            float y = command.GetFloat(2, 0f);
            float z = command.GetFloat(3, 0f);

            // 查找对应的Avatar GameObject
            UnityEngine.GameObject avatarObj = FaXiangUtils.GetAvatarSklPosition(target == 0 ? true : false);
            // 查找所有子对象，删除所有变身模型
            for (int i = 0; i < avatarObj.transform.childCount; i++)
            {
                var child = avatarObj.transform.GetChild(i).gameObject;
                IsToolsMain.LogInfo(child.name);
                // 找到avatar已经激活的模型
                if (child.name.Contains("Avater") &&
                    !(child.name.Contains("Avater50_1(Clone)") || child.name.Contains("Avater51_1(Clone)")))
                {
                    if (child.active)
                    {
                        FaXiangUtils.UpdateGameObjectRotation(child, x, y, z);
                        break;
                    }
                }
            }

            callback?.Invoke();
        }
    }
}
