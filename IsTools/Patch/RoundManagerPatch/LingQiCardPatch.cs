using HarmonyLib;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Patch.RoundManagerPatch
{

    [HarmonyPatch(typeof(RoundManager), "UpdateCard")]
    public class LingQiCardPatch
    {
        [HarmonyPostfix]
        public static void Postfix(MessageData data)
        {
            Avatar avatar = (Avatar)KBEngineApp.app.player();
            avatar.spell.onBuffTickByType(360);
        }
    }
    //public class RoundManagerPatc
    //{
    //    [HarmonyPostfix]
    //    [HarmonyPatch("drawCardCreatSpritAndAddCrystal")]
    //    public static void PostfixDrawCard(Avatar avatar, int type, card __result)
    //    {
    //        IsToolsMain.Log("触发吸收灵气后逻辑");
    //        // 触发灵气改变事件
    //        avatar.spell.onBuffTickByType(360);

    //    }
    //}
}
