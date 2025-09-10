using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Npc
{
    [DialogEvent("IS_TuPoJinDan")]
    public class IS_TuPoJinDan : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = command.GetInt(0);
            int jdlevel = command.GetInt(1);
            int npcBigTuPoFenShu = command.GetInt(2);
            NpcTuPoUtils.TuPoJinDan(npcId, jdlevel, npcBigTuPoFenShu);
            callback?.Invoke();
        }
    }
}
