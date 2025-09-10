using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Player
{
    [DialogEvent("IS_AddShengWang")]
    public class IS_AddShengWang : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            int add = command.GetInt(1);

            PlayerEx.AddShengWang(id, add, true);
            callback?.Invoke();
        }
    }
}
