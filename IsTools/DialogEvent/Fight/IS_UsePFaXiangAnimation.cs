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
    [DialogEvent("IS_UsePFaXiangAnimation")]
    public class IS_UsePFaXiangAnimation : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int target = command.GetInt(0, 0); // 0: 玩家, 1: NPC
            float scale = command.GetFloat(1, 0f); 

            Queue<UnityAction> queue = new Queue<UnityAction>();
            UnityAction item = delegate ()
            {
                FaXiangUtils.GetAvatarSkl(target == 0 ? true : false).transform.GetComponent<Animator>().Play("Punch", -1, 0f);
                YSFuncList.Ints.Continue();
            };
            queue.Enqueue(item);
            YSFuncList.Ints.AddFunc(queue);


            callback?.Invoke();
        }
    }
}
