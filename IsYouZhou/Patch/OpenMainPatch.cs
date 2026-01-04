using HarmonyLib;
using SkySwordKill.Next;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(MainUIMag), "OpenMain")]
    public class OpenMainPatch
    {
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
        }
    }
}