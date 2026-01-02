using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class NpcUtil
    {
        ///   <summary>  
        ///   按条件生成npc，不强制要求的参数写成0，结果为id
        ///   </summary>  
        public static int CreateNpc(int type = 0, int liuPai = 0, int level = 0, int sex = 0, int zhengXie = 0)
        {
            List<JSONObject> list = jsonData.instance.NPCLeiXingDate.list;
            if (type > 0)
                list = list.Where(x => x["Type"].I == type).ToList();
            if (liuPai > 0)
                list = list.Where(x => x["LiuPai"].I == liuPai).ToList();
            if (level > 0)
                list = list.Where(x => x["Level"].I == level).ToList();
            if (list.Count > 0)
            {
                int j = ModUtil.GetRandom(0, list.Count);
                int result = FactoryManager.inst.npcFactory.AfterCreateNpc(list[j], isImportant: false, ZhiDingindex: 0, isNewPlayer: false, importantJson: null, setSex: sex);
                if (zhengXie == 1 || zhengXie == 2)
                {
                    int xingGe = FactoryManager.inst.npcFactory.getRandomXingGe(zhengXie);
                    jsonData.instance.AvatarJsonData[result.ToString()].SetField("XingGe", xingGe);
                }
                IsToolsMain.LogInfo("创建的npcid" + result.ToString());
                return result;
            }
            IsToolsMain.Warning("创建npc失败");
            return 0;
        }

        ///   <summary>  
        ///   按条件筛选npc，不强制要求的参数写成0，结果为id列表
        ///   </summary>  
        public static List<int> SearchNpc(int type = 0, int liuPai = 0, int level = 0, int sex = 0, int zhengXie = 0)
        {
            //LogMessage("SearchNpc tiaojian " + type + liuPai + level + sex + zhengXie);
            List<JSONObject> list = jsonData.instance.AvatarJsonData.list.Where(x => x["id"].I >= 20000 && !x.HasField("IsFly")).ToList();
            if (type > 0)
                list = list.Where(x => x["Type"].I == type).ToList();
            if (liuPai > 0)
                list = list.Where(x => x["LiuPai"].I == liuPai).ToList();
            if (level > 0)
                list = list.Where(x => x["Level"].I == level).ToList();
            if (sex > 0)
                list = list.Where(x => x["SexType"].I == sex).ToList();
            if (zhengXie == 1)
                list = list.Where(x => x["XingGe"].I < 10).ToList();
            if (zhengXie == 2)
                list = list.Where(x => x["XingGe"].I > 10).ToList();
            IsToolsMain.LogInfo("搜索到npc的数量" + list.Count);
            return list.Select(x => x["id"].I).ToList();
        }

        ///   <summary>  
        ///   根据npcId获取姓名
        ///   </summary>
        public static string GetNPCName(int npcId)
        {
            //一般情情况在AvatarRandomJsonData有，死了在npcDeathJson里记newId，实在不行再AvatarJsonData里找oldId
            npcId = NPCEx.NPCIDToNew(npcId);
            int oldId = NPCEx.NPCIDToOld(npcId);
            if (npcId == 0)
                return "旁白";
            else if (npcId == 1)
                return Tools.GetPlayerName();
            else if (jsonData.instance.AvatarRandomJsonData.HasField(npcId.ToString()))
                return jsonData.instance.AvatarRandomJsonData[npcId.ToString()]["Name"].str.ToCN();
            else if (NpcJieSuanManager.inst.npcDeath.npcDeathJson.HasField(npcId.ToString()))
                return NpcJieSuanManager.inst.npcDeath.npcDeathJson[npcId.ToString()]["deathName"].str.ToCN();
            else if (jsonData.instance.AvatarJsonData.HasField(oldId.ToString()))
            {
                JSONObject jsonobject = jsonData.instance.AvatarJsonData[oldId.ToString()];
                return jsonobject["FirstName"].str.ToCN() + jsonobject["Name"].str.ToCN();
            }
            else
                return "未知";
        }
    }
}
