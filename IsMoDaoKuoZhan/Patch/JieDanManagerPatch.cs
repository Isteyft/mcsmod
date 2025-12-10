//using System;
//using System.Collections.Generic;
//using System.Linq;
//using HarmonyLib;
//using KBEngine;
//using top.Isteyft.MCS.IsTools.Util;

//namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
//{
//    [HarmonyPatch(typeof(JieDanManager))]
//    public class JieDanManagerPatch
//    {
//        [HarmonyPatch("Start")]
//        [HarmonyPostfix]
//        public static void PostfixStart(JieDanManager __instance)
//        {
//            if (!ModUtil.CheckModActive("2968233692"))
//            {
//                if (!__instance.jieDanBuff.Contains(4028))
//                {
//                    __instance.jieDanBuff.Add(4028);
//                }
//            }
//            JSONObject jsonobject = jsonData.instance.BuffSeidJsonData[109]["4013"]["value1"];
//            if (!jsonobject.ToList().Contains(52499))
//            {
//                jsonData.instance.BuffSeidJsonData[109]["4013"]["value1"].Add(52499);
//            }
//        }
//        [HarmonyPatch("getJinDanID")]
//        [HarmonyPrefix]
//        public static bool PrefixGetJinDanID(JieDanManager __instance, ref int __result)
//        {
//            Avatar avatar = Tools.instance.getPlayer();
//            Dictionary<int, int> lingqichi = GetLingQiChi();
//            int buffCengShu = 0;
//            bool flag = false;
//            int num = -1;
//            __instance.jieDanBuff.ForEach(delegate (int aa)
//            {
//                avatar.buffmag.getBuffByID(aa).ForEach(delegate (List<int> cc)
//                {
//                    if (aa < 4028)
//                    {
//                        buffCengShu += cc[1];
//                        lingqichi[aa - 4022] += cc[1];
//                    }
//                    else
//                    {
//                        int jinDanTypeByBuffID = GetJinDanTypeByBuffID(aa);
//                        buffCengShu += cc[1];
//                        lingqichi[jinDanTypeByBuffID] += cc[1];
//                    }
//                });
//            });
//            foreach (JSONObject item in jsonData.instance.JieDanBiao.list)
//            {
//                if ((int)item["JinDanQuality"].n != buffCengShu)
//                {
//                    continue;
//                }

//                if (item["JinDanType"].Count == 1)
//                {
//                    if (lingqichi[(int)item["JinDanType"][0].n] > buffCengShu / 2)
//                    {
//                        __result = item["id"].I;
//                        return false;
//                    }
//                }
//                else if (item["JinDanType"].Count == 2)
//                {
//                    bool flag2 = true;
//                    foreach (JSONObject item2 in item["JinDanType"].list)
//                    {
//                        if (lingqichi[(int)item2.n] <= buffCengShu / 3)
//                        {
//                            flag2 = false;
//                        }
//                    }

//                    if (flag2)
//                    {
//                        __result = item["id"].I;
//                        return false;
//                    }
//                }

//                if ((int)item["JinDanType"][0].n == 5)
//                {
//                    flag = true;
//                    num = item["id"].I;
//                }
//            }

//            if (flag)
//            {
//                __result = num;
//                return false;
//            }

//            __result = num;
//            return false;
//        }

//        private static Dictionary<int, int> GetLingQiChi()
//        {
//            return (from item in jsonData.instance.JieDanBiao.list
//                    where item["JinDanType"].Count == 1
//                    select item["JinDanType"][0].I).Distinct().ToDictionary((int key) => key, (int value) => 0);
//        }

//        private static int GetJinDanTypeByBuffID(int BuffID)
//        {
//            if (BuffID == 4028)
//            {
//                return 11;
//            }

//            return 0;
//        }
//    }

//    [HarmonyPatch(typeof(showJieDanBuff), "Start")]
//    public class ShowJieDanBuffPatch
//    {
//        [HarmonyPostfix]
//        public static void Postfix(showJieDanBuff __instance)
//        {
//            bool flag = !__instance.showBuffID.Contains(4028);
//            if (flag)
//            {
//                __instance.showBuffID.Add(4028);
//            }
//        }
//    }
//}