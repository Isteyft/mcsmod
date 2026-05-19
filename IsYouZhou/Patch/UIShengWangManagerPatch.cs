using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.JiuZhou.Patch
{
    [HarmonyPatch(typeof(UIShengWangManager))]
    public class UIShengWangManagerPatch
    {
        [HarmonyPatch("Awake")]
        public static void Postfix(UIShengWangManager __instance)
        {
            GameObject gameObject = __instance.gameObject.transform.Find("BG/ShiLiToggles/UISWShiLiToggle_ZongMen").gameObject;
            if (gameObject != null)
            {
                // 70 灞州
                GameObject gameObject2_70 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_70.name = "UISWShiLiToggle_BaZhou";
                UIShengWangManagerPatch.ResretNewOBJ("灞州", 700, gameObject2_70, -100f);

                // 71 衡州
                GameObject gameObject2_71 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_71.name = "UISWShiLiToggle_HengZhou";
                UIShengWangManagerPatch.ResretNewOBJ("衡州", 710, gameObject2_71, 50f);

                // 72 靖州
                GameObject gameObject2_72 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_72.name = "UISWShiLiToggle_JingZhou";
                UIShengWangManagerPatch.ResretNewOBJ("靖州", 720, gameObject2_72, 200f);

                // 74 颍州
                GameObject gameObject2_74 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_74.name = "UISWShiLiToggle_YingZhou";
                UIShengWangManagerPatch.ResretNewOBJ("颍州", 740, gameObject2_74, 350f);

                // 75 雍州
                GameObject gameObject2_75 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_75.name = "UISWShiLiToggle_YongZhou";
                UIShengWangManagerPatch.ResretNewOBJ("雍州", 750, gameObject2_75, 500f);

                // 76 幽州 (您提供的原始代码)
                GameObject gameObject2_76 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_76.name = "UISWShiLiToggle_YouZhou";
                UIShengWangManagerPatch.ResretNewOBJ("幽州", 760, gameObject2_76, 650f);

                // 77 渝州
                GameObject gameObject2_77 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_77.name = "UISWShiLiToggle_YuZhou";
                UIShengWangManagerPatch.ResretNewOBJ("渝州", 770, gameObject2_77, 800f);

                // 78 中州
                GameObject gameObject2_78 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
                gameObject2_78.name = "UISWShiLiToggle_ZhongZhou";
                UIShengWangManagerPatch.ResretNewOBJ("中州", 780, gameObject2_78, 950f);
            }
        }
        private static void ResretNewOBJ(string ShiLiName, int ID, GameObject gameObject, float index)
        {
            gameObject.transform.localRotation = new Quaternion(0f, 0f, 0.7071f, 0.7071f);
            gameObject.transform.localPosition = new Vector3(index, -200f, 0f);
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
