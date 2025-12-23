using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Utils;

namespace top.Isteyft.MCS.YouZhou.DialogEvent.LoadBigMap
{
    [DialogEvent("YZ_LoadZZIndex")]
    [DialogEvent("中州")]
    public class YZ_LoadZZIndex : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            MyUtil.LoadZZNoMapScenes(index);

            callback?.Invoke();
        }
    }
}
