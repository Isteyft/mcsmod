using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Mod
{
    [DialogEvent("IS_DelConfig")]
    public class DelConfig : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var key = command.GetStr(0);

            ModConfigUtils.RemoveConfigProperty(key);
            callback?.Invoke();
        }
    }
}
