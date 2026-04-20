using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using UnityEngine.UI;
using YSGame;

namespace top.Isteyft.MCS.JiuZhou.UI
{
    /// <summary>
    /// 家族UI面板管理类
    /// </summary>
    public class JiaZuPanel : MonoBehaviour, IESCClose
    {
        public static JiaZuPanel Inst { get; private set; }

        // UI组件引用
        private Image _bg;
        private Text _moneyCount;
        private Text _zouZheCount;
        private Button _closeButton;
        private Button _yesButton;
        private Button _noButton;
        private Text _yesButtonText;
        private Text _noButtonText;
        private Text _titleText; // 标题文本
        private Text _contextText;
        
        private GameObject _zouZheUI; // 走者UI容器
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private GameObject _maskObject; // 全屏遮罩对象

        /// <summary>
        /// 显示家族UI面板
        /// </summary>
        public static void Show()
        {
            // 如果实例已存在且处于激活状态，则不再重复创建
            if (Inst != null && Inst.gameObject.activeInHierarchy)
            {
                Inst.EnsureVisible();
                return;
            }

            var uiManager = IsToolsMain.I?.UIManagerHandle;
            if (uiManager == null)
            {
                IsToolsMain.Warning("UIManagerHandle not found!");
                return;
            }

            if (!uiManager.prefabBank.TryGetValue("JiaZuUI", out GameObject prefab))
            {
                IsToolsMain.Warning("JiaZuUI预制体没找到");
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
            instanceObj.name = "JiaZuPanel(Instance)"; // 重命名以便调试

            Inst = instanceObj.AddComponent<JiaZuPanel>();
            Inst._rectTransform = instanceObj.GetComponent<RectTransform>();

            // 创建遮罩
            Inst.CreateMask(canvas);

            // 设置初始位置到屏幕中央
            Inst.SetPositionToCenter();

            // 确保面板可见性
            Inst.EnsureVisible();

            if (!Inst.InitComponents())
            {
                Object.Destroy(instanceObj);
                return;
            }

            ESCCloseManager.Inst.RegisterClose(Inst);
            IsToolsMain.LogInfo("JiaZuPanel初始化完成");
        }

        /// <summary>
        /// 初始化UI组件
        /// </summary>
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
                _bg = transform.Find("BG")?.GetComponent<Image>();
                _closeButton = transform.Find("CloseButton")?.GetComponent<Button>();
                
                IsToolsMain.LogInfo($"BG: {(_bg != null ? "找到" : "未找到")}");
                IsToolsMain.LogInfo($"CloseButton: {(_closeButton != null ? "找到" : "未找到")}");
                
                // 获取Money相关组件
                Transform moneyTrans = transform.Find("Money");
                if (moneyTrans != null)
                {
                    _moneyCount = moneyTrans.Find("MoneyCount")?.GetComponent<Text>();
                }

                // 获取ZouZhe相关组件
                Transform zouZheTrans = transform.Find("ZouZhe");
                if (zouZheTrans != null)
                {
                    IsToolsMain.LogInfo($"找到ZouZhe节点，子对象数量: {zouZheTrans.childCount}");
                    
                    _zouZheCount = zouZheTrans.Find("ZouZheCount")?.GetComponent<Text>();
                    
                    IsToolsMain.LogInfo($"ZouZheCount: {(_zouZheCount != null ? "找到" : "未找到")}");
                    
                    // 获取ZouZheUI子对象
                    _zouZheUI = zouZheTrans.Find("ZouZheUI")?.gameObject;
                    if (_zouZheUI != null)
                    {
                        _noButton = _zouZheUI.transform.Find("NoButton")?.GetComponent<Button>();
                        _yesButton = _zouZheUI.transform.Find("YesButton")?.GetComponent<Button>();
                        
                        if (_noButton != null)
                        {
                            _noButtonText = _noButton.transform.Find("Text")?.GetComponent<Text>();
                        }
                        
                        if (_yesButton != null)
                        {
                            _yesButtonText = _yesButton.transform.Find("Text")?.GetComponent<Text>();
                        }
                        
                        Transform titleTrans = _zouZheUI.transform.Find("HightLighting");
                        if (titleTrans != null)
                        {
                            _titleText = titleTrans.Find("Text")?.GetComponent<Text>();
                        }
                        
                        _contextText = _zouZheUI.transform.Find("Context")?.GetComponent<Text>();
                    }
                }

                // 检查必需组件
                if (_bg == null)
                {
                    IsToolsMain.Warning("Required component BG not found!");
                    return false;
                }

                // 设置按钮事件
                if (_closeButton != null)
                {
                    _closeButton.onClick.AddListener(Close);
                    IsToolsMain.LogInfo("家族UI - CloseButton事件绑定成功");
                }
                else
                {
                    IsToolsMain.Warning("家族UI - CloseButton未找到！");
                }

                if (_yesButton != null)
                {
                    _yesButton.onClick.AddListener(OnYesButtonClicked);
                }

                if (_noButton != null)
                {
                    _noButton.onClick.AddListener(OnNoButtonClicked);
                }

                // 初始化文本内容（从DialogAnalysis获取数据）
                UpdateMoneyCount();
                UpdateZouZheCount();
                
                if (_yesButtonText != null) _yesButtonText.text = "是";
                if (_noButtonText != null) _noButtonText.text = "否";
                if (_titleText != null) _titleText.text = "家族系统";
                if (_contextText != null) _contextText.text = "家族信息";

                // 设置默认位置和大小
                if (_rectTransform != null)
                {
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

        /// <summary>
        /// 确保面板可见
        /// </summary>
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

        /// <summary>
        /// 创建全屏遮罩，阻止点击穿透
        /// </summary>
        private void CreateMask(Transform canvas)
        {
            try
            {
                // 创建遮罩对象
                _maskObject = new GameObject("JiaZuPanel_Mask");
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
                    // 点击遮罩时播放提示音表示无法点击
                    MusicMag.instance.PlayEffectMusic("3警告", 0.5f);
                });

                IsToolsMain.LogInfo("家族UI遮罩创建成功");
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"CreateMask error: {ex}");
            }
        }

