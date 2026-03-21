using Bag;
using HarmonyLib;
using JSONClass;
using SkySwordKill.Next.DialogEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Patch.UI
{
    [HarmonyPatch(typeof(Bag.BaseItem))]
    public class BagItemPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("GetItemType")]
        public static void Postfix(int type, ref Bag.ItemType __result)
        {
            // 如果传入的类型是 17 或 18
            if (type == 17 || type == 18)
            {
                // 强制将结果设置为 "其他"
                __result = Bag.ItemType.其他;
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch("Create")]
        public static bool CreatePrefix(int id, int count, string uuid, JSONObject seid, ref BaseItem __result)
        {
            int type = 0;
            try
            {
                type = _ItemJsonData.DataDict[id].type;
            }
            catch
            {
                return true;
            }
            if (type == 17)
            {
                __result = new OtherItem();
                __result.SetItem(id, count, seid);
                __result.Uid = uuid;
                return false;
            }
            if (type == 18)
            {
                __result = new EquipItem();
                __result.SetItem(id, count, seid);
                __result.Uid = uuid;
                return false;
            }
            // 17,18以外的按原来那样处理
            return true;
        }
    }
}
