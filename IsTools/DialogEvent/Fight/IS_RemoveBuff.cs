using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Fight
{
    [DialogEvent("IS_RemoveBuff")]
    public class IS_RemoveBuff : IDialogEvent
    {
        // Token: 0x060000D2 RID: 210 RVA: 0x0000785C File Offset: 0x00005A5C
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int buffId = command.GetInt(0, 0);
            int target = command.GetInt(1, 0);
            var player = PlayerEx.Player;
            if (target == 1) {
                player = player.OtherAvatar;
            }
            player.buffmag.RemoveBuff(buffId);
        }
    }
}
