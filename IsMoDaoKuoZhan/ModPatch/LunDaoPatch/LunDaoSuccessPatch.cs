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
    public class LunDaoSuccessPatch
    {
        [HarmonyPatch(typeof(LunDaoSuccess),"Init")]
        [HarmonyPrefix]
        public static void Init_Prefix(LunDaoSuccess __instance)
        {
            int num = jsonData.instance.WuDaoAllTypeJson.list.Max((JSONObject x) => x["id"].I);
            Sprite lunDaoSprite = SpriteUtil.GetLunDaoSprite(1, 45);
            if (__instance.siXuList.Count < 11)
            {
                for (int i = 11; i <= num; i++)
                {
                    __instance.siXuList.Add(null);
                }
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
                        Sprite lunDaoSprite2 = SpriteUtil.GetLunDaoSprite(1, i2);
                        __instance.siXuList.Insert(i2 - 1, lunDaoSprite2);
                    }
                    catch
                    {
                        __instance.siXuList.Insert(i2 - 1, lunDaoSprite);
                    }
                }
            }
            //KBEngine.Avatar player = Tools.instance.getPlayer();
            //int extraWuDaoZhi = 100;
            //player.WuDaoZhi += extraWuDaoZhi;
        }
    }
}
