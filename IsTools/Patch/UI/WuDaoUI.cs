using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JSONClass;
using System.Collections.Generic;
using Tab;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.IsTools.Patch.UI
{
    /// <summary>
    /// 悟道等级面板 UpdateUI 方法的 Harmony 补丁
    /// 实现悟道技能的分页显示功能，每页最多显示4个技能
    /// </summary>
    [HarmonyPatch(typeof(Tab.TabWuDaoLevelBase), "UpdateUI")]
    class Patch_WuDao_UpdateUI
    {
        /// <summary>
        /// 保存每个等级槽位的当前页码
        /// Key: id * 10 + level (唯一标识每个槽位)
        /// Value: 当前页码（从0开始）
        /// </summary>
        private static Dictionary<int, int> currentPageDict = new Dictionary<int, int>();

        /// <summary>
        /// 保存每个等级槽位的所有技能ID列表
        /// Key: id * 10 + level
        /// Value: 该槽位的所有技能ID列表
        /// </summary>
        private static Dictionary<int, List<int>> allSkillIdsDict = new Dictionary<int, List<int>>();

        /// <summary>
        /// UpdateUI 方法的前缀补丁
        /// 在原方法执行前拦截，实现分页显示逻辑
        /// </summary>
        /// <param name="__instance">被补丁的 TabWuDaoLevelBase 实例</param>
        /// <param name="id">悟道类型ID</param>
        /// <returns>false 表示跳过原方法，true 表示继续执行原方法</returns>
        static bool Prefix(Tab.TabWuDaoLevelBase __instance, int id)
        {
            try
            {
                // 获取当前槽位的等级
                int level = (int)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_level").GetValue(__instance);
                // 生成唯一键值，用于区分不同的等级槽位
                int key = id * 10 + level;

                // 如果该槽位还没有页码记录，初始化为第0页
                if (!currentPageDict.ContainsKey(key))
                {
                    currentPageDict[key] = 0;
                }

                // 获取该槽位的所有技能ID
                List<int> allSkillIds = GetAllWuDaoSkillIds(id, level);
                allSkillIdsDict[key] = allSkillIds;

                // 获取当前页码
                int currentPage = currentPageDict[key];
                // 计算总技能数
                int totalSkills = allSkillIds.Count;
                // 每页显示4个技能
                int pageSize = 4;
                // 计算总页数
                int totalPages = (int)Math.Ceiling((double)totalSkills / pageSize);

                // 如果当前页码超出范围，修正到最后一页
                if (currentPage >= totalPages)
                {
                    currentPageDict[key] = Math.Max(0, totalPages - 1);
                    currentPage = currentPageDict[key];
                }

                // 计算当前页要显示的技能索引范围
                int startIndex = currentPage * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalSkills);
                // 当前页实际显示的技能数量
                int displayCount = endIndex - startIndex;

                // 设置实例的悟道类型ID
                AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_id").SetValue(__instance, id);
                // 判断该等级是否已激活
                bool canActive = (bool)AccessTools.Method(typeof(Tab.TabWuDaoLevelBase), "IsCanActive").Invoke(__instance, null);

                // 获取激活和未激活状态的UI对象
                GameObject _active = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_active").GetValue(__instance);
                GameObject _noActive = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_noActive").GetValue(__instance);
                GameObject _go = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_go").GetValue(__instance);

                // 根据是否激活切换UI显示
                _active.SetActive(canActive);
                _noActive.SetActive(!canActive);

                // 清理之前动态生成的技能列表
                if (_go.transform.childCount > 2)
                {
                    UnityEngine.Object.Destroy(_go.transform.GetChild(2).gameObject);
                }

                // 根据当前页显示的技能数量更换背景云图
                Image _cloud = (Image)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_cloud").GetValue(__instance);
                _cloud.sprite = SingletonMono<TabUIMag>.Instance.WuDaoPanel.WudaoBgImgDict[displayCount.ToString()];

                // 实例化对应的技能列表预制体
                GameObject skillListGo = SingletonMono<TabUIMag>.Instance.WuDaoPanel.WudaoSkillListDict[displayCount].Inst(_go.transform);

                // 设置技能列表的位置
                float y = SingletonMono<TabUIMag>.Instance.WuDaoPanel.WudaoSkillListDict[displayCount].transform.position.y;
                skillListGo.transform.SetPostionY(y).SetLocalPositionX(0f);
                skillListGo.name = displayCount.ToString();

                // 遍历当前页要显示的技能，创建对应的技能槽位
                for (int i = 0; i < displayCount; i++)
                {
                    // 获取技能ID和技能数据
                    int skillId = allSkillIds[startIndex + i];
                    WuDaoJson skillData = WuDaoJson.DataDict[skillId];
                    // 创建技能槽位
                    WuDaoSlot slot = new WuDaoSlot(skillListGo.transform.GetChild(i).gameObject, skillData.id);

                    if (canActive)
                    {
                        // 已激活等级：判断技能是否已学习
                        if (Tools.instance.getPlayer().wuDaoMag.IsStudy(skillData.id))
                        {
                            slot.SetState(1); // 已学习
                        }
                        else
                        {
                            slot.SetState(2); // 未学习（但可学）
                        }
                    }
                    else
                    {
                        slot.SetState(3); // 未激活等级：技能不可用
                    }
                }

                // 添加翻页按钮
                AddPageButtons(__instance, key, totalPages, currentPage);

                // 返回false，跳过原方法
                return false;
            }
            catch (Exception ex)
            {
                // 发生错误时记录日志，并返回true让原方法继续执行
                IsToolsMain.Error($"Patch_WuDao_UpdateUI 错误: {ex.Message}");
                return true;
            }
        }

        /// <summary>
        /// 获取指定悟道类型和等级的所有技能ID列表
        /// </summary>
        /// <param name="id">悟道类型ID</param>
        /// <param name="level">等级</param>
        /// <returns>符合条件的技能ID列表</returns>
        private static List<int> GetAllWuDaoSkillIds(int id, int level)
        {
            List<int> result = new List<int>();
            // 遍历所有悟道技能
            foreach (WuDaoJson skill in WuDaoJson.DataList)
            {
                // 判断技能类型包含当前悟道ID，且技能等级等于当前槽位等级
                if (skill.Type.Contains(id) && skill.Lv == level)
                {
                    result.Add(skill.id);
                }
            }
            return result;
        }

        /// <summary>
        /// 添加翻页按钮到UI
        /// </summary>
        /// <param name="__instance">TabWuDaoLevelBase 实例</param>
        /// <param name="key">槽位唯一键值</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="currentPage">当前页码</param>
        private static void AddPageButtons(Tab.TabWuDaoLevelBase __instance, int key, int totalPages, int currentPage)
        {
            // 如果只有一页或没有页，不需要翻页按钮
            if (totalPages <= 1)
            {
                return;
            }

            // 获取当前槽位的GameObject
            GameObject _go = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_go").GetValue(__instance);

            // 如果已经存在翻页按钮，先删除
            Transform existingButtons = _go.transform.Find("PageButtons");
            if (existingButtons != null)
            {
                UnityEngine.Object.Destroy(existingButtons.gameObject);
            }

            // 创建翻页按钮容器
            GameObject buttonsContainer = new GameObject("PageButtons");
            buttonsContainer.transform.SetParent(_go.transform, false);
            buttonsContainer.transform.SetAsLastSibling();

            // 创建上一页和下一页按钮
            GameObject prevButton = CreatePageButton("上一页", buttonsContainer.transform, new Vector3(-100f, -50f, 0f));
            GameObject nextButton = CreatePageButton("下一页", buttonsContainer.transform, new Vector3(100f, -50f, 0f));

            // 获取按钮组件
            Button prevBtn = prevButton.GetComponent<Button>();
            Button nextBtn = nextButton.GetComponent<Button>();

            // 根据当前页码设置按钮的可交互状态
            prevBtn.interactable = currentPage > 0;
            nextBtn.interactable = currentPage < totalPages - 1;

            // 上一页按钮点击事件
            prevBtn.onClick.AddListener(() =>
            {
                if (currentPageDict[key] > 0)
                {
                    // 页码减1
                    currentPageDict[key]--;
                    // 重新调用UpdateUI刷新UI
                    int id = (int)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_id").GetValue(__instance);
                    __instance.UpdateUI(id);
                }
            });

            // 下一页按钮点击事件
            nextBtn.onClick.AddListener(() =>
            {
                if (currentPageDict[key] < totalPages - 1)
                {
                    // 页码加1
                    currentPageDict[key]++;
                    // 重新调用UpdateUI刷新UI
                    int id = (int)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_id").GetValue(__instance);
                    __instance.UpdateUI(id);
                }
            });
        }

        /// <summary>
        /// 创建翻页按钮GameObject
        /// </summary>
        /// <param name="text">按钮显示的文本</param>
        /// <param name="parent">父级Transform</param>
        /// <param name="localPosition">本地位置</param>
        /// <returns>创建的按钮GameObject</returns>
        private static GameObject CreatePageButton(string text, Transform parent, Vector3 localPosition)
        {
            // 创建按钮GameObject
            GameObject button = new GameObject(text);
            button.transform.SetParent(parent, false);
            button.transform.localPosition = localPosition;

            // 添加RectTransform组件并设置大小
            RectTransform rect = button.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80f, 30f);

            // 添加Image组件作为按钮背景
            Image image = button.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // 添加Button组件
            Button btn = button.AddComponent<Button>();

            // 创建文本GameObject
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(button.transform, false);

            // 添加文本的RectTransform
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(80f, 30f);

            // 添加Text组件并设置文本内容
            Text textComp = textObj.AddComponent<Text>();
            textComp.text = text;
            textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComp.fontSize = 14;
            textComp.alignment = TextAnchor.MiddleCenter;
            textComp.color = Color.white;

            return button;
        }
    }

    #region 滚动条法
    //[HarmonyPatch(typeof(Tab.TabWuDaoPanel), "Init")]
    //class Patch_TabWuDaoPanel_Init
    //{
    //    static void Postfix(Tab.TabWuDaoPanel __instance)
    //    {
    //        // --- 原有的循环代码 ---
    //        for (int i = 5; i <= 10; i++)
    //        {
    //            init_Muban(__instance, i);

    //        }

    //        // --- 原有的背景图字典循环代码 ---
    //        for (int i = 5; i <= 10; i++)
    //        {
    //            string key = i.ToString();
    //            if (!__instance.WudaoBgImgDict.ContainsKey(key))
    //            {
    //                if (__instance.WudaoBgImgDict.TryGetValue("1", out Sprite defaultSprite))
    //                {
    //                    __instance.WudaoBgImgDict.Add(key, defaultSprite);
    //                }
    //            }
    //        }
    //    }

    //    public static void init_Muban(Tab.TabWuDaoPanel __instance, int slot_number)
    //    {
    //        if (!__instance.WudaoSkillListDict.ContainsKey(slot_number))
    //        {
    //            // 尝试获取1个技能的预制体作为基础
    //            if (__instance.WudaoSkillListDict.TryGetValue(1, out GameObject basePrefab))
    //            {
    //                // 创建新的预制体实例
    //                GameObject newPrefab = Object.Instantiate(basePrefab);
    //                newPrefab.name = slot_number.ToString();


    //                // 计算间距 - 使用与原始布局相同的间距
    //                float spacing = 181f; // 原始垂直差距为181单位


    //                // 创建技能槽位
    //                for (int j = 2; j <= slot_number; j++)
    //                {
    //                    // 复制Slot1作为模板
    //                    Transform slot1 = basePrefab.transform.Find("Slot1");
    //                    if (slot1 != null)
    //                    {
    //                        GameObject newSkillSlot = Object.Instantiate(slot1.gameObject);
    //                        newSkillSlot.transform.SetParent(newPrefab.transform);
    //                        newSkillSlot.transform.localPosition = new Vector3(0, -(j - 1) * spacing, 0);
    //                        newSkillSlot.name = "Slot" + j;
    //                    }
    //                }

    //                // 将新预制体添加到字典中
    //                __instance.WudaoSkillListDict.Add(slot_number, newPrefab);

    //                // 实例化
    //                GameObject parentWuDaoSkill = GameObject.Find("NewUICanvas(Clone)/TabPanel(Clone)/TabSelect/Panel/悟道/WuDaoSkill");
    //                if (parentWuDaoSkill != null)
    //                {
    //                    // 实例化 newPrefab 并设置父对象和初始状态
    //                    GameObject instantiatedPrefab = Object.Instantiate(newPrefab, parentWuDaoSkill.transform);
    //                    instantiatedPrefab.name = $"{slot_number}";
    //                    instantiatedPrefab.SetActive(true);

    //                    // 可选：重置本地变换，确保位置、旋转、缩放符合预期
    //                    RectTransform rectTransform = instantiatedPrefab.GetComponent<RectTransform>();
    //                    if (rectTransform != null)
    //                    {
    //                        rectTransform.anchoredPosition3D = Vector3.zero; // 重置位置
    //                        rectTransform.localRotation = Quaternion.identity; // 重置旋转
    //                        rectTransform.localScale = Vector3.one; // 重置缩放
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    #endregion
}