using BepInEx;
using Fungus;
using HarmonyLib;
using SkySwordKill.Next;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace top.Isteyft.MCS.NewIndex
{
    [BepInDependency("skyswordkill.plugin.Next")]
    [BepInPlugin("top.Isteyft.MCS.Screen", "壁纸", "1.0.0")]
    public class ScreenMain : BaseUnityPlugin
    {
        public static ScreenMain I { get; private set; }
        public static List<string> ModMP4Paths = new List<string>();

        private void Start()
        {
            I = this;
            new Harmony("top.Isteyft.MCS.NewIndex").PatchAll();
            Logger.LogInfo("Main background loaded.");
            LoadAllScene();
        }

        private void LoadAllScene()
        {
            List<DirectoryInfo> allModDirectory = WorkshopTool.GetAllModDirectory();
            string text = "";
            foreach (DirectoryInfo directoryInfo in allModDirectory)
            {
                if (!WorkshopTool.CheckModIsDisable(directoryInfo.Name))
                {
                    LoadMP4(directoryInfo.FullName);
                }
            }
            DirectoryInfo directoryInfo2 = new DirectoryInfo(Application.dataPath + "/../本地Mod测试");
            if (directoryInfo2.Exists)
            {
                foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories())
                {
                    LoadMP4(directoryInfo3.FullName);
                }
            }
            Logger.LogInfo($"加载{ModMP4Paths.Count}个视频路径");
        }
        private void LoadMP4(string modPath)
        {
            string path = Path.Combine(modPath, "plugins", "BaizeAssets", "BG");
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path, "*.MP4"); // 查找MP4文件
            string[] filesLower = Directory.GetFiles(path, "*.mp4"); // 查找mp4文件
            var allFiles = files.Concat(filesLower).ToArray();
            ModMP4Paths.AddRange(allFiles);
        }
    }

    [HarmonyPatch(typeof(MainUIMag), "OpenMain")]
    public class OpenMainPatch
    {
        // --- 成员变量 ---
        // 使用ScreenMain中定义的列表，不再硬编码数量
        private static List<string> availableVideoPaths = ScreenMain.ModMP4Paths; // 引用全局列表
        private static int totalVideoCount = 0; // 动态获取列表长度
        private static int totalStates = 0; // 包括原始状态

        // 用于视频播放的组件
        private static VideoPlayer currentVideoPlayer = null;
        private static RawImage videoRawImage = null; // 用于显示视频的UI元素
        private static GameObject videoPlayerGO = null; // 存储VideoPlayer所在的GameObject
        private static bool isVideoMuted = false; // 视频是否静音

        // 缓存原始背景
        private static Image originalBgImageComponent = null; // 存储原始Image组件引用
        private static Sprite originalBgSprite = null; // 保存原始背景精灵
        private static GameObject originalBgObject = null; // 保存原始背景GameObject引用

        private static int currentVideoIndex = 0; // 当前视频索引，0 表示原始背景

        /// <summary>
        /// 停止并清理之前的视频
        /// </summary>
        private static void CleanupPreviousVideo()
        {
            if (currentVideoPlayer != null)
            {
                currentVideoPlayer.Stop();
                currentVideoPlayer.enabled = false; // 先禁用再销毁
                UnityEngine.Object.Destroy(currentVideoPlayer.targetTexture); // 销毁RenderTexture
                UnityEngine.Object.Destroy(videoPlayerGO); // 销毁VideoPlayer所在的GameObject
                currentVideoPlayer = null;
                videoPlayerGO = null;
            }
            if (videoRawImage != null)
            {
                videoRawImage.texture = null; // 清除texture引用
                videoRawImage.gameObject.SetActive(false); // 隐藏视频UI
                videoRawImage = null;
            }
        }

        /// <summary>
        /// 恢复原始背景
        /// </summary>
        private static void RestoreOriginalBackground()
        {
            CleanupPreviousVideo(); // 清理视频

            // 显示原始背景
            if (originalBgImageComponent != null)
            {
                originalBgImageComponent.sprite = originalBgSprite; // 恢复原始精灵
                originalBgImageComponent.enabled = true; // 启用原始Image
            }
            //if (originalBgObject != null)
            //{
            //    originalBgObject.SetActive(true); // 启用原始背景GameObject
            //}
        }

        /// <summary>
        /// 加载并播放指定索引的视频
        /// </summary>
        /// <param name="index">视频在列表中的索引</param>
        private static void LoadAndPlayVideo(int index)
        {
            if (index < 0 || index >= availableVideoPaths.Count)
            {
                Main.LogError($"[壁纸] 视频索引 {index} 超出范围 (0 到 {availableVideoPaths.Count - 1}) 或列表为空。");
                RestoreOriginalBackground(); // 索引无效时恢复原背景
                return;
            }

            string videoPath = availableVideoPaths[index];
            Main.LogInfo($"[壁纸] 尝试加载: {videoPath}");

            // 1. 销毁之前的视频播放器和UI（如果有）
            CleanupPreviousVideo();

            // 2. 创建用于播放视频的GameObject
            videoPlayerGO = new GameObject("MainUIVideoPlayer");
            currentVideoPlayer = videoPlayerGO.AddComponent<VideoPlayer>();
            GameObject rawImageGO = new GameObject("VideoRawImage"); // 创建RawImage的GameObject

            // 3. 获取Canvas或父UI容器的RectTransform (假设主界面是Canvas下的)
            Transform parentTransform = originalBgObject.transform.parent; // 尝试使用原始背景的父级作为容器
            if (parentTransform == null)
            {
                Main.LogWarning("[壁纸] 未能找到合适的父级UI容器，可能需要手动调整视频UI的位置。");
            }
            else
            {
                rawImageGO.transform.SetParent(parentTransform, false); // 设置父级
            }

            // 4. 设置RawImage组件
            videoRawImage = rawImageGO.AddComponent<RawImage>();
            RectTransform rectTrans = rawImageGO.GetComponent<RectTransform>();
            rectTrans.anchorMin = Vector2.zero;      // 左下角
            rectTrans.anchorMax = Vector2.one;       // 右上角
            rectTrans.anchoredPosition = Vector2.zero; // 居中
            rectTrans.sizeDelta = Vector2.zero;      // 拉伸填充父容器
            rectTrans.pivot = new Vector2(0.5f, 0.5f);   // 中心轴心点

            // 5. 创建RenderTexture (可以根据视频分辨率或屏幕分辨率动态调整)
            // 这里暂时用一个固定分辨率，实际应用中可能需要优化
            RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
            renderTexture.Create();
            // 6. 配置VideoPlayer
            currentVideoPlayer.source = VideoSource.Url; // 使用URL方式加载本地文件
                                                         // 注意：Unity中从文件系统加载需要 "file://" 协议
                                                         // 确保 videoPath 是一个完整的文件系统路径
            if (!videoPath.StartsWith("file://"))
            {
                // 如果路径不是以 "file://" 开头，则加上它
                // 这里假设 videoPath 本身就是完整路径，如 C:\... 或 /home/...
                // 如果路径是相对于某个根目录的，需要先转换为完整路径
                // 例如，如果路径是相对于游戏目录的，可以这样处理：
                // string absolutePath = Path.Combine(Application.dataPath, "..", videoPath); // 如果videoPath是相对路径
                // currentVideoPlayer.url = "file://" + absolutePath;
                // 但因为我们是从LoadMP4直接获取的完整路径，所以直接加 "file://"
                currentVideoPlayer.url = "file://" + videoPath;
            }
            else
            {
                // 如果路径已经是 file:// 格式，则直接赋值
                currentVideoPlayer.url = videoPath;
            }

            currentVideoPlayer.targetTexture = renderTexture; // 设置渲染目标为RenderTexture
            currentVideoPlayer.isLooping = true; // 设置循环播放
            currentVideoPlayer.playOnAwake = false; // 不自动播放
                                                    // currentVideoPlayer.audioOutputMode = VideoAudioOutputMode.Direct; // 根据需要设置音频输出

            currentVideoPlayer.SetDirectAudioMute(0, isVideoMuted); // 应用当前静音状态

            // 7. 将RenderTexture赋给RawImage
            videoRawImage.texture = renderTexture;

            // 8. 激活视频UI
            videoRawImage.gameObject.SetActive(true);

            // 9. 开始播放视频
            currentVideoPlayer.Play();
            rawImageGO.transform.SetSiblingIndex(0);
        }


        [HarmonyPrefix]
        public static bool OpenMain_Prefix_Patch(MainUIMag __instance)
        {
            // --- 初始化视频路径列表信息 (首次进入时) ---
            totalVideoCount = availableVideoPaths.Count;
            totalStates = totalVideoCount + 1; // 视频数量 + 1 (原始背景)

            // 获取原始背景组件和对象
            originalBgObject = __instance.新主界面.transform.Find("bg1").gameObject;
            originalBgImageComponent = originalBgObject.GetComponent<Image>();

            // 首次进入时保存原始背景精灵
            if (originalBgSprite == null && originalBgImageComponent != null)
            {
                originalBgSprite = originalBgImageComponent.sprite;
                // --- 新增逻辑：首次加载时，如果有视频则默认播放第一个视频 ---
                if (totalVideoCount > 0)
                {
                    currentVideoIndex = 1; // 设置为第一个视频
                }
            }

            // --- 决定显示哪个背景 ---
            // currentVideoIndex 从 0 开始，0 代表原始背景
            if (currentVideoIndex == 0 && totalVideoCount == 0)
            {
                RestoreOriginalBackground(); // 显示原始背景
            }
            else if (currentVideoIndex <= totalVideoCount) // 索引从 1 开始对应第一个视频
            {
                // LoadAndPlayVideo 期望的是列表索引 (从 0 开始)
                // currentVideoIndex 为 1 时，应播放列表索引 0 的视频
                LoadAndPlayVideo(currentVideoIndex - 1);
            }
            else
            {
                // 如果索引超出范围（理论上不应该发生，因为取模运算），也恢复原背景
                Main.LogWarning($"[壁纸] currentVideoIndex ({currentVideoIndex}) 超出预期范围。恢复原始背景。");
                RestoreOriginalBackground();
            }


            // --- 原有的按钮创建和逻辑 ---
            GameObject btnObj = __instance.新主界面.transform.Find("Panel/btn/神仙斗法")
                .gameObject.CopyGameObject(null, "切换");
            btnObj.transform.MoveLocal(new Vector3(0f, 402f, 0f));

            FpBtn btn = btnObj.GetComponent<FpBtn>();
            btn.mouseUpEvent = new UnityEvent();
            btn.mouseUpEvent.AddListener(delegate ()
            {
                // 循环切换：0 (原始) -> 1 (视频1) -> ... -> N (视频N) -> 0 (原始) ...
                // currentVideoIndex 的范围是 [0, totalStates)
                currentVideoIndex = (currentVideoIndex + 1) % totalStates;
                Main.LogInfo($"[壁纸] 切换到索引: {currentVideoIndex}, 总状态数: {totalStates}"); // 调试日志

                // 根据新的索引决定显示视频还是原始背景
                if (currentVideoIndex == 0 && totalVideoCount == 0)
                {
                    RestoreOriginalBackground(); // 显示原始背景
                }
                else if (currentVideoIndex <= totalVideoCount)
                {
                    if (currentVideoIndex == 0)
                    {
                        currentVideoIndex += 1;
                    }
                    LoadAndPlayVideo(currentVideoIndex - 1); // 播放对应列表索引的视频
                }
                else
                {
                    // 理论上不会到这里，因为取模了
                    RestoreOriginalBackground();
                }
            });

            // 设置按钮外观（保持不变）
            Sprite normal = Main.Res.LoadAsset<Texture2D>("Assets/CG/切换.png")?.ToSprite();
            Sprite down = Main.Res.LoadAsset<Texture2D>("Assets/CG/切换_down.png")?.ToSprite();
            Sprite enter = Main.Res.LoadAsset<Texture2D>("Assets/CG/切换_enter.png")?.ToSprite();

            if (normal != null)
            {
                btnObj.GetComponent<Image>().sprite = normal;
                btn.nomalSprite = normal;
            }
            if (down != null) btn.mouseDownSprite = down;
            if (enter != null) btn.mouseEnterSprite = enter;

            btnObj.AddComponent<MainPanelButtonAnimation>();

            // --- 音量开关按钮 ---
            GameObject volumeBtnObj = __instance.新主界面.transform.Find("Panel/btn/神仙斗法")
                .gameObject.CopyGameObject(null, "音量开关");
            volumeBtnObj.transform.MoveLocal(new Vector3(86f, 402f, 0f));

            FpBtn volumeBtn = volumeBtnObj.GetComponent<FpBtn>();
            volumeBtn.mouseUpEvent = new UnityEvent();
            volumeBtn.mouseUpEvent.AddListener(delegate ()
            {
                // 切换静音状态
                isVideoMuted = !isVideoMuted;

                // 应用到当前播放的视频
                if (currentVideoPlayer != null)
                {
                    currentVideoPlayer.SetDirectAudioMute(0, isVideoMuted);
                }

                Main.LogInfo($"[壁纸] 视频音量: {(isVideoMuted ? "静音" : "开启")}");
            });

            // 设置按钮外观
            Sprite volumeNormal = Main.Res.LoadAsset<Texture2D>("Assets/CG/声音开关.png")?.ToSprite();
            Sprite volumeDown = Main.Res.LoadAsset<Texture2D>("Assets/CG/音量开关_down.png")?.ToSprite();
            Sprite volumeEnter = Main.Res.LoadAsset<Texture2D>("Assets/CG/音量开关_enter.png")?.ToSprite();

            if (volumeNormal != null)
            {
                volumeBtnObj.GetComponent<Image>().sprite = volumeNormal;
                volumeBtn.nomalSprite = volumeNormal;
            }
            if (volumeDown != null) volumeBtn.mouseDownSprite = volumeDown;
            if (volumeEnter != null) volumeBtn.mouseEnterSprite = volumeEnter;

            volumeBtnObj.AddComponent<MainPanelButtonAnimation>();

            Main.FGUI.ResetCamera();

            return true;
        }

        [HarmonyPostfix]
        public static void OpenMain_Patch(MainUIMag __instance)
        {
            GameObject.Find("NewMain(Clone)/tree+logo/树").SetActive(false);
        }
    }

}