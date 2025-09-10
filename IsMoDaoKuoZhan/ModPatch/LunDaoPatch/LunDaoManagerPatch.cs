using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Util;
using UnityEngine;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.ModPatch.LunDaoPatch
{
    public class LunDaoManagerPatch
    {
        [HarmonyPatch(typeof(LunDaoManager),"StartGame")]
        [HarmonyPostfix]
        public static void Postfix(LunDaoManager __instance)
        {
            int num = jsonData.instance.WuDaoAllTypeJson.list.Max((JSONObject x) => x["id"].I);
            Sprite lunDaoSprite = SpriteUtil.GetLunDaoSprite(5, 45);
            Sprite lunDaoSprite2 = SpriteUtil.GetLunDaoSprite(2, 45);
            for (int i = 11; i <= num; i++)
            {
                __instance.cardSprites.Add(null);
                __instance.cardSpriteList.Add(null);
            }
            foreach (JSONObject jsonobject in from x in jsonData.instance.WuDaoAllTypeJson.list
                                              orderby x["id"].I
                                              select x)
            {
                int i2 = jsonobject["id"].I;
                if (i2 == 11)
                {
                    try
                    {
                        Sprite lunDaoSprite3 = SpriteUtil.GetLunDaoSprite(5, i2);
                        Sprite lunDaoSprite4 = SpriteUtil.GetLunDaoSprite(2, i2);
                        __instance.cardSprites.Insert(i2 - 1, lunDaoSprite3);
                        __instance.cardSpriteList.Insert(i2 - 1, lunDaoSprite4);
                    }
                    catch
                    {
                        __instance.cardSprites.Insert(i2 - 1, lunDaoSprite);
                        __instance.cardSpriteList.Insert(i2 - 1, lunDaoSprite2);
                    }
                }
            }
        }
    }
}
