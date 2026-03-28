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
using UnityEngine.Events;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Fight
{
    [DialogEvent("IS_UseFaXiangAnimation")]
    public class IS_UseFaXiangAnimation : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0: 玩家, 1: NPC
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
                        Queue<UnityAction> queue = new Queue<UnityAction>();
                        UnityAction item = delegate ()
                        {
                            child.transform.GetComponent<Animator>().Play("Punch", -1, 0f);
                            YSFuncList.Ints.Continue();
                        };
                        queue.Enqueue(item);
                        YSFuncList.Ints.AddFunc(queue);
                        break;
                    }
                }
            }

            callback?.Invoke();
        }
    }
}
