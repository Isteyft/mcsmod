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
    [DialogEvent("YZ_LoadYongZhouIndex")]
    [DialogEvent("雍州")]
    public class YZ_LoadYongZhouIndex : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            MyUtil.LoadYongZhouNoMapScenes(index);

            callback?.Invoke();
        }
    }
}