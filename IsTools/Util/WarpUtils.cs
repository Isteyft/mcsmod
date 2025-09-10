using JSONClass;
using System;
using System.Collections.Generic;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class WarpUtils
    {
        protected static KBEngine.Avatar player => Tools.instance.getPlayer();

        ///   <summary>  
        ///   传送
        ///   </summary>  
        public static bool PlayerWarp(string scene, int index = 0)
        {
            IsToolsMain.Log("NowMapIndex " + player.NowMapIndex);
            if (scene.StartsWith("AllMaps"))
            {
                if (index <= 0) index = 101;
                player.NowMapIndex = index;
                Tools.instance.loadMapScenes(scene);
                return true;
            }
            else if (scene.StartsWith("F"))
            {
                //此方法从大地图进副本出来后还在原地
                //player.NowMapIndex = AutoMapIndex(scene);
                if (index <= 0) index = 1;
                SceneEx.LoadFuBen(scene, index);
                return true;
            }
            else if (scene.StartsWith("Sea"))
            {
                player.NowMapIndex = 29;
                if (index <= 0) index = SeaZuoBiao[scene];
                SceneEx.LoadFuBen(scene, index);
                return true;
            }
            else if (scene.StartsWith("S"))
            {
                //注意S101洞府要手动设置进第几层，推荐手动设置出来大地图的路点位置。
                if (index > 0)
                    player.NowMapIndex = index;
                if (index == -1)
                    player.NowMapIndex = AutoMapIndex(scene);
                IsToolsMain.Log("set NowMapIndex " + player.NowMapIndex);
                Tools.instance.loadMapScenes(scene);
                return true;
            }
            else
            {
                return false;
            }
        }

        ///   <summary>  
        ///   获取所在位置
        ///   </summary>  
        public static string GetPlaceName()
        {
            string screenName = Tools.getScreenName();
            if (RandomFuBen.IsInRandomFuBen)
            {
                return (string)player.RandomFuBenList[RandomFuBen.NowRanDomFuBenID.ToString()]["Name"];
            }
            else if (screenName == "S101")
            {
                return DongFuManager.GetDongFuName(DongFuManager.NowDongFuID);
            }
            else
            {
                if (!jsonData.instance.SceneNameJsonData.HasField(screenName))
                {
                    return "未知";
                }
                return jsonData.instance.SceneNameJsonData[screenName]["MapName"].Str;
                //其实本来是EventName场景名称，奈何觅长生比较奇葩，MapName地图名称信息更多，比如四大岛的客栈坊市码头，而五宗门广场编号不同但是没做区分。
            }
        }

        ///   <summary>  
        ///   传送npc至指定场景
        ///   </summary>  
        public static bool NpcWarp(int npcId, string scene, int index = 0)
        {
            npcId = NPCEx.NPCIDToNew(npcId);
            if (NPCEx.IsDeath(npcId))
                return false;
            IsToolsMain.Log("NpcWarp " + npcId + scene);
            if (scene.StartsWith("AllMaps"))
            {
                //这里也只清了大地图中的npc
                NpcMapRemoveNpc(npcId);
                NPCEx.WarpToMap(npcId, index);
                return true;
            }
            else if (scene.StartsWith("F"))
            {
                // NPCEx.WarpToPlayerNowFuBen仅为传送到玩家当前所在副本
                NpcMapRemoveNpc(npcId);
                if (index == 0 && player.NowFuBen == scene) index = player.fubenContorl[scene].NowIndex;
                if (index == 0) index = 1;
                Dictionary<string, Dictionary<int, List<int>>> fuBenDict = NpcJieSuanManager.inst.npcMap.fuBenNPCDictionary;
                if (!fuBenDict.ContainsKey(scene))
                {
                    fuBenDict.Add(scene, new Dictionary<int, List<int>>());
                }
                if (!fuBenDict[scene].ContainsKey(index))
                {
                    fuBenDict[scene].Add(index, new List<int>());
                }
                if (!fuBenDict[scene][index].Contains(npcId))
                {
                    fuBenDict[scene][index].Add(npcId);
                }
                NpcJieSuanManager.inst.isUpDateNpcList = true;
                return true;
            }
            else if (scene.StartsWith("S"))
            {
                //原版只移除三级场景中的，不行
                //NPCEx.WarpToScene(npcId, scene);
                NpcMapRemoveNpc(npcId);
                Dictionary<string, List<int>> threeDict = NpcJieSuanManager.inst.npcMap.threeSenceNPCDictionary;
                if (!threeDict.ContainsKey(scene))
                {
                    threeDict.Add(scene, new List<int>());
                }
                if (!threeDict[scene].Contains(npcId))
                {
                    threeDict[scene].Add(npcId);
                }
                if (scene == "S101")
                {
                    jsonData.instance.AvatarJsonData[npcId.ToString()].SetField("DongFuId", index);
                }
                NpcJieSuanManager.inst.isUpDateNpcList = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        ///   <summary>  
        ///   移除地图上特定npc
        ///   </summary> 
        public static bool NpcMapRemoveNpc(int npcId)
        {
            //原版也有一个NPCMap.RemoveNpcByList，但原版的只移除一次就返回了，而且表中有特殊隐藏npc就不处理（特殊key）
            npcId = NPCEx.NPCIDToNew(npcId);
            bool removed = false;
            //地点在副本
            foreach (Dictionary<int, List<int>> fubendict in NpcJieSuanManager.inst.npcMap.fuBenNPCDictionary.Values)
            {
                foreach (List<int> posdict in fubendict.Values)
                {
                    if (posdict.Contains(npcId))
                    {
                        posdict.Remove(npcId);
                        removed = true;
                    }
                }
            }
            //地点在大地图
            foreach (List<int> ludiandict in NpcJieSuanManager.inst.npcMap.bigMapNPCDictionary.Values)
            {
                if (ludiandict.Contains(npcId))
                {
                    ludiandict.Remove(npcId);
                    removed = true;
                }

            }
            //地点在三级场景
            foreach (List<int> threedict in NpcJieSuanManager.inst.npcMap.threeSenceNPCDictionary.Values)
            {
                if (threedict.Contains(npcId))
                {
                    threedict.Remove(npcId);
                    removed = true;
                }

            }
            //刷新玩家当前所在场景的npc
            NpcJieSuanManager.inst.isUpDateNpcList = true;
            return removed;
        }

        //场景id和大地图上路点的规律并没有严格遵守，所以还是建议手动指定的好
        private static int AutoMapIndex(string name)
        {
            int type = SceneNameJsonData.DataDict[name].MoneyType;
            if (type == 2 || type == 3)
                return 29;
            if (name == "S101")
            {
                if (DongFuManager.NowDongFuID == 1)
                    return 98;
                if (DongFuManager.NowDongFuID == 2)
                {
                    switch (player.menPai)
                    {
                        case 1: return 12;
                        case 3: return 14;
                        case 4: return 16;
                        case 5: return 15;
                        case 6: return 11;
                        default: return 101;
                    }
                }
            }
            if (name.Length <= 3)
                return Convert.ToInt32(name.Remove(0, 1));
            else if (name.StartsWith("S10"))
                return 101;
            else if (name.StartsWith("S1"))
            {
                if (name.Length == 4)
                    return Convert.ToInt32(name.Substring(2, 1));
                else
                    return Convert.ToInt32(name.Substring(2, 2));
            }
            else
                return Convert.ToInt32(name.Substring(1, 2));

        }
        //关于无尽之海中格子的序号：
        //概念上分为小海域，大海域（为什么不是中？），和无尽之海
        //小海域是7*7格的方块。
        //大海域就是我们地图上看到的XX海，所谓Sea2~Sea18，每个都包含若干个完整小海域，形状不同，有配置表@大海域拥有的小海域记录了包含关系
        //无尽之海总共是宽19个小海域，高10个小海域，也就是总宽133格，高70格
        //NodeIndex从1开始，到133*70=9310，是每个格子的编号，按从左到右从上到下顺序排列，转化为第几行第几格NodePos（xy从0开始）
        //同样，小海域的编号，也是在无尽之海中从左往右从上到下计算，可用函数 int GetSmallSeaIDByNodeIndex(int nodeIndex)计算
        //有了小海域id，可通过查询SeaEx.BigSeaHasSmallSeaIDDict(jsonData.instance.EndlessSeaHaiYuData)得知属于哪个我们熟悉的大海域
        //每个格子在小海域中的位置，可以由Vector2Int GetSmallSeaPosByNodePos(Vector2Int nodePos)计算
        //格子在小海域中的序号inseaid（1~49），可转为总无尽之海的Index = EndlessSeaMag.GetRealIndex(int seaID, int index)，也可再转回来SeaNodeMag.GetInSeaID(int AllMapIndex, int wide)
        //结合一下，就可以循环遍历知道每个大海域包含哪些格子序号了。
        private static Dictionary<string, int> SeaZuoBiao = new Dictionary<string, int>()
        {
            {"Sea2",200},//北宁海
            {"Sea3",1111},//西宁海
            {"Sea4",2200},//南宁海
            {"Sea5",3217},//千流海域
            {"Sea6",6976},//南崖海域
            {"Sea7",5142},//碎星海域
            {"Sea8",1425},//蓬莎海域
            {"Sea9",1842},//浪方海域
            {"Sea10",5895},//吞云海
            {"Sea11",952},//雷鸣海
            {"Sea12",6144},//图南海
            {"Sea13",6591},//阴冥海
            {"Sea14",7554},//幽冥海
            {"Sea15",7336},//玄冥海
            {"Sea16",3829},//东海
            {"Sea17",5831},//化龙海
            {"Sea18",4172},//无尽海渊
        };
    }
}
