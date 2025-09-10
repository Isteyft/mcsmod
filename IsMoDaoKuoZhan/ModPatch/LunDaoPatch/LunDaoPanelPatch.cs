using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Util;
using UnityEngine;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.ModPatch.LunDaoPatch
{
    public class LunDaoPanelPatch
    {
        [HarmonyPatch(typeof(LunDaoPanel),"Init")]
        [HarmonyPrefix]
        public static void Prefix(LunDaoPanel __instance)
        {
            Traverse traverse = Traverse.Create(__instance);
            List<Sprite> value = traverse.Field("lunTiNameSpriteList").GetValue<List<Sprite>>();
            List<Sprite> value2 = traverse.Field("wuDaoQiuSpriteList").GetValue<List<Sprite>>();
            int num = jsonData.instance.WuDaoAllTypeJson.list.Max((JSONObject x) => x["id"].I);
            Sprite lunDaoSprite = SpriteUtil.GetLunDaoSprite(4, 45);
            Sprite lunDaoSprite2 = SpriteUtil.GetLunDaoSprite(3, 45);
            for (int i = 11; i <= num; i++)
            {
                ((IList)value).Add(null);
                ((IList)value2).Add(null);
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
                        Sprite lunDaoSprite3 = SpriteUtil.GetLunDaoSprite(4, i2);
                        Sprite lunDaoSprite4 = SpriteUtil.GetLunDaoSprite(3, i2);
                        ((IList)value).Insert(i2 - 1, lunDaoSprite3);
                        ((IList)value2).Insert(i2, lunDaoSprite4);
                    }
                    catch
                    {
                        ((IList)value).Insert(i2 - 1, lunDaoSprite);
                        ((IList)value2).Insert(i2, lunDaoSprite2);
                    }
                }
            }
        }
    }
}
