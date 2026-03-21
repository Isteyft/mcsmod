using Bag;
using FairyGUI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tab;
using UnityEngine;

namespace top.Isteyft.MCS.IsTools.Patch.UI.EquipSlot
{
    [HarmonyPatch(typeof(Tab.TabWuPingPanel))]
    public class TabWuPingPanelPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Init")]
        public static void InitPostfix(TabWuPingPanel __instance) 
        {
            GameObject gameObject = typeof(TabWuPingPanel).GetField("_go", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as GameObject;
            //string path = "TabPanel(Clone)/TabSelect/Panel/物品/EquipList/饰品";
            Transform transform = gameObject.transform.Find("EquipList/饰品");
            Transform transform2 = UnityEngine.Object.Instantiate<Transform>(transform, transform.transform.parent);
            transform2.name = "灵兽";
            transform2.localPosition = new Vector3(0.0f, -400.0f, 0f);

            var slotComponent = transform2.GetComponent<Bag.EquipSlot>();

            const int SPIRIT_BEAST_ID = 18;

            // 1. 设置 AcceptType (决定哪些物品能放入)
            var acceptTypeField = AccessTools.Field(typeof(Bag.EquipSlot), "AcceptType");
            if (acceptTypeField != null)
            {
                var canSlotTypeEnum = acceptTypeField.FieldType;
                var acceptValue = Enum.ToObject(canSlotTypeEnum, SPIRIT_BEAST_ID);
                acceptTypeField.SetValue(slotComponent, acceptValue);
            }

            // 2. 【关键新增】设置 EquipSlotType (决定槽位自身ID)
            var equipSlotTypeField = AccessTools.Field(typeof(Bag.EquipSlot), "EquipSlotType");
            if (equipSlotTypeField != null)
            {
                var equipSlotTypeEnum = equipSlotTypeField.FieldType;
                var slotTypeValue = Enum.ToObject(equipSlotTypeEnum, SPIRIT_BEAST_ID);
                equipSlotTypeField.SetValue(slotComponent, slotTypeValue);
            }

            // 3. 注册到 EquipDict (必须用相同ID!)
            var equipDict = __instance.EquipDict;
            if (equipDict.ContainsKey(SPIRIT_BEAST_ID))
                equipDict.Remove(SPIRIT_BEAST_ID);

            equipDict.Add(SPIRIT_BEAST_ID, slotComponent);
        }
        //[HarmonyPrefix]
        //[HarmonyPatch("AddEquip")]
        //[HarmonyPatch(new Type[] { typeof(Bag.EquipItem) })]
        //public static void AddEquipPatch(EquipItem equipItem, TabWuPingPanel __instance)
        //{
        //    IsToolsMain.Log(equipItem.Type);
        //    //foreach (int key in __instance.EquipDict.Keys)
        //    //{
        //    //    if (equipItem.Type != 18)
        //    //    {
        //    //        return true;
        //    //    }
        //    //    else
        //    //    {
        //    //        __instance.AddEquip(key, equipItem);
        //    //        break;
        //    //    }
        //    //}

        //    //DragMag.Inst.Clear();
        //    //return false;
        //}

    }
}
