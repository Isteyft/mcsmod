using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Mod
{
    [DialogEvent("IS_SetStaticValue")]
    [DialogEvent("IS_设置全局变量")]
    public class IS_SetStaticValue : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var id = command.GetInt(0);
            var value = command.GetInt(1);

            GlobalValue.Set(id, value);
            callback?.Invoke();
        }
    }
}
