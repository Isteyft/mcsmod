using System.Collections.Generic;
using top.Isteyft.MCS.IsTools;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.IsTools.Patch {
    public static class MusicScanner
    {
        /// <summary>
        /// 获取游戏中所有音乐的名称（背景音乐 + 音效）
        /// </summary>
        public static List<string> GetAllMusicNames()
        {
            List<string> musicNames = new List<string>();

            // 确保 MusicMag 已初始化
            if (MusicMag.instance == null)
            {
                IsToolsMain.Error("MusicMag.instance 为 null，请确保游戏已初始化");
                return musicNames;
            }

            // 遍历背景音乐
            foreach (var musicInfo in MusicMag.instance.BackGroudMusic)
            {
                if (musicInfo.audioClip != null)
                {
                    musicNames.Add(musicInfo.name);
                    IsToolsMain.Log($"背景音乐: {musicInfo.name} (文件名: {musicInfo.audioClip.name})");
                }
            }

            // 遍历音效
            foreach (var musicInfo in MusicMag.instance.EffectMusic)
            {
                if (musicInfo.audioClip != null)
                {
                    musicNames.Add(musicInfo.name);
                    IsToolsMain.Log($"音效: {musicInfo.name} (文件名: {musicInfo.audioClip.name})");
                }
            }

            return musicNames;
        }
    }
}