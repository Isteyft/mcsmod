using Fungus;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Map
{
    [DialogEvent("IS_PlayerMove")]
    public class IS_PlayerMove : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            //在大地图和副本和海上都有效，注意Index可用范围差异较大
            int index = command.GetInt(0);
            AvatarTransfer.Do(index);
            callback?.Invoke();
        }
    }
}
