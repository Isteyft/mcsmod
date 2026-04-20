using HarmonyLib;
using SkySwordKill.Next;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace top.Isteyft.MCS.JiuZhou.Patch
{
    [HarmonyPatch(typeof(MainUIMag), "OpenMain")]
    public class OpenMainPatch
    {
        private const int CUSTOM_BG_COUNT = 5;
        private const int TOTAL_STATES = CUSTOM_BG_COUNT + 1;
        // 懒加载缓存：key = 图片编号，value = Sprite
        private static Dictionary<int, Sprite> spriteCache = new Dictionary<int, Sprite>();
        private static Sprite originalBgSprite = null; // 用于保存原始背景
        private static int currentBgIndex = 0;         // 初始为 0（九州1）

        private static Sprite LoadYouZhouSprite(int index)
        {
            // index: 0-based → 对应 九州1.png 是 index=0
            int fileNumber = index + 1;

            if (spriteCache.TryGetValue(index, out Sprite cached))
            {
                return cached; // 已缓存，直接返回
            }

            string path = $"Assets/CG/九州{fileNumber}.png";
            Texture2D tex = Main.Res.LoadAsset<Texture2D>(path);
            if (tex == null)
            {
                Debug.LogError($"[九州背景] 无法加载: {path}");
                return null;
            }

            // 创建 Sprite 并缓存
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            spriteCache[index] = sprite;
            return sprite;
        }

        // 清空缓存
        // public static void ClearCache() { spriteCache.Clear(); }

        [HarmonyPrefix]
        public static bool OpenMain_Prefix_Patch(MainUIMag __instance)
        {
            var bgImageObj = __instance.新主界面.transform.Find("bg1").gameObject;
            var bgImage = bgImageObj.GetComponent<Image>();

            // 首次进入时保存原始背景
            if (originalBgSprite == null)
            {
                originalBgSprite = bgImage.sprite;
            }

            if (currentBgIndex < CUSTOM_BG_COUNT)
            {
                Sprite sprite = LoadYouZhouSprite(currentBgIndex);
                if (sprite != null)
                    bgImage.sprite = sprite;
                else
                    bgImage.sprite = originalBgSprite; // 加载失败回退
             }
            else
            {
                bgImage.sprite = originalBgSprite;
              }
            bool isCustom = (currentBgIndex < CUSTOM_BG_COUNT);
            bgImageObj.SetActive(isCustom);

            // 复制按钮
            GameObject btnObj = __instance.新主界面.transform.Find("Panel/btn/神仙斗法")
                .gameObject.CopyGameObject(null, "Next");
            if (ModUtil.CheckModActive("2939918022"))
            {
                btnObj.transform.MoveLocal(new Vector3(0f, 258f, 0f));
            } else
            {
                btnObj.transform.MoveLocal(new Vector3(0f, 172f, 0f));
            }
            

            FpBtn btn = btnObj.GetComponent<FpBtn>();
            btn.mouseUpEvent = new UnityEvent();
            btn.mouseUpEvent.AddListener(delegate ()
            {
                currentBgIndex = (currentBgIndex + 1) % TOTAL_STATES;
                bool isCustomNow = (currentBgIndex < CUSTOM_BG_COUNT);
                if (currentBgIndex < CUSTOM_BG_COUNT)
                {
                    Sprite nextSprite = LoadYouZhouSprite(currentBgIndex);
                    if (nextSprite != null)
                        bgImage.sprite = nextSprite;
                    else
                        bgImage.sprite = originalBgSprite;
                }
                else
                {
                    bgImage.sprite = originalBgSprite;
                }
                bgImageObj.SetActive(isCustomNow);
            });

            // 设置按钮外观（保持不变）
            Sprite normal = Main.Res.LoadAsset<Texture2D>("Assets/CG/九州切换.png")?.ToSprite();
            Sprite down = Main.Res.LoadAsset<Texture2D>("Assets/CG/九州切换_down.png")?.ToSprite();
            Sprite enter = Main.Res.LoadAsset<Texture2D>("Assets/CG/九州切换_enter.png")?.ToSprite();

            if (normal != null)
            {
                btnObj.GetComponent<Image>().sprite = normal;
                btn.nomalSprite = normal;
            }
            if (down != null) btn.mouseDownSprite = down;
            if (enter != null) btn.mouseEnterSprite = enter;

            btnObj.AddComponent<MainPanelButtonAnimation>();
            Main.FGUI.ResetCamera();

            return true;
        }

        // 觅长生2
        [HarmonyPostfix]
        public static void OpenMain_Patch(MainUIMag __instance)
        {
            string path = "Assets/CG/觅长生2.png";
            Texture2D texture = Main.Res.LoadAsset<Texture2D>(path);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), // pivot 居中
                100f // pixels per unit，可根据原图调整
            );
            GameObject targetObj = GameObject.Find("NewMain(Clone)/tree+logo/觅长生图标");
            Image image = targetObj.GetComponent<Image>();
            image.sprite = sprite;
            GameWindowTitle.Inst.SetTitle($"觅长生2 九州巡礼 0.8.0");
        }
    }
}