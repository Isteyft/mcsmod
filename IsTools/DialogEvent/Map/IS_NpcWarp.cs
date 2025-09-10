using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Map
{
    [DialogEvent("IS_NpcWarp")]
    public class IS_NpcWarp : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = command.GetInt(0);
            string scene = command.GetStr(1);
            int index = command.ParamList.Length > 2 && command.GetStr(2) != "" ? command.GetInt(2) : 0;

            bool result = top.Isteyft.MCS.IsTools.Util.WarpUtils.NpcWarp(npcId, scene, index);
            env.tmpArgs.Remove("NpcWarp");
            env.tmpArgs.Add("NpcWarp", Convert.ToInt32(result));

            callback?.Invoke();
        }
    }
}
