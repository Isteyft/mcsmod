using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Mod
{
    [DialogEvent("IS_SaveConfig")]
    public class IS_SaveConfig : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var key = command.GetStr(0);
            var value = command.GetStr(1);

            ModConfigUtils.SetConfigProperty(key, value);
            callback?.Invoke();
        }
    }
}
