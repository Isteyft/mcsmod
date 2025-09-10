using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Npc
{
    [DialogEvent("IS_ShowNPCInfoPanel")]
    public class IS_ShowNPCInfoPanel : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = command.GetInt(0);
            var npcData = new UINPCData(npcId);
            npcData.RefreshData();
            UINPCJiaoHu.Inst.NowJiaoHuNPC = npcData;
            UINPCJiaoHu.Inst.ShowNPCInfoPanel();
            callback?.Invoke();
        }
    }
}
