using HarmonyLib;
using SkySwordKill.Next;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Util;
using YSGame.TuJian;

namespace top.Isteyft.MCS.CGView.Patch
{
    [HarmonyPatch(typeof(TuJianDB))]
    public class TuJianDBPatch
    {
        [HarmonyPatch("InitDB")]
        [HarmonyPostfix]
        public static void Postfix()
        {
            Main.LogInfo("加载已经完成的成就配置");
            var success = ModConfigUtils.GetConfigIntList("successCG");
            if (success != null)
            {
                CGData.successData = success;
            }
            TujianDBRefresh();
        }

        public static void TujianDBRefresh()
        {
            List<Dictionary<int, string>> CGBaseList = new List<Dictionary<int, string>>();
            foreach (CGData item in CGData.data)
            {
                string title = item.CGName;
                //if (!CGData.IsSuccess(item.CGId)) title = title;
                CGBaseList.Add(new Dictionary<int, string> {
                {
                    item.CGId,
                    title
                } });
            }

            TuJianDB.ItemTuJianFilterData[99] = CGBaseList;
        }
    }
}
