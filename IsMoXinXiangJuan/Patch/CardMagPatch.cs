using HarmonyLib;
using IsMoXinXiangJuan;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    [HarmonyPatch(typeof(RoundManager))]
    public class CardMagPatch
    {

        //[HarmonyPatch("addCard")]
        //[HarmonyPrefix]
        //public static bool addCard(ref int cardType, int CardNum)
        //{
        //    if (cardType == 5) {
        //        cardType = new System.Random().Next(0, 5);
        //        IsMoXinXiangJuanMain.Log(cardType+"啊");
        //        return true;
        //    }
        //    return true;
        //}

        [HarmonyPatch("DrawCardCreatSpritAndAddCrystal")]
        [HarmonyPrefix]
        public static bool DrawCardCreatSpritAndAddCrystalPrefix(KBEngine.Avatar avatar, ref int type, int count = 1)
        {
            Avatar player = PlayerEx.Player;
            if (player == avatar) {
                if (Tools.instance.CheckHasTianFu(52601))
                {
                    if (type == 5)
                    {
                        type = new System.Random().Next(0, 5);
                    }
                    return true;
                } else
                {
                    return true;
                }
            }  else
            {
                return true;
            }
        }
    }
}
