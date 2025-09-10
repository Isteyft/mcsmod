using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Npc
{
    [DialogEvent("IS_TuPoHuaShen")]
    public class IS_TuPoHuaShen : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = command.GetInt(0);
            int npcBigTuPoFenShu = command.GetInt(1);
            NpcTuPoUtils.TuPoHuaShen(npcId, npcBigTuPoFenShu);
            callback?.Invoke();
        }
    }
}
