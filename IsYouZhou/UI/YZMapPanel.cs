﻿using UnityEngine;
using UnityEngine.UI;
using YSGame;
using System.Collections.Generic;
using top.Isteyft.MCS.YouZhou.Utils;
using SkySwordKill.Next.DialogSystem;

namespace top.Isteyft.MCS.YouZhou.UI
{
    public class YZMapPanel : MonoBehaviour, IESCClose
    {
        public static YZMapPanel Inst { get; private set; }

        // UI组件引用
        private Button _closeBtn;
        private Image _bg;
        private List<Button> _locationBtns = new List<Button>();
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private GameObject _maskObject; // 遮罩对象

        public static void Show()
        {
            // 如果实例已存在且处于激活状态，则不再重复创建
            if (Inst != null && Inst.gameObject.activeInHierarchy)
            {
                Inst.EnsureVisible();
                return;
            }

            try
            {
                IsToolsMain.LogInfo("开始加载YZMapPanel...");

                var uiManager = IsToolsMain.I?.UIManagerHandle;
                if (uiManager == null)
                {
                    IsToolsMain.Warning("UIManagerHandle not found!");
                    return;
                }

                if (!uiManager.prefabBank.TryGetValue("YZMapUI", out GameObject prefab))
                {
                    IsToolsMain.Warning("YZMapUI prefab not found!");
                    return;
                }

                var canvas = Object.FindObjectOfType<NewUICanvas>()?.transform;
                if (canvas == null)
                {
                    IsToolsMain.Warning("NewUICanvas not found!");
                    return;
                }

                // 实例化并初始化
                var instanceObj = Object.Instantiate(prefab, canvas);
                instanceObj.name = "YZMapPanel(Instance)"; // 重命名以便调试

                Inst = instanceObj.AddComponent<YZMapPanel>();
                Inst._rectTransform = instanceObj.GetComponent<RectTransform>();

                // 创建遮罩
                Inst.CreateMask(canvas);

                // 设置初始位置到右上角
                Inst.SetPositionToRightTop();

                // 确保面板可见性
                Inst.EnsureVisible();

                if (!Inst.InitComponents())
                {
                    Object.Destroy(instanceObj);
                    return;
                }

                ESCCloseManager.Inst.RegisterClose(Inst);
                IsToolsMain.LogInfo("YZMapPanel初始化完成");
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"YZMapPanel.Show error: {ex}");
            }
        }

        private bool InitComponents()
        {
            try
            {
                // 初始化CanvasGroup（如果不存在则添加）
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;

                // 获取基础组件
                _closeBtn = transform.Find("CloseBtn")?.GetComponent<Button>();
                _bg = transform.Find("BG")?.GetComponent<Image>();

                if (_closeBtn == null || _bg == null)
                {
                    IsToolsMain.Warning("Required components (CloseBtn/BG) not found!");
                    return false;
                }

                // 获取当前场景名称
                string currentScene = Tools.getScreenName();

                // 初始化位置按钮 (2Btn-18Btn)
                for (int i = 2; i <= 18; i++)
                {
                    string btnName = $"{i}Btn";
                    var btn = transform.Find(btnName)?.GetComponent<Button>();

                    if (btn != null)
                    {
                        int locationId = i;
                        
                        // 只有8-12号按钮（城市）可以点击
                        bool isClickable = locationId >= 8 && locationId <= 12;
                        
                        // 检查是否是当前所在的城市,如果是则禁用该按钮
                        bool isCurrentLocation = IsCurrentLocation(locationId, currentScene);
                        
                        if (isClickable && !isCurrentLocation)
                        {
                            btn.onClick.AddListener(() => OnLocationClicked(locationId));
                            btn.interactable = true;
                        }
                        else
                        {
                            btn.interactable = false;
                            // 可选：为不可点击的按钮设置半透明效果
                            var btnImage = btn.GetComponent<Image>();
                            if (btnImage != null)
                            {
                                Color color = btnImage.color;
                                color.a = 0.75f; // 半透明
                                btnImage.color = color;
                            }
                        }
                        
                        _locationBtns.Add(btn);
                        btn.gameObject.SetActive(true); // 确保按钮激活
                    }
                    else
                    {
                        IsToolsMain.Warning($"Button {btnName} not found!");
                    }
                }

                _closeBtn.onClick.AddListener(Close);

                // 设置默认位置和大小
                if (_rectTransform != null)
                {
                    _rectTransform.anchoredPosition = Vector2.zero;
                    _rectTransform.localScale = Vector3.one;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"InitComponents error: {ex}");
                return false;
            }
        }

        private void EnsureVisible()
        {
            if (gameObject == null) return;

            // 确保对象激活
            gameObject.SetActive(true);

            // 设置显示层级
            transform.SetAsLastSibling();

            // 确保CanvasGroup设置正确
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            // 强制布局重建
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        private void OnLocationClicked(int locationId)
        {
            MusicMag.instance.PlayEffectMusic("1打开关闭tab界面", 1f);
            //UIPopTip.Inst.Pop($"已选择位置 {locationId}", PopTipIconType.叹号);

            //// 这里可以添加具体的位置点击逻辑
            //// 例如：在地图上显示标记、导航到该位置等
            string eventStr = GetEventString(locationId);
            if (!string.IsNullOrEmpty(eventStr))
            {
                DialogAnalysis.StartDialogEvent(eventStr, null);
            }
            else
            {
                Debug.LogWarning($"未找到locationId {locationId} 对应的事件");
            }
            Close();
        }

        public void Close()
        {
            try
            {
                if (Inst != null)
                {
                    ESCCloseManager.Inst.UnRegisterClose(this);
                    // 销毁遮罩
                    if (_maskObject != null)
                    {
                        Object.Destroy(_maskObject);
                        _maskObject = null;
                    }
                    Object.Destroy(gameObject);
                    Inst = null;
                }
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"Close error: {ex}");
            }
        }

