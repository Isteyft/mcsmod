using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Custom.RealizeSeid;
using Spine.Unity;
using Tab;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.YouZhou.Patch.Tab
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
            transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            // 将心境坐标往左边移动20像素
            RectTransform xinjingRect = transform.GetComponent<RectTransform>();
            xinjingRect.anchoredPosition = new Vector2(xinjingRect.anchoredPosition.x - 50f, xinjingRect.anchoredPosition.y);
            
            Transform transform1 = gameObject.transform.Find("BaseData/丹毒");
            transform1.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            // 将丹毒坐标往左边移动20像素
            RectTransform danduRect = transform1.GetComponent<RectTransform>();
            danduRect.anchoredPosition = new Vector2(danduRect.anchoredPosition.x - 100f, danduRect.anchoredPosition.y);
            
            Transform transform2 = gameObject.transform.Find("BaseData/灵感");
            transform2.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            // 将灵感坐标往左边移动20像素
            RectTransform lingganRect = transform2.GetComponent<RectTransform>();
            lingganRect.anchoredPosition = new Vector2(lingganRect.anchoredPosition.x - 150f, lingganRect.anchoredPosition.y);

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
            rectTransform.anchoredPosition = new Vector2(0f, -25f); // 向上偏移一点
            rectTransform.sizeDelta = new Vector2(200, 200);
            rectTransform.localScale = new Vector3(0.08f, 0.08f, 0.08f); // 动画缩放
            
            // 复制灵感，往右边移动50
            if (transform2 != null)
            {
                // 复制灵感对象
                GameObject copiedLingGan = UnityEngine.Object.Instantiate(transform2.gameObject, parentTransform, false);
                copiedLingGan.name = "魔念";
                copiedLingGan.transform.SetAsFirstSibling();
                
                // 获取复制后的灵感的RectTransform
                RectTransform copiedLingGanRect = copiedLingGan.GetComponent<RectTransform>();
                
                // 往右边移动50像素
                copiedLingGanRect.anchoredPosition = new Vector2(
                    copiedLingGanRect.anchoredPosition.x + 200f,
                    copiedLingGanRect.anchoredPosition.y
                );
                
                // 将动画挂载到复制的灵感上
                container.transform.SetParent(copiedLingGan.transform, false);
                
                // 调整动画容器位置
                containerRect.anchoredPosition = new Vector2(0f, 0f); // 相对于复制的灵感居中偏上
                containerRect.sizeDelta = new Vector2(250, 150); // 容器大小
                
                // 查找复制的灵感下的子对象
                Transform nameTransform = copiedLingGan.transform.Find("Name");
                Transform valueTransform = copiedLingGan.transform.Find("Value");
                Transform valueNameTransform = copiedLingGan.transform.Find("ValueName");
                Transform imageTransform = copiedLingGan.transform.Find("BG");
                
                // 将name改成魔念
                if (nameTransform != null)
                {
                    Text nameText = nameTransform.GetComponent<Text>();
                    if (nameText != null)
                    {
                        nameText.text = "魔念：";
                    }
                }
                
                if (valueNameTransform != null)
                {
                    Text valueNameText = valueNameTransform.GetComponent<Text>();
                    if (valueNameText != null)
                    {
                        valueNameText.text = "毫无魔念";
                    }
                }
                var MoNianValue = DialogAnalysis.GetInt("幽州-魔念");
                // 将value改成1
                if (valueTransform != null)
                {
                    Text valueText = valueTransform.GetComponent<Text>();
                    if (valueText != null)
                    {
                        valueText.text = $"{MoNianValue}/999";
                    }
                }
                
                // 删除Image
                if (imageTransform != null)
                {
                    UnityEngine.Object.Destroy(imageTransform.gameObject);
                }
            }

            // 添加Canvas组件并设置较低的层级，避免挡住提示框
            Canvas canvas = container.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1; // 设置较低的层级，确保不挡住提示框
            container.AddComponent<GraphicRaycaster>();

            // 在动画对象也添加Canvas并设置相同的层级
            Canvas animCanvas = xinmoObject.AddComponent<Canvas>();
            animCanvas.overrideSorting = true;
            animCanvas.sortingOrder = 1; // 与容器保持相同的层级

            // 重新计算UI布局
            Canvas.ForceUpdateCanvases();
        }
    }
}