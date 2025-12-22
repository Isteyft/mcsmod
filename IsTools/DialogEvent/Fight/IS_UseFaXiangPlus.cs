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
    [DialogEvent("IS_UseFaXiangPlus")]
    public class IS_UseFaXiangPlus : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0: 玩家, 1: NPC
            int id = command.GetInt(1, 0); // 0: 玩家, 1: NPC
            // 查找对应的Avatar GameObject
            UnityEngine.GameObject avatarObj = FaXiangUtils.GetAvatarSklPosition(target == 0 ? true : false);
            // 查找所有子对象，删除所有变身模型
            for (int i = 0; i < avatarObj.transform.childCount; i++)
            {
                var child = avatarObj.transform.GetChild(i).gameObject;
                IsToolsMain.LogInfo(child.name);
                // 查找指定模型
                if (child.name.Contains($"Avater{id}_1(Clone)"))
                {
                    if (child.active)
                    {
                        child.transform.SetAsFirstSibling();
                        break;
                    }
                    else
                    {
                        IsToolsMain.LogInfo("法相已经隐藏了");

                    }
                }
            }

            callback?.Invoke();
        }
    }
}
