using System;
using System.Collections.Generic;
using System.Linq;
using GUIPackage;
using HarmonyLib;
using JSONClass;
using SkySwordKill.Next;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Patchs;
using SuperScrollView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    [HarmonyPatch(typeof(UIHuaShenRuDaoSelect))]
    public static class UIHuaShenRuDaoSelectPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIHuaShenRuDaoSelect), "Awake")]
        public static void PostfixAwake(UIHuaShenRuDaoSelect __instance)
        {
            if (HuaShenData.DataList.Count == 9)
            {
                return;
            }
            var dict = HuaShenData.DataDict;

            var transform = __instance.ButtomListTransform;
            var go = transform.gameObject;
            go.AddMissingComponent<GridLayoutGroup>();
            transform.localPosition = new Vector3(0, 220, 0);
            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100 * 6, 100 * 2);
            var wudaoObj = transform.GetChild(0).gameObject;
            wudaoObj.SetActive(false);
            var key = dict.Keys.Where(i => i == 11).ToList();
            foreach (var i in key)
            {
                var clone = Object.Instantiate(wudaoObj, transform);
                var custom = clone.AddMissingComponent<CustomHuaShenRuDao>();
                custom.Id = i;
                HuaShenRuDaoUtils.CustomHuaShenRuDaos.Add(custom);
                clone.SetActive(true);
            }
            wudaoObj.SetActive(true);


        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIHuaShenRuDaoSelect), nameof(UIHuaShenRuDaoSelect.RefreshBtnState))]
        public static void RefreshBtnState_Postfix(UIHuaShenRuDaoSelect __instance)
        {
            if (HuaShenRuDaoUtils.CustomHuaShenRuDaos.Count == 0)
            {
                return;
            }
            foreach (var custom in HuaShenRuDaoUtils.CustomHuaShenRuDaos)
            {
                custom.Refresh();
            }

        }
    }
}
