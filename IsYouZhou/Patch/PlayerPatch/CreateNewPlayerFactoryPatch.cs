//using DebuggingEssentials;
//using HarmonyLib;
//using KBEngine;
//using MaiJiu.MCS.HH.Scene;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using top.Isteyft.MCS.YouZhou.Scene;

//namespace top.Isteyft.MCS.YouZhou.Patch.PlayerPatch
//{
//    public class CreateNewPlayerFactoryPatch
//    {
//        [HarmonyPatch("createPlayer")]
//        [HarmonyPostfix]
//        public static void Postfix()
//        {
//            if (Tools.instance.getPlayer() != null)
//            {
//                Avatar player = Tools.instance.getPlayer();
//                //player.addHasSkillList(1878107);
//                //player.addHasSkillList(1878113);
//                //player.AvatarQieCuo.AddField("InitSea", true);
//                if (Tools.instance.CheckHasTianFu(720004))
//                {
//                    Tools.instance.loadMapScenes("S27390", true);
//                    AllMapComponent.SetAvatarNowMapIndexStatic(11);
//                    //player.AvatarType = 3U;
//                    //player.menPai = 390;
//                    //player.SetChengHaoId(0);
//                    //PlayerEx.SetShiLiChengHaoLevel(1, 1);
//                    //PlayerEx.RecordShengPing("加入冥教", null);
//                    //IsToolsMain.Log("天赋天煞孤星激活。");
//                }
//                //bool flag3 = Tools.instance.CheckHasTianFu(836056);
//                //if (flag3)
//                //{
//                //    player.addHasStaticSkillList(669293, 1);
//                //}
//            }
//        }
//    }
//}
