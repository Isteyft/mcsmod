using HarmonyLib;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSGame.Fight;

namespace top.Isteyft.MCS.IsTools.Patch.RoundManagerPatch
{
    [HarmonyPatch(typeof(RoundManager))]
    [HarmonyPatch("endRound")]
    public class NewRoundPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Entity _avater)
        {
            var roundManager = RoundManager.instance;
            if (roundManager == null)
            {
                return true;
            }
            KBEngine.Avatar avatar = (KBEngine.Avatar)_avater;
            if (avatar == null)
            {
                IsToolsMain.Warning("玩家对象为空");
                return true;
            }
            if (avatar.spell.HasBuff(9600))
            {
                IsToolsMain.LogInfo($" ({(avatar.isPlayer() ? "玩家" : "对手")}) 获得全新回合");
                if (avatar.isPlayer()) UIFightPanel.Inst.FightCenterTip.HideTip();
                roundManager.startRound(_avater);
                return false;
            }
            return true;
        }
    }
}
