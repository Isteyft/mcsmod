using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.XuanDaoZong.Patch
{
    [HarmonyPatch(typeof(UIShengWangManager))]
    public class UIShengWangManagerPatch
    {
        [HarmonyPatch("Awake")]
        public static void Postfix(UIShengWangManager __instance)
        {
            GameObject gameObject = __instance.gameObject.transform.Find("BG/ShiLiToggles/UISWShiLiToggle_ZongMen").gameObject;
            bool flag = gameObject != null;
            if (flag)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2.name = "UISWShiLiToggle_XuanDaoZong";
                UIShengWangManagerPatch.ResretNewOBJ("玄道宗", 28, gameObject2, 0f);
            }
        }
        private static void ResretNewOBJ(string ShiLiName, int ID, GameObject gameObject, float index)
        {
            gameObject.transform.localRotation = new Quaternion(0f, 0f, 0.7071f, 0.7071f);
            gameObject.transform.localPosition = new Vector3(index, 190f, 0f);
            Text component = gameObject.transform.Find("Label").GetComponent<Text>();
            component.text = ShiLiName;
            component.horizontalOverflow = HorizontalWrapMode.Overflow;
            component.gameObject.transform.localRotation = new Quaternion(0f, 0f, -0.7071f, 0.7071f);
            UISWShiLiToggle component2 = gameObject.GetComponent<UISWShiLiToggle>();
            component2.ShiLiID = ID;
            component2.ShiLiName = ShiLiName;
        }
    }
}