        /// <summary>
        /// 将面板定位到屏幕中央
        /// </summary>
        private void SetPositionToCenter()
        {
            if (_rectTransform == null) return;

            // 设置锚点和轴心到中心
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // 设置位置为中心
            _rectTransform.localPosition = Vector3.zero;

            IsToolsMain.LogInfo($"家族UI面板定位到中央，位置：{_rectTransform.localPosition}");
        }

        /// <summary>
        /// 更新金钱显示（从DialogAnalysis获取）
        /// </summary>
        public void UpdateMoneyCount()
        {
            if (_moneyCount != null)
            {
                int money = DialogAnalysis.GetInt("幽州-家族-灵石");
                _moneyCount.text = money.ToString();
            }
        }

        /// <summary>
        /// 更新奏折数量显示（从DialogAnalysis获取）
        /// </summary>
        public void UpdateZouZheCount()
        {
            if (_zouZheCount != null)
            {
                int zouZhe = DialogAnalysis.GetInt("幽州-家族-奏折");
                _zouZheCount.text = zouZhe.ToString();
            }
        }

        /// <summary>
        /// 获取当前灵石数量
        /// </summary>
        public int GetMoney()
        {
            return DialogAnalysis.GetInt("幽州-家族-灵石");
        }

        /// <summary>
        /// 设置灵石数量
        /// </summary>
        public void SetMoney(int money)
        {
            DialogAnalysis.SetInt("幽州-家族-灵石", money);
            UpdateMoneyCount();
        }

        /// <summary>
        /// 获取当前奏折数量
        /// </summary>
        public int GetZouZhe()
        {
            return DialogAnalysis.GetInt("幽州-家族-奏折");
        }

        /// <summary>
        /// 设置奏折数量
        /// </summary>
        public void SetZouZhe(int count)
        {
            DialogAnalysis.SetInt("幽州-家族-奏折", count);
            UpdateZouZheCount();
        }

        /// <summary>
        /// 设置上下文文本
        /// </summary>
        public void SetContextText(string text)
        {
            if (_contextText != null)
            {
                _contextText.text = text;
            }
        }

        /// <summary>
        /// 设置标题文本
        /// </summary>
        public void SetTitleText(string text)
        {
            if (_titleText != null)
            {
                _titleText.text = text;
            }
        }

        /// <summary>
        /// 显示/隐藏走者UI
        /// </summary>
        public void ShowZouZheUI(bool show)
        {
            if (_zouZheUI != null)
            {
                _zouZheUI.SetActive(show);
            }
        }

        /// <summary>
        /// Yes按钮点击事件
        /// </summary>
        private void OnYesButtonClicked()
        {
            IsToolsMain.LogInfo("家族UI - Yes按钮被点击");
            MusicMag.instance.PlayEffectMusic("1打开关闭tab界面", 1f);
            
            // TODO: 在这里添加Yes按钮的具体逻辑
            
            // 示例：关闭面板
            Close();
        }

        /// <summary>
        /// No按钮点击事件
        /// </summary>
        private void OnNoButtonClicked()
        {
            IsToolsMain.LogInfo("家族UI - No按钮被点击");
            MusicMag.instance.PlayEffectMusic("1打开关闭tab界面", 1f);
            
            // TODO: 在这里添加No按钮的具体逻辑
            
            // 示例：关闭面板
            Close();
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void Close()
        {
            try
            {
                IsToolsMain.LogInfo("开始关闭家族UI面板...");
                
                // 注销ESC关闭
                if (ESCCloseManager.Inst != null)
                {
                    ESCCloseManager.Inst.UnRegisterClose(this);
                }
                
                // 销毁遮罩
                if (_maskObject != null)
                {
                    IsToolsMain.LogInfo("销毁遮罩对象");
                    Object.Destroy(_maskObject);
                    _maskObject = null;
                }
                
                // 清空静态实例引用
                Inst = null;
                
                // 销毁面板对象
                if (gameObject != null)
                {
                    IsToolsMain.LogInfo("销毁面板对象");
                    Object.Destroy(gameObject);
                }
                
                IsToolsMain.LogInfo("家族UI面板关闭完成");
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"JiaZuPanel.Close error: {ex}");
            }
        }

        /// <summary>
        /// ESC关闭接口实现
        /// </summary>
        public bool TryEscClose()
        {
            Close();
            return true;
        }

        void OnDestroy()
        {
            IsToolsMain.LogInfo("家族UI面板OnDestroy被调用");
            
            // 确保遮罩被销毁
            if (_maskObject != null)
            {
                Object.Destroy(_maskObject);
                _maskObject = null;
            }
            
            // 清空静态实例
            if (Inst == this)
            {
                Inst = null;
            }
        }
    }
}
