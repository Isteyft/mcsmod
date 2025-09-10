using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Player
{
    [DialogEvent("IS_AddShengPing")]
    public class IS_AddShengPing : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            String shengping = command.GetStr(0);

            PlayerEx.RecordShengPing(shengping);
            callback?.Invoke();
        }
    }
}
