using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Player
{
    [DialogEvent("IS_GetId")]
    public class IS_GetId : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {

            UIPopTip.Inst.Pop($"MonstarID:{Tools.instance.MonstarID}");
            UIPopTip.Inst.Pop($"MonstarID:{Tools.instance.GetInstanceID()}");

            callback?.Invoke();
        }
    }
}
