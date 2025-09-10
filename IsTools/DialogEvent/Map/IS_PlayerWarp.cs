using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Map
{
    [DialogEvent("IS_PlayerWarp")]
    public class IS_PlayerWarp : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string scene = command.GetStr(0);
            int index = command.ParamList.Length > 1 && command.GetStr(1) != "" ? command.GetInt(1) : 0;

            bool result = top.Isteyft.MCS.IsTools.Util.WarpUtils.PlayerWarp(scene, index);
            env.mapScene = SceneEx.NowSceneName;

            env.tmpArgs.Remove("PlayerWarp");
            env.tmpArgs.Add("PlayerWarp", Convert.ToInt32(result));

            callback?.Invoke();
        }
    }
}
