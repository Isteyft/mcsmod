using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.JiuZhou.YZAction
{
    public class WanBaoLouPaiMai
    {
        public static void NpcToWanBaoLouPaiMai1(int npcId)
        {
            try
            {
                int i = NpcJieSuanManager.inst.GetNpcData(npcId)["Type"].I;
                if (jsonData.instance.NpcThreeMapBingDate[i.ToString()].HasField("WanBaoLouPaiMai1"))
                {
                    NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npcId, 27385);
                    NpcJieSuanManager.inst.npcTeShu.AddNpcToPaiMaiList(27385, npcId);
                }
            } catch(Exception e)
            {
                IsToolsMain.Error(e.ToString());
            }
        }
        public static void NpcToWanBaoLouPaiMai2(int npcId)
        {
            int i = NpcJieSuanManager.inst.GetNpcData(npcId)["Type"].I;
            if (jsonData.instance.NpcThreeMapBingDate[i.ToString()].HasField("WanBaoLouPaiMai2"))
            {
                NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npcId, 27387);
                NpcJieSuanManager.inst.npcTeShu.AddNpcToPaiMaiList(27387, npcId);
            }
        }
        public static void NpcToWanBaoLouPaiMai3(int npcId)
        {
            int i = NpcJieSuanManager.inst.GetNpcData(npcId)["Type"].I;
            if (jsonData.instance.NpcThreeMapBingDate[i.ToString()].HasField("WanBaoLouPaiMai3"))
            {
                NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npcId, 27388);
                NpcJieSuanManager.inst.npcTeShu.AddNpcToPaiMaiList(27388, npcId);
            }
        }
    }
}
