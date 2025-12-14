using Fungus;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Data;
using UnityEngine;
using UnityEngine.UI;
using YSGame.Fight;

namespace top.Isteyft.MCS.IsTools.Patch
{
    [HarmonyPatch(typeof(RoundManager), "initAvatarInfo")]
    public class AddDaoJuPatch
    {
        private static readonly KeyCode[] HomeRowKeys = {
            KeyCode.None, KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I,
            KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.S, KeyCode.U,
            KeyCode.V, KeyCode.X, KeyCode.Z, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Minus,
            KeyCode.Equals, KeyCode.LeftBracket, KeyCode.RightBracket, KeyCode.Semicolon, KeyCode.Quote
        };

        // 静态字典：保存技能索引和道具ID的对应关系
        private static Dictionary<int, int> skillIndexToItemIDMap = new Dictionary<int, int>();

        [HarmonyPostfix]
        public static void PostfixMethod(KBEngine.Avatar avatar)
        {
            if (!avatar.isPlayer()) return;
            if (Tools.instance.monstarMag.FightType == StartFight.FightEnumType.ZhuJi ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.JieDan ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.JieYing ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.HuaShen ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.无装备无丹药擂台 ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.天劫秘术领悟 ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.DuJie ||
                Tools.instance.monstarMag.FightType == StartFight.FightEnumType.FeiSheng)
            {
                IsToolsMain.LogInfo("属于筑基，结丹，结婴，化神，无装备无丹药擂台，天劫秘术感悟不加载道具");
                return;
            }

            // 每次战斗开始时清除技能索引和道具ID的对应关系
            skillIndexToItemIDMap.Clear();

            // 收集需要显示的道具（玩家已拥有的道具）
            var ownedDaoJus = new List<DaoJuData>();
            foreach (var daoJu in DaoJuData.data)
            {
                if (avatar.hasItem(daoJu.DaoJuItem))
                {
                    IsToolsMain.LogInfo($"拥有{daoJu.DaoJuItem},加载技能{daoJu.DaoJuSkill}");
                    ownedDaoJus.Add(daoJu);
                }
            }

            if (ownedDaoJus.Count == 0)
            {
                return;
            }

            var uiFightPanel = UIFightPanel.Inst;
            if (uiFightPanel == null) return;

            // 确保技能栏有足够的槽位（在原有10个基础上扩展）
            EnsureSkillSlots(uiFightPanel, ownedDaoJus.Count);

            // 为每个道具添加技能
            for (int i = 0; i < ownedDaoJus.Count; i++)
            {
                DaoJuData daoJu = ownedDaoJus[i];
                int skillIndex = 10 + i; // 从第11个技能槽开始添加道具技能

                // 确保不超过技能栏容量
                if (skillIndex >= uiFightPanel.FightSkills.Count)
                {
                    break;
                }

                // 创建技能对象
                GUIPackage.Skill skill = new GUIPackage.Skill(daoJu.DaoJuSkill, 0, 10);

                // 添加技能到角色
                avatar.skill.Add(skill);

                // 设置技能到UI
                uiFightPanel.FightSkills[skillIndex].SetSkill(skill);

                // 设置热键
                KeyCode hotKey = NumberToHomeRowLetter(i + 1);
                uiFightPanel.FightSkills[skillIndex].HotKey = hotKey;

                // 更新热键显示
                UpdateHotKeyText(uiFightPanel.FightSkills[skillIndex].gameObject, hotKey);

                // 添加道具数量显示
                AddCountText(uiFightPanel.FightSkills[skillIndex].gameObject, avatar.getItemNum(daoJu.DaoJuItem));

                // 记录技能索引和道具ID的对应关系
                if (!skillIndexToItemIDMap.ContainsKey(skillIndex))
                {
                    skillIndexToItemIDMap.Add(skillIndex, daoJu.DaoJuItem);
                }
            }

            // 所有技能按钮创建完成后，应用自定义布局
            ApplySkillPositions(uiFightPanel);
        }

        /// <summary>
        /// 应用所有技能按钮的位置调整
        /// </summary>
        private static void ApplySkillPositions(UIFightPanel uiFightPanel)
        {
            if (uiFightPanel.FightSkills == null || uiFightPanel.FightSkills.Count < 10)
            {
                return;
            }

            Transform parentTransform = uiFightPanel.FightSkills[0].transform.parent;

            // 确保GridLayoutGroup启用，以便前10个技能按钮保持正确的网格布局
            var gridLayout = parentTransform.GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                gridLayout.enabled = true;

                // 强制布局更新，确保前10个按钮位置正确
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentTransform.GetComponent<RectTransform>());
            }

