using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YSGame.TuJian;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    	[HarmonyPatch(typeof(TuJianItemTab))]
    internal class TuJianItemTabPatch
    {
        [HarmonyPatch("Awake")]
        public static void Postfix(TuJianItemTab __instance)
        {
            Dictionary<int, List<string>> value = Traverse.Create(__instance).Field("options").GetValue<Dictionary<int, List<string>>>();
            value[11].Add("魔");
            value[12].Add("魔");
            value[13].Add("魔");
            __instance.ShuXingDropdown.template.sizeDelta = new Vector2(0f, 605f);
            FieldInfo fieldInfo2 = AccessTools.Field(typeof(KuangShiInfoPanel), "ShuXingDropdownDict");
            Dictionary<int, int> value3 = new Dictionary<int, int>
            {
                {
                    11,
                    11
                }
            };
            fieldInfo2.SetValue(typeof(KuangShiInfoPanel), value3);
            FieldInfo fieldInfo3 = AccessTools.Field(typeof(YaoShouCaiLiaoInfoPanel), "ShuXingDropdownDict");
            Dictionary<int, int> value4 = new Dictionary<int, int>
            {
                {
                    11,
                    11
                }
            };
            fieldInfo3.SetValue(typeof(YaoShouCaiLiaoInfoPanel), value4);
        }
    }
}
