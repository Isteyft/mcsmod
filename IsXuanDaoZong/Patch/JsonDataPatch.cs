using HarmonyLib;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.XuanDaoZong.Patch
{
    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
    internal class JsonDataPatch
    {
        [HarmonyPrefix]
        private static void PrefixMethod()
        {
            //XuanDaoZongMain.Log("导出数据");
            jsonData.instance.CyShiLiNameData["28"] = new JSONObject();
            jsonData.instance.CyShiLiNameData["28"].SetField("id", 28);
            jsonData.instance.CyShiLiNameData["28"].SetField("name", "\u7384\u9053\u5b97");
            jsonData.instance.ShiLiHaoGanDuName["28"] = new JSONObject();
            jsonData.instance.ShiLiHaoGanDuName["28"].SetField("id", 28);
            //jsonData.instance.ShiLiHaoGanDuName["28"].SetField("ChinaText", "玄道宗");
            jsonData.instance.ShiLiHaoGanDuName["28"].SetField("ChinaText", "\u7384\u9053\u5b97");
            jsonData.instance.StrTextJsonData["menpai28"] = new JSONObject();
            jsonData.instance.StrTextJsonData["menpai28"].SetField("StrID", "menpai28");
            jsonData.instance.StrTextJsonData["menpai28"].SetField("ChinaText", "\u7384\u9053\u5b97");
            //XuanDaoZongMain.Log(jsonData.instance.ShiLiHaoGanDuName.ToString());
        }

    }
}