            // 获取第1列第一个按钮的位置，作为新增按钮的基准位置
            Vector3 basePosition = uiFightPanel.FightSkills[0].transform.localPosition;

            // 获取第二列第一个按钮（索引5）的位置，用于计算水平间距
            Vector3 secondColumnFirstPosition = uiFightPanel.FightSkills[5].transform.localPosition;

            // 计算两列之间的水平间距
            float horizontalSpacing = secondColumnFirstPosition.x - basePosition.x;

            // 处理新增的技能按钮（索引10及以上）
            // 新增的技能按钮要添加到第一列的上方
            for (int i = 10; i < uiFightPanel.FightSkills.Count; i++)
            {
                var skillItem = uiFightPanel.FightSkills[i];

                // 获取或添加LayoutElement组件并设置ignoreLayout
                var layoutElement = skillItem.GetComponent<LayoutElement>();
                if (layoutElement == null)
                {
                    layoutElement = skillItem.gameObject.AddComponent<LayoutElement>();
                }
                layoutElement.ignoreLayout = true;

                // 计算相对于第一列第一个按钮的位置偏移
                // 基于用户设置，每行垂直间隔为87f
                int skillIndex = i - 10; // 新增技能的相对索引（从0开始）
                int groupIndex = skillIndex / 5; // 5个一组，计算组索引
                int indexInGroup = skillIndex % 5; // 组内索引（0-4）

                // 计算垂直偏移：基础偏移2个87，每组再增加1个87
                float verticalOffset = 87f * (1 + groupIndex);

                // 计算水平偏移：每个按钮水平偏移83单位
                // 第1个按钮（indexInGroup = 0）：水平偏移 0
                // 第2个按钮（indexInGroup = 1）：水平偏移 83
                // 第3个按钮（indexInGroup = 2）：水平偏移 166
                // 第4个按钮（indexInGroup = 3）：水平偏移 249
                // 第5个按钮（indexInGroup = 4）：水平偏移 332
                float horizontalOffset = 83f * indexInGroup;

                // 设置新增按钮的位置，基于第1列第一个按钮的位置向上偏移
                skillItem.transform.localPosition = new Vector3(basePosition.x + horizontalOffset, basePosition.y + verticalOffset, basePosition.z);
            }
        }

        /// <summary>
        /// 确保技能栏有足够的槽位（在原有10个基础上扩展）
        /// </summary>
        private static void EnsureSkillSlots(UIFightPanel uiFightPanel, int itemCount)
        {
            if (uiFightPanel.FightSkills == null || uiFightPanel.FightSkills.Count < 10)
            {
                IsToolsMain.Error("技能栏数量不足，无法添加道具技能");
                return;
            }

            Transform parentTransform = uiFightPanel.FightSkills[0].transform.parent;

            // 确保GridLayoutGroup是启用的，以便前10个技能按钮正确布局
            var gridLayout = parentTransform.GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                // 临时启用GridLayoutGroup，确保前10个按钮布局正确
                bool wasEnabled = gridLayout.enabled;
                if (!wasEnabled)
                {
                    gridLayout.enabled = true;

                    // 强制布局更新
                    LayoutRebuilder.ForceRebuildLayoutImmediate(parentTransform.GetComponent<RectTransform>());
                }
            }

            // 获取第一列第一个按钮（索引0）的位置，作为新增按钮的基准位置
            Vector3 basePosition = uiFightPanel.FightSkills[0].transform.localPosition;

            // 获取第二列第一个按钮（索引5）的位置，用于计算水平间距
            Vector3 secondColumnFirstPosition = uiFightPanel.FightSkills[5].transform.localPosition;

            // 计算两列之间的水平间距
            float horizontalSpacing = secondColumnFirstPosition.x - basePosition.x;

            // 扩展技能栏，确保有足够的槽位（基础10个 + 道具技能数量）
            int neededSlots = 10 + itemCount;
            while (uiFightPanel.FightSkills.Count < neededSlots)
            {
                // 获取第1列第一个技能按钮作为模板
                GameObject template = uiFightPanel.FightSkills[0].gameObject;

                // 先在父物体外部克隆新技能按钮，避免受到GridLayoutGroup影响
                GameObject newBtn = UnityEngine.Object.Instantiate(template, null, false);

                // 获取或添加LayoutElement组件并设置ignoreLayout
                var layoutElement = newBtn.GetComponent<LayoutElement>();
                if (layoutElement == null)
                {
                    layoutElement = newBtn.AddComponent<LayoutElement>();
                }
                layoutElement.ignoreLayout = true;

                // 确保新增按钮的RectTransform参数正确
                var rectTransform = newBtn.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    // 设置锚点和中心点，确保位置计算正确
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                }

                // 设置新按钮位置，添加到第一列的上方
                // 计算相对于第一列第一个按钮的位置偏移
                int currentIndex = uiFightPanel.FightSkills.Count;
                int skillIndex = currentIndex - 10; // 新增技能的相对索引（从0开始）
                int groupIndex = skillIndex / 5; // 5个一组，计算组索引
                int indexInGroup = skillIndex % 5; // 组内索引（0-4）

                // 计算垂直偏移：基础偏移2个87，每组再增加1个87
                float verticalOffset = 87f * (1 + groupIndex);

                // 计算水平偏移：每个按钮水平偏移83单位
                // 第1个按钮（indexInGroup = 0）：水平偏移 0
                // 第2个按钮（indexInGroup = 1）：水平偏移 83
                // 第3个按钮（indexInGroup = 2）：水平偏移 166
                // 第4个按钮（indexInGroup = 3）：水平偏移 249
                // 第5个按钮（indexInGroup = 4）：水平偏移 332
                float horizontalOffset = 83f * indexInGroup;

                // 设置新位置，基于第1列第一个按钮的位置向上偏移
                newBtn.transform.localPosition = new Vector3(basePosition.x + horizontalOffset, basePosition.y + verticalOffset, basePosition.z);

                // 将新按钮添加到父物体
                newBtn.transform.SetParent(parentTransform, false);

                // 配置新技能按钮
                UIFightSkillItem newItem = newBtn.GetComponent<UIFightSkillItem>();
                newItem.Clear();

                // 设置热键（从A开始）
                KeyCode hotKey = NumberToHomeRowLetter(uiFightPanel.FightSkills.Count - 9);
                newItem.HotKey = hotKey;

                // 更新热键文本
                UpdateHotKeyText(newBtn, hotKey);

                // 将新按钮添加到技能栏列表
                uiFightPanel.FightSkills.Add(newItem);

                // 设置按钮名称
                newBtn.name = "FightSkillItem_" + hotKey.ToString();
            }
        }

        /// <summary>
        /// 将数字转换为对应的主键字母
        /// </summary>
        private static KeyCode NumberToHomeRowLetter(int number)
        {
            if (number >= 1 && number <= HomeRowKeys.Length - 1)
            {
                return HomeRowKeys[number];
            }
            return KeyCode.None;
        }

        /// <summary>
        /// 更新热键文本显示
        /// </summary>
        private static void UpdateHotKeyText(GameObject skillBtn, KeyCode hotKey)
        {
            var hotKeyText = skillBtn.transform.Find("Slot/LeftUpMask/HotKey")?.GetComponent<Text>();
            if (hotKeyText != null)
            {
                hotKeyText.text = hotKey != KeyCode.None ? hotKey.ToString() : "";
            }
        }

        /// <summary>
        /// 添加道具数量显示
        /// </summary>
        private static void AddCountText(GameObject skillBtn, int count)
        {
            var slotTransform = skillBtn.transform.Find("Slot");
            if (slotTransform == null) return;

            // 检查是否已有数量显示组件
            if (slotTransform.Find("CountText") != null) return;

            // 获取热键文本作为样式参考
            var hotKeyText = skillBtn.transform.Find("Slot/LeftUpMask/HotKey")?.GetComponent<Text>();
            if (hotKeyText == null) return;

            // 创建新的数量显示组件
            GameObject countTextObj = new GameObject("CountText");
            countTextObj.transform.SetParent(slotTransform, false);
            Text countText = countTextObj.AddComponent<Text>();

            // 复制hotKeyText的所有样式属性
            countText.font = hotKeyText.font;
            countText.fontSize = hotKeyText.fontSize;
            countText.color = new Color(0, 0, 0, 1);
            countText.alignment = hotKeyText.alignment;
            countText.fontStyle = hotKeyText.fontStyle;
            countText.lineSpacing = hotKeyText.lineSpacing;
            countText.horizontalOverflow = hotKeyText.horizontalOverflow;
            countText.verticalOverflow = hotKeyText.verticalOverflow;

            // 设置文本内容为道具数量
            countText.text = count.ToString();

            // 设置位置
            RectTransform countRect = countTextObj.GetComponent<RectTransform>();
            if (countRect != null)
            {
                countRect.sizeDelta = hotKeyText.GetComponent<RectTransform>().sizeDelta;
                countRect.localPosition = new Vector3(-45, -50, 0);
            }
        }

        /// <summary>
        /// 更新道具数量显示文本
        /// </summary>
        /// <param name="skillBtn">技能按钮对象</param>
        /// <param name="count">剩余数量</param>
        public static void UpdateItemCountText(GameObject skillBtn, int count)
        {
            var slotTransform = skillBtn.transform.Find("Slot");
            if (slotTransform == null) return;

            // 查找数量显示组件
            var countTextObj = slotTransform.Find("CountText");
            if (countTextObj == null) return;

            // 获取Text组件并更新显示
            Text countText = countTextObj.GetComponent<Text>();
            if (countText != null)
            {
                countText.text = count.ToString();
            }
        }

        /// <summary>
        /// 刷新所有道具的数量显示
        /// </summary>
        /// <param name="avatar">角色对象</param>
        /// <param name="daoJuList">道具列表</param>
        public static void RefreshAllItemCountText(KBEngine.Avatar avatar, List<DaoJuData> daoJuList)
        {
            // 检查参数有效性
            if (avatar == null || daoJuList == null)
                return;

            // 获取战斗界面面板实例
            var uiFightPanel = UIFightPanel.Inst;
            if (uiFightPanel == null || uiFightPanel.FightSkills == null)
                return;

            // 遍历所有道具
            for (int i = 0; i < daoJuList.Count; i++)
            {
                var daoJu = daoJuList[i];

                // 确保技能槽位已创建
                EnsureSkillSlots(uiFightPanel, i + 1);

                // 检查技能索引是否有效（道具技能从索引10开始）
                int skillIndex = 10 + i;
                if (skillIndex >= 0 && skillIndex < uiFightPanel.FightSkills.Count)
                {
                    // 获取当前道具数量并更新显示
                    int itemCount = avatar.getItemNum(daoJu.DaoJuItem);
                    UpdateItemCountText(uiFightPanel.FightSkills[skillIndex].gameObject, itemCount);
                }
            }
        }

        /// <summary>
        /// 刷新所有道具的数量显示（简化版，自动获取所需数据）
        /// </summary>
        public static void RefreshAllItemCountText()
        {
            try
            {
                // 获取当前玩家角色
                var player = PlayerEx.Player;
                if (player == null) return;

                // 获取战斗界面面板实例
                var uiFightPanel = UIFightPanel.Inst;
                if (uiFightPanel == null || uiFightPanel.FightSkills == null)
                    return;

                // 遍历已记录的技能索引和道具ID对应关系，更新数量显示
                foreach (var kvp in skillIndexToItemIDMap)
                {
                    int skillIndex = kvp.Key;
                    int itemID = kvp.Value;

                    // 检查技能索引是否有效
                    if (skillIndex >= 0 && skillIndex < uiFightPanel.FightSkills.Count)
                    {
                        // 获取当前道具数量并更新显示
                        int itemCount = player.getItemNum(itemID);
                        UpdateItemCountText(uiFightPanel.FightSkills[skillIndex].gameObject, itemCount);
                    }
                }
            }
            catch (Exception ex)
            {
                IsToolsMain.Error($"刷新道具数量显示时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 消耗指定数量的道具并更新UI显示
        /// </summary>
        /// <param name="avatar">角色对象</param>
        /// <param name="itemID">道具ID</param>
        /// <param name="count">消耗数量</param>
        public static void ConsumeItemAndUpdateUI(KBEngine.Avatar avatar, int itemID, int count = 1)
        {
            try
            {
                // 检查参数有效性
                if (avatar == null || count <= 0)
                    return;

                // 检查是否有足够的道具
                int currentCount = avatar.getItemNum(itemID);
                if (currentCount < count)
                    return;

                // TODO: 调用游戏本体的道具消耗方法（暂用占位符）
                // deitem(itemID, count); // 这是游戏本体的方法，暂时无法访问

                // 消耗道具后更新所有道具的数量显示
                RefreshAllItemCountText();
            }
            catch (Exception ex)
            {
                IsToolsMain.Error($"消耗道具时出错: {ex.Message}");
            }
        }
    }
}