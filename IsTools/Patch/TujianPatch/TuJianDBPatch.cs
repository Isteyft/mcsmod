using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Util;
using YSGame.TuJian;

namespace top.Isteyft.MCS.IsTools.Patch.TujianPatch
{
    [HarmonyPatch(typeof(TuJianDB))]
    public class TuJianDBPatch
    {
        [HarmonyPatch("InitDB")]
        [HarmonyPostfix]
        public static void Postfix()
        {
            IsToolsMain.LogInfo("加载已经完成的成就配置");
            var success = ModConfigUtils.GetConfigIntList("successAchievement");
            if (success != null)
            {
                AchievementData.successData = success;
            }
            TujianDBRefresh();
        }

        public static void TujianDBRefresh()
        {
            List<Dictionary<int, string>> AchievementBaseList = new List<Dictionary<int, string>>();
            foreach (AchievementData item in AchievementData.data)
            {
                string title = item.AchievementTitle;
                if (!AchievementData.IsSuccess(item.Id)) title = title + "(未完成)";
                AchievementBaseList.Add(new Dictionary<int, string> {
                {
                    item.Id,
                    title
                } });
            }

            TuJianDB.ItemTuJianFilterData[11] = AchievementBaseList;
        }
    }
}
