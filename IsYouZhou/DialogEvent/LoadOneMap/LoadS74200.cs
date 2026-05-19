using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.Utils;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent.LoadOneMap
{
    [DialogEvent("LoadS74200")]
    public class LoadS74200 : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            MyUtil.LoadS74200NoMapScenes(command.GetInt(0, 742102));

            callback?.Invoke();
        }
    }
}