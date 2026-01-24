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
    [HarmonyPatch(typeof(Tab.TabWuDaoLevelBase), "UpdateUI")]
    class Patch_WuDao_UpdateUI
    {
        private static Dictionary<int, int> currentPageDict = new Dictionary<int, int>();
        private static Dictionary<int, List<int>> allSkillIdsDict = new Dictionary<int, List<int>>();

        static bool Prefix(Tab.TabWuDaoLevelBase __instance, int id)
        {
            try
            {
                int level = (int)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_level").GetValue(__instance);
                int key = id * 10 + level;

                if (!currentPageDict.ContainsKey(key))
                {
                    currentPageDict[key] = 0;
                }

                List<int> allSkillIds = GetAllWuDaoSkillIds(id, level);
                allSkillIdsDict[key] = allSkillIds;

                int currentPage = currentPageDict[key];
                int totalSkills = allSkillIds.Count;
                int pageSize = 4;
                int totalPages = (int)Math.Ceiling((double)totalSkills / pageSize);

                if (currentPage >= totalPages)
                {
                    currentPageDict[key] = Math.Max(0, totalPages - 1);
                    currentPage = currentPageDict[key];
                }

                int startIndex = currentPage * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalSkills);
                int displayCount = endIndex - startIndex;

                AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_id").SetValue(__instance, id);
                bool canActive = (bool)AccessTools.Method(typeof(Tab.TabWuDaoLevelBase), "IsCanActive").Invoke(__instance, null);

                GameObject _active = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_active").GetValue(__instance);
                GameObject _noActive = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_noActive").GetValue(__instance);
                GameObject _go = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_go").GetValue(__instance);

                _active.SetActive(canActive);
                _noActive.SetActive(!canActive);

                if (_go.transform.childCount > 2)
                {
                    UnityEngine.Object.Destroy(_go.transform.GetChild(2).gameObject);
                }

                Image _cloud = (Image)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_cloud").GetValue(__instance);
                _cloud.sprite = SingletonMono<TabUIMag>.Instance.WuDaoPanel.WudaoBgImgDict[displayCount.ToString()];

                GameObject skillListGo = SingletonMono<TabUIMag>.Instance.WuDaoPanel.WudaoSkillListDict[displayCount].Inst(_go.transform);

                float y = SingletonMono<TabUIMag>.Instance.WuDaoPanel.WudaoSkillListDict[displayCount].transform.position.y;
                skillListGo.transform.SetPostionY(y).SetLocalPositionX(0f);
                skillListGo.name = displayCount.ToString();

                for (int i = 0; i < displayCount; i++)
                {
                    int skillId = allSkillIds[startIndex + i];
                    WuDaoJson skillData = WuDaoJson.DataDict[skillId];
                    WuDaoSlot slot = new WuDaoSlot(skillListGo.transform.GetChild(i).gameObject, skillData.id);

                    if (canActive)
                    {
                        if (Tools.instance.getPlayer().wuDaoMag.IsStudy(skillData.id))
                        {
                            slot.SetState(1);
                        }
                        else
                        {
                            slot.SetState(2);
                        }
                    }
                    else
                    {
                        slot.SetState(3);
                    }
                }

                AddPageButtons(__instance, key, totalPages, currentPage);

                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Patch_WuDao_UpdateUI 错误: {ex.Message}");
                return true;
            }
        }

        private static List<int> GetAllWuDaoSkillIds(int id, int level)
        {
            List<int> result = new List<int>();
            foreach (WuDaoJson skill in WuDaoJson.DataList)
            {
                if (skill.Type.Contains(id) && skill.Lv == level)
                {
                    result.Add(skill.id);
                }
            }
            return result;
        }

        private static void AddPageButtons(Tab.TabWuDaoLevelBase __instance, int key, int totalPages, int currentPage)
        {
            if (totalPages <= 1)
            {
                return;
            }

            GameObject _go = (GameObject)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_go").GetValue(__instance);

            Transform existingButtons = _go.transform.Find("PageButtons");
            if (existingButtons != null)
            {
                UnityEngine.Object.Destroy(existingButtons.gameObject);
            }

            GameObject buttonsContainer = new GameObject("PageButtons");
            buttonsContainer.transform.SetParent(_go.transform, false);
            buttonsContainer.transform.SetAsLastSibling();

            GameObject prevButton = CreatePageButton("上一页", buttonsContainer.transform, new Vector3(-100f, -50f, 0f));
            GameObject nextButton = CreatePageButton("下一页", buttonsContainer.transform, new Vector3(100f, -50f, 0f));

            Button prevBtn = prevButton.GetComponent<Button>();
            Button nextBtn = nextButton.GetComponent<Button>();

            prevBtn.interactable = currentPage > 0;
            nextBtn.interactable = currentPage < totalPages - 1;

            prevBtn.onClick.AddListener(() =>
            {
                if (currentPageDict[key] > 0)
                {
                    currentPageDict[key]--;
                    int id = (int)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_id").GetValue(__instance);
                    __instance.UpdateUI(id);
                }
            });

            nextBtn.onClick.AddListener(() =>
            {
                if (currentPageDict[key] < totalPages - 1)
                {
                    currentPageDict[key]++;
                    int id = (int)AccessTools.Field(typeof(Tab.TabWuDaoLevelBase), "_id").GetValue(__instance);
                    __instance.UpdateUI(id);
                }
            });
        }

        private static GameObject CreatePageButton(string text, Transform parent, Vector3 localPosition)
        {
            GameObject button = new GameObject(text);
            button.transform.SetParent(parent, false);
            button.transform.localPosition = localPosition;

            RectTransform rect = button.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80f, 30f);

            Image image = button.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            Button btn = button.AddComponent<Button>();

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(button.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(80f, 30f);

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