//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
//{
//    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
//    internal class JsonDataPatch
//    {
//        [HarmonyPrefix]
//        private static void PrefixMethod()
//        {
//            jsonData.instance.BuffSeidJsonData[109]["4013"]["value1"].Add(52499);
//            IsMoDaoKuoZhanMain.Log(jsonData.instance.BuffSeidJsonData[109]["4013"]["value1"].ToString());
//        }
//    }
//}
