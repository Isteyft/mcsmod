using Bag;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    public static class LianQiCaiLiaoTypeExtension
    {
        public const int 魔 = 8;
    }

    [HarmonyPatch(typeof(CaiLiaoItem), "GetLianQiCaiLiaoType")]
    public class BagCaiLiaoItemPatch
    {

        public static bool Prefix(CaiLiaoItem __instance, ref LianQiCaiLiaoType __result)
        {
            int shuXingType = __instance.ShuXingType / 10;

            if (shuXingType == 8)
            {
                __result = (LianQiCaiLiaoType)6; // 强制转换成枚举
                return false;
            }

            return true;

        }

    }
}
