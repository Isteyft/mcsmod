using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Npc
{
    [DialogEvent("IS_SetNpcChengHaoId")]
    public class IS_SetNpcChengHaoId : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            int chenghaoid = command.GetInt(1);

            id = NPCEx.NPCIDToNew(id);
            if (NPCEx.IsDeath(id))
                return;

            NpcJieSuanManager.inst.npcChengHao.UpDateChengHao(id, chenghaoid);

            IsToolsMain.Log($"修改NPCID:{id}称号为Id{chenghaoid}");
            callback?.Invoke();
        }
    }
}
