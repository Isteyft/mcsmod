using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using HarmonyLib;
using Newtonsoft.Json;
using SkySwordKill.Next;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;
using Ventulus;

namespace top.Isteyft.MCS.IsTools.ModPatch
{
    public class MoreNPCInfoPatch
    {
        public static List<NPCInfoData> NPCInfoDatas = new List<NPCInfoData>();

        [HarmonyPatch(typeof(MoreNPCInfo),"BuildCiTiao")]
        [HarmonyPrefix]
        public static void Prefix(MoreNPCInfo __instance)
        {
            MoreNPCInfoPatch.Init();
            for (int i = 0; i < MoreNPCInfoPatch.NPCInfoDatas.Count; i++)
            {
                NPCInfoData npcinfoData = MoreNPCInfoPatch.NPCInfoDatas[i];
                if (npcinfoData.NPCAction != null && npcinfoData.NPCAction.Count > 0)
                {
                    MoreNPCInfoPatch.TryAdd(__instance, "NPCAction", npcinfoData.NPCAction);
                }
                if (npcinfoData.NPCLiuPai != null && npcinfoData.NPCLiuPai.Count > 0)
                {
                    MoreNPCInfoPatch.TryAdd(__instance, "NPCLiuPai", npcinfoData.NPCLiuPai);
                }
                if (npcinfoData.NPCTag != null && npcinfoData.NPCTag.Count > 0)
                {
                    MoreNPCInfoPatch.TryAdd(__instance, "NPCTag", npcinfoData.NPCTag);
                }
                if (npcinfoData.NPCType != null && npcinfoData.NPCType.Count > 0)
                {
                    MoreNPCInfoPatch.TryAdd(__instance, "NPCType", npcinfoData.NPCType);
                }
                if (npcinfoData.NPCWuDao != null && npcinfoData.NPCWuDao.Count > 0)
                {
                    MoreNPCInfoPatch.TryAdd(__instance, "NPCWuDao", npcinfoData.NPCWuDao);
                }
                if (npcinfoData.NPCXingGe != null && npcinfoData.NPCXingGe.Count > 0)
                {
                    MoreNPCInfoPatch.TryAdd(__instance, "NPCXingGe", npcinfoData.NPCXingGe);
                }
            }
            IsToolsMain.LogInfo("所有信息添加完毕");
        }

        public static void TryAdd(MoreNPCInfo __instance, string name, Dictionary<int, string> newdic)
        {
            Dictionary<int, string> value = Traverse.Create(__instance).Field(name).GetValue<Dictionary<int, string>>();
            foreach (KeyValuePair<int, string> keyValuePair in newdic)
            {
                value[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        public static void Init()
        {
            List<DirectoryInfo> AllMods = IsToolsMain.AllMods;
            foreach (DirectoryInfo directoryInfo in AllMods)
            {
                string path = directoryInfo.FullName + "/plugins/BaizeAssets/config/MoreNPCInfo.json";
                string jsonString = JsonUtil.GetJsonString(path);
                if (!jsonString.IsNullOrWhiteSpace())
                {
                    MoreNPCInfoPatch.NPCInfoDatas.Add(JsonConvert.DeserializeObject<NPCInfoData>(jsonString));
                }
            }
        }

    }


    public class NPCInfoData
    {
        public Dictionary<int, string> NPCLiuPai { get; set; }
        public Dictionary<int, string> NPCTag { get; set; }
        public Dictionary<int, string> NPCXingGe { get; set; }
        public Dictionary<int, string> NPCWuDao { get; set; }
        public Dictionary<int, string> NPCType { get; set; }
        public Dictionary<int, string> NPCAction { get; set; }
    }
}