using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Item
{
    [DialogEvent("IS_ZhiZuoFuLu")]
    [DialogEvent("制作符箓")]
    public class IS_ZhiZuoFuLu : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int fuLuId = command.GetInt(0, 0);
            int count = command.GetInt(1, 1);

            FuLuUtils.ZhiZuoFuLu(fuLuId, count);

            callback?.Invoke();
        }

    }
}
