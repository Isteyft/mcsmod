using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.Utils;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{
    [DialogEvent("YZ_NpcTo")]
    public class YZ_NpcTo : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npc = command.GetInt(0);
            int map = command.GetInt(1);
            NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npc, map);

            callback?.Invoke();
        }
    }
}
