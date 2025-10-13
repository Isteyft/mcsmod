using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.YZAction;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(NpcJieSuanManager))]
    public class NpcJieSuanManagerPatch
    {
        [HarmonyPatch("initNpcAction")]
        [HarmonyPostfix]
        public static void InitNpcAction_Postfix()
        {
            NpcJieSuanManager.inst.ActionDictionary.Add(70, new Action<int>(WanBaoLouPaiMai.NpcToWanBaoLouPaiMai1));
            NpcJieSuanManager.inst.ActionDictionary.Add(71, new Action<int>(WanBaoLouPaiMai.NpcToWanBaoLouPaiMai2));
            NpcJieSuanManager.inst.ActionDictionary.Add(72, new Action<int>(WanBaoLouPaiMai.NpcToWanBaoLouPaiMai3));
        }

        [HarmonyPatch("getFinallyNpcActionQuanZhongDictionary")]
        [HarmonyPrefix]
        public static void GetFinallyNpcActionQuanZhongDictionary_Prefix(JSONObject npcDate, Dictionary<int, int> dictionary)
        {
            // 检查70号拍卖是否开放
            if (NpcJieSuanManager.inst.PaiMaiIsOpen(jsonData.instance.NPCActionPanDingDate["70"]["PaiMaiTime"].I))
            {
                PaiMaiPanDing70(npcDate, dictionary);
            }
            // 检查71号拍卖是否开放
            if (NpcJieSuanManager.inst.PaiMaiIsOpen(jsonData.instance.NPCActionPanDingDate["71"]["PaiMaiTime"].I))
            {
                PaiMaiPanDing71(npcDate, dictionary);
            }
            // 检查72号拍卖是否开放
            if (NpcJieSuanManager.inst.PaiMaiIsOpen(jsonData.instance.NPCActionPanDingDate["72"]["PaiMaiTime"].I))
            {
                PaiMaiPanDing72(npcDate, dictionary);
            }
        }
        [HarmonyPatch("getFinallyNpcActionQuanZhongDictionary")]
        [HarmonyPostfix]
        public static void GetFinallyNpcActionQuanZhongDictionary_Postfix(JSONObject npcDate, Dictionary<int, int> dictionary)
        {
            NoXunLuoNpc(npcDate, dictionary);
        }

        private static void NoXunLuoNpc(JSONObject npcDate, Dictionary<int, int> dictionary)
        {
            // 遍历 npcDate 的所有字段并打印
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"===== NPC {npcDate["id"]?.I} 属性列表 =====");

            //foreach (string key in npcDate.keys)
            //{
            //    // 获取字段值（处理不同类型）
            //    string valueStr;
            //    if (npcDate[key].IsNumber)
            //    {
            //        valueStr = npcDate[key].I.ToString(); // 数字类型
            //    }
            //    else if (npcDate[key].IsBool)
            //    {
            //        valueStr = npcDate[key].b.ToString(); // 布尔类型
            //    }
            //    else if (npcDate[key].IsString)
            //    {
            //        valueStr = npcDate[key].str; // 字符串类型
            //    }
            //    else
            //    {
            //        valueStr = "[Complex Data]"; // 复杂类型（如数组、嵌套JSON）
            //    }

            //    sb.AppendLine($"{key}: {valueStr}");
            //}

            //// 打印完整信息
            //IsToolsMain.LogInfo(sb.ToString());
            if (npcDate.HasField("Type") && npcDate["Type"].I >= 750 && npcDate["Type"].I <= 760)
            {
                dictionary[35] = 0;
            }
        }

        private static void PaiMaiPanDing70(JSONObject npcDate, Dictionary<int, int> dictionary)
        {
            bool flag = jsonData.instance.AvatarBackpackJsonData[npcDate["id"].I.ToString()]["money"].I >= jsonData.instance.NPCActionPanDingDate["70"]["LingShi"].I &&
                npcDate["Level"].I >= jsonData.instance.NPCActionPanDingDate["70"]["JingJie"][0].I && 
                npcDate["Level"].I <= jsonData.instance.NPCActionPanDingDate["70"]["JingJie"][1].I &&
                npcDate["paimaifenzu"][NpcJieSuanManager.inst.getRandomInt(0, npcDate["paimaifenzu"].Count - 1)].I == jsonData.instance.NPCActionPanDingDate["70"]["PaiMaiType"].I;
            if (flag)
            {
                dictionary[70] = dictionary[70] + jsonData.instance.NPCActionPanDingDate["70"]["ChangeTo"].I;
            }
        }

        private static void PaiMaiPanDing71(JSONObject npcDate, Dictionary<int, int> dictionary)
        {
            bool flag = jsonData.instance.AvatarBackpackJsonData[npcDate["id"].I.ToString()]["money"].I >= jsonData.instance.NPCActionPanDingDate["71"]["LingShi"].I &&
                npcDate["Level"].I >= jsonData.instance.NPCActionPanDingDate["71"]["JingJie"][0].I && npcDate["Level"].I <= jsonData.instance.NPCActionPanDingDate["71"]["JingJie"][1].I &&
                npcDate["paimaifenzu"][NpcJieSuanManager.inst.getRandomInt(0, npcDate["paimaifenzu"].Count - 1)].I == jsonData.instance.NPCActionPanDingDate["71"]["PaiMaiType"].I;
            if (flag)
            {
                dictionary[71] = dictionary[71] + jsonData.instance.NPCActionPanDingDate["71"]["ChangeTo"].I;
            }
        }

        private static void PaiMaiPanDing72(JSONObject npcDate, Dictionary<int, int> dictionary)
        {
            bool flag = jsonData.instance.AvatarBackpackJsonData[npcDate["id"].I.ToString()]["money"].I >= jsonData.instance.NPCActionPanDingDate["72"]["LingShi"].I &&
                npcDate["Level"].I >= jsonData.instance.NPCActionPanDingDate["72"]["JingJie"][0].I && npcDate["Level"].I <= jsonData.instance.NPCActionPanDingDate["72"]["JingJie"][1].I &&
                npcDate["paimaifenzu"][NpcJieSuanManager.inst.getRandomInt(0, npcDate["paimaifenzu"].Count - 1)].I == jsonData.instance.NPCActionPanDingDate["72"]["PaiMaiType"].I;
            if (flag)
            {
                dictionary[72] = dictionary[72] + jsonData.instance.NPCActionPanDingDate["72"]["ChangeTo"].I;
            }
        }
    }
}
