using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(NpcJieSuanManager))]
    public class NpcJieSuanPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("RandomNpcAction")]
        public static void LuDingJieSuan(NpcJieSuanManager __instance)
        {
            LuDingPositionManager();
        }
        public static void LuDingPositionManager()
        {
            //NpcJieSuanManager.inst.npcMap.GetNpcSceneName
            // 遍历LuDing数组
            if (IsToolsMain.YouZhouData != null && IsToolsMain.YouZhouData.Data != null)
            {
                if (IsToolsMain.YouZhouData.Data.TryGetValue("LuDing", out string luDingStr) && !string.IsNullOrEmpty(luDingStr))
                {
                    var luDing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(luDingStr);
                    foreach (var npc in luDing)
                    {
                        if (npc.ContainsKey("id"))
                        {
                            int npcid = npc["id"];
                            NPCWarpToScene(npcid, "S101", 990);
                            //NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npcid, 101);
                        }
                    }
                }
            }
        }
        public static void NPCWarpToScene(int npcid, string sceneName, int stateCode)
        {
            // 先从当前地图移除该NPC
            NpcJieSuanManager.inst.npcMap.RemoveNpcByList(NPCEx.NPCIDToNew(npcid));

            IsToolsMain.LogInfo($"npc:{NpcUtil.GetNPCName(npcid)},前往场景{sceneName}");
            // 传送到目标场景
            NPCEx.WarpToScene(npcid, sceneName);
            //NpcJieSuanManager.inst.npcMap.AddNpcToThreeScene(npcid, 101);
            jsonData.instance.AvatarJsonData[npcid.ToString()].SetField("DongFuId", 1);
            // 设置其动作状态
            NPCEx.SetNPCAction(npcid, stateCode);
        }
    }
}
