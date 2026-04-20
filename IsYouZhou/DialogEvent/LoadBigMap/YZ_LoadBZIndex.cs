using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.Utils;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent.LoadBigMap
{
    [DialogEvent("YZ_LoadBZIndex")]
    [DialogEvent("���")]
    public class YZ_LoadBZIndex : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            MyUtil.LoadBZNoMapScenes(index);

            callback?.Invoke();
        }
    }
}