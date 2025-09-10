using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YSGame.Fight;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    [HarmonyPatch(typeof(FightFaBaoShow))]
    public class FightFaBaoShowPatch
    {
        [HarmonyPatch("SetEquipFightShow", new Type[]
        {
            typeof(int),
            typeof(int),
            typeof(int),
            typeof(bool),
            typeof(bool)
        })]
        [HarmonyPrefix]
        public static void Prefix(FightFaBaoShow __instance, int equipShuXing)
        {
            Dictionary<int, Color> value = Traverse.Create(__instance).Field("EquipFightShowLightColorDict").GetValue<Dictionary<int, Color>>();
            bool flag = !value.ContainsKey(equipShuXing);
            if (flag)
            {
                ((IDictionary)value).Add(equipShuXing, new Color(0.75686276f, 0.28235295f, 0.14901961f));
            }
        }

        [HarmonyPatch("SetEquipFightShow", new Type[]
        {
            typeof(string),
            typeof(int),
            typeof(bool)
        })]
        [HarmonyPrefix]
        public static void PreFix(FightFaBaoShow __instance, string fabaoType)
        {
            Dictionary<int, Color> value = Traverse.Create(__instance).Field("EquipFightShowLightColorDict").GetValue<Dictionary<int, Color>>();
            int num = int.Parse(fabaoType.Split(new char[]
            {
                '_'
            })[1]);
            bool flag = !value.ContainsKey(num);
            if (flag)
            {
                ((IDictionary)value).Add(num, new Color(0.75686276f, 0.28235295f, 0.14901961f));
            }
        }
    }
}
