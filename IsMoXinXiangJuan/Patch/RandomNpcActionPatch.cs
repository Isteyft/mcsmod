//using Google.Protobuf.WellKnownTypes;
//using HarmonyLib;
//using IsMoXinXiangJuan;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
//{
//    [HarmonyPatch(typeof(NpcJieSuanManager))]
//    public class RandomNpcActionPatch
//    {
//        [HarmonyPostfix]
//        [HarmonyPatch("RandomNpcAction")]
//        public static void PostfixRandomNpcAction(NpcJieSuanManager __instance)
//        {
//            int npcid = NPCEx.NPCIDToNew(10305);
//            if (NPCEx.IsDeath(npcid))
//            {
//                IsMoXinXiangJuanMain.Log("许墨心死亡，结算返回");
//                return;
//            }
//            if (NpcJieSuanManager.inst.IsFly(npcid)) {
//                IsMoXinXiangJuanMain.Log("许墨心飞升，结算返回");
//                return;
//            }
//            DateTime time = DateTime.Parse(__instance.JieSuanTime);
//            //int year = time.Year;
//            //int month = time.Month;
//            DateTime time1Start = DateTime.Parse("0001-01-01");
//            DateTime time1End = DateTime.Parse("0094-12-31");

//            DateTime time2Start = DateTime.Parse("0130-01-01");
//            DateTime time2End = DateTime.Parse("0239-12-31");

//            DateTime time3Start = DateTime.Parse("0400-01-01");
//            DateTime time3End = DateTime.Parse("0505-12-31");

//            // 判断当前时间是否落在任意一个时间段内
//            if ((time >= time1Start && time <= time1End) ||
//                (time >= time2Start && time <= time2End) ||
//                (time >= time3Start && time <= time3End))
//            {
//                IsMoXinXiangJuanMain.Log("许墨心不在宁州，返回");
//                return; // 如果在任意一个时间段内，直接 return
//            }
//            if (PlayerEx.IsDaoLv(npcid))
//            {
//                IsMoXinXiangJuanMain.Log("许墨心前往洞府");
//                NpcJieSuanManager.inst.npcUseItem.autoUseItem(npcid);
//                JSONObject jsonobject = jsonData.instance.AvatarJsonData[npcid.ToString()];
//                int num = jsonobject["xiuLianSpeed"].I * 2;
//                if (jsonobject.HasField("JinDanData"))
//                {
//                    float num2 = jsonobject["JinDanData"]["JinDanAddSpeed"].f / 100f;
//                    num += (int)(num2 * (float)num);
//                }
//                NpcJieSuanManager.inst.npcSetField.AddNpcExp(npcid, num);
//                NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npcid, 101);
//            }
//        }
//    }
//}
