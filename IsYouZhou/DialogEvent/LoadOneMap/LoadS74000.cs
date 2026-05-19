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
    [DialogEvent("LoadS74000")]
    public class LoadS74000 : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            MyUtil.LoadS74000NoMapScenes(command.GetInt(0, 740006));

            callback?.Invoke();
        }
    }
}