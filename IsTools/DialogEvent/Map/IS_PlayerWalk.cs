using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Map
{
    [DialogEvent("IS_PlayerWalk")]
    public class IS_PlayerWalk : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            //仅大地图生效
            if (Tools.getScreenName() != "AllMaps")
                return;
            int index = command.GetInt(0);
            AllMapManage.instance.mapIndex[index].movaAvatar();
            PlayerEx.Player.NowMapIndex = index;
            callback?.Invoke();
        }
    }
}
