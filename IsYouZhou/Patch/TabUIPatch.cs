using System;
using System.Reflection;
using HarmonyLib;
using Spine.Unity;
using Tab;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(TabShuXingPanel))]
    public class TabUIPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Init")]
        private static void TabUIMag_Init_Patch(TabShuXingPanel __instance)
        {
            UnityEngine.GameObject gameObject = typeof(TabShuXingPanel)
                .GetField("_go", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(__instance) as UnityEngine.GameObject;

            Transform transform = gameObject.transform.Find("BaseData/心境");

            if (transform == null) return;

            // 不复制心境，只获取父容器用于挂载
            Transform parentTransform = transform.transform.parent;

            // 加载SkeletonDataAsset
            string xinmoDataPath = "ui icon/fight/jieying/xinmo/xinmo_qie_SkeletonData";
            var skeletonData = Resources.Load<SkeletonDataAsset>(xinmoDataPath);

            // 加载字体
            Font cloudLiBianFont = Resources.Load<Font>("HUAWNXINWEI");
            if (cloudLiBianFont == null)
            {
                // 如果找不到字体，尝试其他可能的路径
                cloudLiBianFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            // 创建外层的容器
            GameObject container = new GameObject("XinMo_Container");
            container.transform.SetParent(parentTransform, false);

            // 添加容器的RectTransform
            RectTransform containerRect = container.AddComponent<RectTransform>();

            // 创建动画对象作为容器的子对象
            GameObject xinmoObject = new GameObject("XinMo_Qie_Animation");
            xinmoObject.transform.SetParent(container.transform, false);

            // 添加RectTransform
            RectTransform rectTransform = xinmoObject.AddComponent<RectTransform>();

            // 使用SkeletonGraphic替代SkeletonAnimation
            SkeletonGraphic skeletonGraphic = xinmoObject.AddComponent<SkeletonGraphic>();
            skeletonGraphic.skeletonDataAsset = skeletonData;
            skeletonGraphic.raycastTarget = false;

            // 初始化
            skeletonGraphic.Initialize(false);

            skeletonGraphic.AnimationState.SetAnimation(0, "stand by", true);

            // 设置动画的位置（在容器内居中）
            rectTransform.anchoredPosition = new Vector2(0f, 40f); // 向上偏移一点
            rectTransform.sizeDelta = new Vector2(200, 200);
            rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // 动画缩放

            // 设置整个容器的位置（在心境左边100像素）
            RectTransform xinjingRect = transform.GetComponent<RectTransform>();
            containerRect.anchoredPosition = new Vector2(
                xinjingRect.anchoredPosition.x - 200f,
                xinjingRect.anchoredPosition.y + 80f
            );
            containerRect.sizeDelta = new Vector2(250, 150); // 容器大小

            // 设置层级
            container.transform.SetAsLastSibling();

            // 添加文字"魔念"在容器内（不在动画对象下）
            GameObject textObject1 = new GameObject("魔念");
            textObject1.transform.SetParent(container.transform, false);
            Text text1 = textObject1.AddComponent<Text>();
            text1.text = "魔念";
            text1.fontSize = 28; // 增大字号
            text1.color = Color.white;
            text1.alignment = TextAnchor.UpperCenter;
            text1.font = cloudLiBianFont;
            text1.horizontalOverflow = HorizontalWrapMode.Overflow;
            text1.verticalOverflow = VerticalWrapMode.Overflow;
            text1.color = new Color(0, 0, 0);

            RectTransform textRect1 = textObject1.GetComponent<RectTransform>();
            textRect1.anchoredPosition = new Vector2(0f, 10f); // 在动画下方
            textRect1.sizeDelta = new Vector2(120, 35);
            textRect1.localScale = Vector3.one; // 文字不受动画缩放影响

            // 添加数字"1"在"魔念"下方
            GameObject textObject2 = new GameObject("Value");
            textObject2.transform.SetParent(container.transform, false);
            Text text2 = textObject2.AddComponent<Text>();
            text2.text = "1";
            text2.fontSize = 32; // 增大字号
            text2.color = Color.white;
            text2.alignment = TextAnchor.UpperCenter;
            text2.font = cloudLiBianFont;
            text2.horizontalOverflow = HorizontalWrapMode.Overflow;
            text2.verticalOverflow = VerticalWrapMode.Overflow;
            text2.color = new Color(0,0,0);

            RectTransform textRect2 = textObject2.GetComponent<RectTransform>();
            textRect2.anchoredPosition = new Vector2(0f, -20f); // 在魔念下方
            textRect2.sizeDelta = new Vector2(80, 38);
            textRect2.localScale = Vector3.one; // 文字不受动画缩放影响

            // 添加Canvas组件确保Text能正确渲染
            Canvas canvas = container.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 101; // 比动画层级稍高
            container.AddComponent<GraphicRaycaster>();

            // 在动画对象也添加Canvas（可选）
            Canvas animCanvas = xinmoObject.AddComponent<Canvas>();
            animCanvas.overrideSorting = true;
            animCanvas.sortingOrder = 100;

            // 重新计算UI布局
            Canvas.ForceUpdateCanvases();
        }
    }
}