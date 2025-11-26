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
    [DialogEvent("LoadYongZhou")]
    public class LoadYongZhou : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            MyUtil.LoadYongZhou();

            callback?.Invoke();
        }
    }
}