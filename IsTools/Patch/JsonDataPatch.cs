using HarmonyLib;

namespace top.Isteyft.MCS.IsTools.Patch
{
    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
    internal class JsonDataPatch
    {
        [HarmonyPrefix]
        private static void PrefixMethod()
        {
            //给东石谷拍卖变成12格
            jsonData.instance.PaiMaiBiao["1"].SetField("ItemNum", 12);
            //IsToolsMain.Log(jsonData.instance.PaiMaiBiao.ToString());
            //jsonData.instance.StaticSkillTypeJsonData["1"].SetField("ItemNum", 12);
            // 添加ID为52的条目
            //if (!jsonData.instance.StaticSkillTypeJsonData.HasField("52"))
            //{
            //    JSONObject newEntry = new JSONObject();
            //    newEntry.SetField("id", 52);
            //    newEntry.SetField("value1", "心"); // 根据需求调整value1的值
            //    jsonData.instance.StaticSkillTypeJsonData.SetField("52", newEntry);
            //}
            //IsToolsMain.Log(jsonData.instance.StaticSkillTypeJsonData.ToString());
        }

    }
}