        public bool TryEscClose()
        {
            Close();
            return true;
        }

        void OnDestroy()
        {
            if (Inst == this)
            {
                Inst = null;
            }
        }


        /// <summary>
        /// 创建全屏遮罩，阻止点击范围外的UI
        /// </summary>
        private void CreateMask(Transform canvas)
        {
            try
            {
                // 创建遮罩对象
                _maskObject = new GameObject("YZMapPanel_Mask");
                _maskObject.transform.SetParent(canvas, false);

                // 设置为面板的前一个兄弟节点（让遮罩在面板下层）
                _maskObject.transform.SetSiblingIndex(transform.GetSiblingIndex());

                // 添加RectTransform并设置为全屏
                RectTransform maskRect = _maskObject.AddComponent<RectTransform>();
                maskRect.anchorMin = Vector2.zero;
                maskRect.anchorMax = Vector2.one;
                maskRect.offsetMin = Vector2.zero;
                maskRect.offsetMax = Vector2.zero;

                // 添加Image组件作为遮罩背景（半透明黑色）
                Image maskImage = _maskObject.AddComponent<Image>();
                maskImage.color = new Color(0, 0, 0, 0.5f); // 半透明黑色

                // 添加Button组件以阻止点击穿透
                Button maskButton = _maskObject.AddComponent<Button>();
                maskButton.transition = UnityEngine.UI.Selectable.Transition.None; // 无过渡效果
                maskButton.onClick.AddListener(() => 
                {
                    // 点击遮罩时关闭面板（可选）
                    // Close();
                    // 或者播放提示音表示无法点击
                    MusicMag.instance.PlayEffectMusic("3警告", 0.5f);
                });

                IsToolsMain.LogInfo("遮罩创建成功");
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"CreateMask error: {ex}");
            }
        }

        /// <summary>
        /// 将面板定位到右上角（基于中心坐标系）
        /// </summary>
        private void SetPositionToRightTop()
        {
            if (_rectTransform == null) return;

            // 重置锚点和轴心到中心
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // 获取Canvas尺寸
            RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            float canvasWidth = canvasRect.rect.width;
            float canvasHeight = canvasRect.rect.height;

            // 获取面板尺寸
            float panelWidth = _rectTransform.rect.width;
            float panelHeight = _rectTransform.rect.height;

            // 计算从中心到右上角的偏移量
            float posX = (canvasWidth / 2) - (panelWidth / 2) - 20; // 右边留20像素边距
            float posY = (canvasHeight / 2) - (panelHeight / 2) - 20; // 上边留20像素边距

            // 设置位置
            _rectTransform.localPosition = new Vector3(posX, posY, 0);

            IsToolsMain.LogInfo($"面板定位到右上角，位置：{_rectTransform.localPosition} " +
                              $"Canvas尺寸：{canvasWidth}x{canvasHeight} " +
                              $"面板尺寸：{panelWidth}x{panelHeight}");
        }


        /// <summary>
        /// 检查指定位置ID是否是当前所在场景
        /// </summary>
        private bool IsCurrentLocation(int locationId, string currentScene)
        {
            // 根据locationId获取对应的场景ID,然后与当前场景比对
            switch (locationId)
            {
                case 8:  return currentScene == "S27300"; // 长风城
                case 9:  return currentScene == "S27320"; // 浅湾城
                case 10: return currentScene == "S27340"; // 幽篁城
                case 11: return currentScene == "S27380"; // 天魔城
                case 12: return currentScene == "S27360"; // 上雪城
                default: return false;
            }
        }

        // 事件名称
        private string GetEventString(int locationId)
        {
            // 根据提供的locationId和event对应关系返回相应的字符串
            switch (locationId)
            {
                case 2: return "幽州-进入天魔道";
                case 3: return "幽州-进入天衍阵宗";
                case 4: return "幽州-进入金刚宗";
                case 5: return "幽州-进入尘山";
                case 6: return "幽州-进入苍雾山";
                case 7: return "幽州-进入长暮山";
                case 8: return "幽州-进入长风城";
                case 9: return "幽州-进入浅湾城";
                case 10: return "幽州-进入幽篁城";
                case 11: return "幽州-进入天魔城";
                case 12: return "幽州-进入上雪城";
                case 13: return "幽州-进入北雪九峰";
                case 14: return "幽州-进入沽幽山";
                case 15: return "幽州-进入破败小村";
                case 16: return "幽州-进入猪神教";
                case 17: return "幽州-进入丘魏矿脉";
                case 18: return "幽州-进入天雪剑宗";
                default: return null;
            }
        }
    }

}