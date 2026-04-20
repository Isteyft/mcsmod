using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.TianNan.Utils;

namespace top.Isteyft.MCS.TianNan.DialogEvent
{
    [DialogEvent("LoadTianNan")]
    [DialogEvent("天南")]
    [DialogEvent("进入天南")]
    public class LoadTianNan : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            TianNanMapUtils.LoadTianNan(index);

            callback?.Invoke();
        }
    }
}
