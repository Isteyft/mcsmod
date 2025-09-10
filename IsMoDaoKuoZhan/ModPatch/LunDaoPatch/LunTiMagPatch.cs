using HarmonyLib;
using SkySwordKill.Next;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Util;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.ModPatch.LunDaoPatch
{
    public class LunTiMagPatch
    {
        [HarmonyPatch(typeof(LunTiMag),"CreateLunTi")]
        [HarmonyPrefix]
        public static void Prefix(List<int> lunTiList, int npcId)
        {
            JSONObject jsonobject = jsonData.instance.AvatarJsonData[npcId.ToString()]["wuDaoJson"];
            int? num = null;
            foreach (int num2 in lunTiList)
            {
                try
                {
                    num = new int?(jsonobject[num2.ToString()]["level"].I);
                }
                catch
                {
                    string lunDaoName = JsonUtil.GetLunDaoName(num2);
                    IsMoDaoKuoZhanMain.LogInfo("检测到该npc不存在魔道悟道");
                    JSONObject jsonobject2 = new JSONObject();
                    jsonobject2.AddField("id", num2);
                    jsonobject2.AddField("level", 1);
                    jsonobject2.AddField("exp", 1000);
                    jsonobject.AddField(num2.ToString(), jsonobject2);
                    IsMoDaoKuoZhanMain.LogInfo("已为该npc新增魔道悟道");
                }
            }
        }
    }
}
