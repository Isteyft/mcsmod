using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Mod
{
    [DialogEvent("IS_AutoSave")]
    [DialogEvent("IS_自动存档")]
    public class IS_AutoSave : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            AutoSaveUtils.AutoSave();
            if (callback != null)
            {
                callback();
            }
        }

    }
}
