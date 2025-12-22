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
using KBEngine;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Fight
{
    [DialogEvent("IS_ActionFaXiang")]
    public class IS_ActionFaXiang : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0表示玩家，1表示npc

            // 查找对应的Avatar GameObject
            UnityEngine.GameObject avatarObj = FaXiangUtils.GetAvatarSklPosition(target == 0 ? true : false);

            // 查找所有子对象，删除所有变身模型
            for (int i = 0; i < avatarObj.transform.childCount; i++)
            {
                var child = avatarObj.transform.GetChild(i).gameObject;
                IsToolsMain.LogInfo(child.name);
                // 删除所有不是原始模型的Avatar模型
                if (child.name.Contains("Avater") &&
                    !(child.name.Contains("Avater50_1(Clone)") || child.name.Contains("Avater51_1(Clone)")))
                {
                    //UnityEngine.Object.Destroy(child);
                    child.SetActive(true);
                    IsToolsMain.LogInfo($"激活变身模型: {child.name}");
                }
            }
            FaXiangUtils.usePlayer(target == 0 ? true : false);
            callback?.Invoke();
        }
    }
}