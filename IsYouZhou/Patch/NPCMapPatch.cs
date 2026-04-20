using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.JiuZhou.Patch
{
    [HarmonyPatch(typeof(NPCMap))]
    public class NPCMapPatch
    {

        public static List<int> npctypelist = new List<int>
        {
            750,751,752,753,754,755,756,757,758,759
        };
        [HarmonyPatch("AddNpcToBigMap")]
        public static bool Prefix(int npcId, bool isCanJieSha = true)
        {
            int i = jsonData.instance.AvatarJsonData[npcId.ToString()]["Type"].I;
            if (NPCMapPatch.npctypelist.Contains(i))
            {
                int randomInt = NpcJieSuanManager.inst.getRandomInt(100, 146);
                NPCMapPatch.AddNpcToFuBen(npcId, "F幽州", randomInt);
                if (isCanJieSha && NPCEx.GetFavor(npcId) < 200)
                {
                    NpcJieSuanManager.inst.allBigMapNpcList.Add(npcId);
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void AddNpcToFuBen(int npcId, string sceneId, int fuBenIndex)
        {
            Dictionary<int, List<int>> dictionary;
            if (NpcJieSuanManager.inst.npcMap.fuBenNPCDictionary.TryGetValue(sceneId, out dictionary))
            {
                List<int> list;
                if (dictionary.TryGetValue(fuBenIndex, out list))
                {
                    list.Add(npcId);
                }
                else
                {
                    dictionary.Add(fuBenIndex, new List<int>
                    {
                        npcId
                    });
                }
            }
            else
            {
                Dictionary<int, List<int>> value = new Dictionary<int, List<int>>
                {
                    {
                        fuBenIndex,
                        new List<int>
                        {
                            npcId
                        }
                    }
                };
                NpcJieSuanManager.inst.npcMap.fuBenNPCDictionary.Add(sceneId, value);
            }
        }
    }
}
