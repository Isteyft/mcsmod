using HarmonyLib;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.JiuZhou.Scene.sea
{
    [HarmonyPatch(typeof(EndlessSeaMag))]
    public class EndlessSeaMagPatch
    {
        public class GlobalTeleportRule
        {
            public List<int> TriggerIndexes;  // 连续的坐标
            public string TargetMapName;      // 目标地图
            public int CoordinateOffset;      // 坐标偏移量（例如 100）
        }
        public static List<int> indexs = new List<int>
        {
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
            29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56,
            78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105,
            106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126
        };
        public static List<GlobalTeleportRule> globalRules = new List<GlobalTeleportRule>
        {
            new GlobalTeleportRule
            {
                TriggerIndexes = new List<int> { 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105},
                TargetMapName = "Sea41", // 31605
                CoordinateOffset = 31521
            },
            new GlobalTeleportRule
            {
                TriggerIndexes = new List<int> { 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126 },
                TargetMapName = "Sea36", // 31605
                CoordinateOffset = 31521
            },
            new GlobalTeleportRule
            {
                TriggerIndexes = new List<int> { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 },
                TargetMapName = "Sea37", // 31605
                CoordinateOffset = 31521
            },
            new GlobalTeleportRule
            {
                TriggerIndexes = new List<int> {29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56 },
                TargetMapName = "Sea31", // 31605
                CoordinateOffset = 31521
            }
        };


        private static FieldInfo _luXianDianField = AccessTools.Field(typeof(MapSeaCompent), "LuXianDian");

        [HarmonyPatch("Move")]
        public static bool Prefix(int endPositon, int index, EndlessSeaMag __instance)
        {
            if (!indexs.Contains(endPositon)) { return true; }
            foreach (GlobalTeleportRule rule in globalRules)
            {
                if (rule.TriggerIndexes.Contains(endPositon))
                {
                    int avatarNowMapIndex = __instance.getAvatarNowMapIndex();
                    __instance.RestLuXianDianSprite();
                    JSONObject jSONObject = jsonData.instance.SceneNameJsonData[rule.TargetMapName];
                    USelectBox.Show("是否进入" + jSONObject["EventName"].Str + "？", delegate
                    {
                        int targetIndex = endPositon + rule.CoordinateOffset;
                        IsToolsMain.LogInfo(targetIndex.ToString());
                        DialogAnalysis.StartTestDialogEvent($"PlayerWarp*{rule.TargetMapName}#{targetIndex}");
                    });
                    return false;
                }
            }

            return true;
        }


    }
}
