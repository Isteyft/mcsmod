using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.JiuZhou.Patch
{
    [HarmonyPatch(typeof(MainUISelectTianFu))]
    public class TianfuPatch
    {
        public const int BIRTH_MAP_PAGE = 9;
        public static List<MainUITianFuCell> BirthMapCells = new List<MainUITianFuCell>();

        // === 创建私有字段的访问器 ===
        private static readonly AccessTools.FieldRef<MainUISelectTianFu, GameObject> finallyPageRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, GameObject>("finallyPage");

        private static readonly AccessTools.FieldRef<MainUISelectTianFu, GameObject> nextBtnRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, GameObject>("nextBtn");

        private static readonly AccessTools.FieldRef<MainUISelectTianFu, Text> titleRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, Text>("title");

        private static readonly AccessTools.FieldRef<MainUISelectTianFu, Text> descRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, Text>("desc");

        private static readonly AccessTools.FieldRef<MainUISelectTianFu, GameObject> shenYuNumRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, GameObject>("shenYuNum");

        private static readonly AccessTools.FieldRef<MainUISelectTianFu, MainUISetLinGen> setLingGenRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, MainUISetLinGen>("setLingGen");

        private static readonly AccessTools.FieldRef<MainUISelectTianFu, Text> finallyDescRef =
            AccessTools.FieldRefAccess<MainUISelectTianFu, Text>("finallyDesc");

        // ===== Init 补丁 =====
        [HarmonyPostfix]
        [HarmonyPatch("Init")]
        public static void Postfix_Init(MainUISelectTianFu __instance)
        {
            if (!__instance.tianFuPageList.ContainsKey(BIRTH_MAP_PAGE))
            {
                __instance.tianFuPageList[BIRTH_MAP_PAGE] = new List<MainUITianFuCell>();
            }

            foreach (var cell in BirthMapCells)
            {
                cell.page = BIRTH_MAP_PAGE;
                __instance.tianFuPageList[BIRTH_MAP_PAGE].Add(cell);
                cell.gameObject.SetActive(false);
            }
        }

        // ===== NextPage 补丁 =====
        [HarmonyPrefix]
        [HarmonyPatch("NextPage")]
        public static bool Prefix_NextPage(MainUISelectTianFu __instance)
        {
            var finallyPage = finallyPageRef(__instance);
            var nextBtn = nextBtnRef(__instance);

            if (__instance.curPage == 8)
            {
                if (!__instance.CheckCurHasSelect())
                {
                    UIPopTip.Inst.Pop("至少选择一个天赋", PopTipIconType.叹号);
                    return false;
                }

                __instance.HideCurPage();
                __instance.curPage = BIRTH_MAP_PAGE;
                __instance.ShowCurPageList();
                return false;
            }

            if (__instance.curPage == BIRTH_MAP_PAGE)
            {
                if (!__instance.CheckCurHasSelect())
                {
                    UIPopTip.Inst.Pop("请选择出生地图", PopTipIconType.叹号);
                    return false;
                }

                if (__instance.tianfuDian < 0)
                {
                    UIPopTip.Inst.Pop("天赋点不能为负数", PopTipIconType.叹号);
                    return false;
                }

                __instance.HideCurPage();
                __instance.curPage++; // now 10
                ShowFinallyPage(__instance); // 自定义调用
                finallyPage.SetActive(true);
                nextBtn.SetActive(false);
                return false;
            }

            return true;
        }

        // ===== LastPage 补丁 =====
        [HarmonyPrefix]
        [HarmonyPatch("LastPage")]
        public static bool Prefix_LastPage(MainUISelectTianFu __instance)
        {
            var finallyPage = finallyPageRef(__instance);
            var nextBtn = nextBtnRef(__instance);
            var shenYuNum = shenYuNumRef(__instance);
            var setLingGen = setLingGenRef(__instance);
            var finallyDesc = finallyDescRef(__instance);
            var title = titleRef(__instance);
            var desc = descRef(__instance);

            // 从最终页返回（当前页面是10）
            if (__instance.curPage == 10)
            {
                // 隐藏最终页面并重置所有UI元素
                finallyPage.SetActive(false);
                nextBtn.SetActive(true);
                shenYuNum.SetActive(true);
                
                // 清除最终页的文本
                if (finallyDesc != null)
                {
                    finallyDesc.text = "";
                }
                
                // 重置标题和描述
                title.text = "出生之地";
                desc.text = "选择你的出生地图，这将影响初始资源与遭遇。";
                
                // 追加已选天赋的描述
                foreach (var key in __instance.hasSelectList.Keys)
                {
                    if (__instance.hasSelectList[key].page == BIRTH_MAP_PAGE)
                    {
                        desc.text += "\n" + jsonData.instance.CreateAvatarJsonData[__instance.hasSelectList[key].id.ToString()]["Info"].Str;
                    }
                }
                
                // 隐藏当前页面并显示出生地图页面
                __instance.HideCurPage();
                __instance.curPage = BIRTH_MAP_PAGE;
                __instance.ShowCurPageList();
                
                return false; // 阻止原始方法执行
            }
            
            // 如果当前页面是出生地图页面，执行原始逻辑
            if (__instance.curPage == BIRTH_MAP_PAGE)
            {
                return true; // 执行原始方法
            }
            
            // 其他页面执行原始逻辑
            return true;
        }

        // ===== UpdateDesc 补丁 =====
        [HarmonyPrefix]
        [HarmonyPatch("UpdateDesc")]
        public static bool Prefix_UpdateDesc(MainUISelectTianFu __instance)
        {
            // 获取私有字段
            var title = titleRef(__instance);
            var desc = descRef(__instance);

            // 如果是第9页（出生地图），我们完全接管逻辑，不走原方法
            if (__instance.curPage == BIRTH_MAP_PAGE)
            {
                title.text = "出生之地";
                desc.text = "选择你的出生地图，这将影响机缘，以及后续发展。";

                // 可选：追加已选天赋的描述
                foreach (var key in __instance.hasSelectList.Keys)
                {
                    if (__instance.hasSelectList[key].page == BIRTH_MAP_PAGE)
                    {
                        desc.text += "\n" + jsonData.instance.CreateAvatarJsonData[__instance.hasSelectList[key].id.ToString()]["Info"].Str;
                    }
                }

                return false; // ⚠️ 阻止原始 UpdateDesc 执行！
            }

            // 其他页面走原逻辑
            return true;
        }

        // ===== 修复天赋取消再次点击时的重复键错误 =====
        [HarmonyPatch("Init")]
        [HarmonyPatch(typeof(MainUISelectTianFu))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void Postfix_Init_FixDuplicateKey(MainUISelectTianFu __instance)
        {
            // 为所有出生地图天赋的toggle添加键存在检查
            if (__instance.tianFuPageList.ContainsKey(BIRTH_MAP_PAGE))
            {
                foreach (var cell in __instance.tianFuPageList[BIRTH_MAP_PAGE])
                {
                    // 移除现有的监听器
                    cell.toggle.valueChange.RemoveAllListeners();
                    // 添加修复后的监听器
                cell.toggle.valueChange.AddListener(delegate()
                {
                    if (cell.toggle.isOn)
                    {
                        // 检查是否已存在相同的键
                        if (!__instance.hasSelectList.ContainsKey(cell.id))
                        {
                            __instance.hasSelectList[cell.id] = cell;
                            __instance.AddTianFuDian(-cell.costNum);
                        }
                    }
                    else
                    {
                        // 移除已选择的天赋
                        if (__instance.hasSelectList.ContainsKey(cell.id))
                        {
                            __instance.AddTianFuDian(cell.costNum);
                            __instance.hasSelectList.Remove(cell.id);
                        }
                    }
                    // 更新描述文本，确保第9页的文字在天赋更改时更新
                    __instance.UpdateDesc();
                });
                }
            }
        }

        // ===== 模拟原 ShowFinallyPage 逻辑（避免调用私有方法）=====
        private static void ShowFinallyPage(MainUISelectTianFu __instance)
        {
            var title = titleRef(__instance);
            var desc = descRef(__instance);
            var finallyPage = finallyPageRef(__instance);
            var shenYuNum = shenYuNumRef(__instance);
            var finallyDesc = finallyDescRef(__instance);
            var nextBtn = nextBtnRef(__instance);

            title.text = "经历";
            desc.text = "";
            shenYuNum.SetActive(false);
            nextBtn.SetActive(false);
            finallyDesc.text = "\n";

            // 复制原始逻辑：排序并显示已选天赋描述
            List<int> list = new List<int>();
            foreach (int item in __instance.hasSelectList.Keys)
            {
                list.Add(item);
            }
            list.Sort((int x, int y) => x.CompareTo(y));
            foreach (int key in list)
            {
                if (__instance.hasSelectList[key].page != 1)
                {
                    Text text = finallyDesc;
                    text.text = text.text + jsonData.instance.CreateAvatarJsonData[__instance.hasSelectList[key].id.ToString()]["Info"].Str + "\n\n";
                }
            }
            
            // 添加最终的故事文本
            Text text2 = finallyDesc;
            text2.text += "十六岁那年，在指引下，长生之途的大门缓缓为你敞开——\n";

            finallyPage.SetActive(true);
        }

        // ===== 外部注册接口 =====
        public static void RegisterBirthMapCell(MainUITianFuCell cell)
        {
            if (cell != null)
            {
                BirthMapCells.Add(cell);
            }
        }
    }
}