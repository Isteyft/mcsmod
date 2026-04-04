using MaiJiu.MCS.Pet.PetManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using YSGame.TuJian;
using MaiJiu.MCS.Pet.TuJian;
using top.Isteyft.MCS.IsTools.Data;
using MaiJiu.MCS.HH.Tool;

namespace top.Isteyft.MCS.IsTools.Patch.TujianPatch
{
    /// <summary>
    /// 成就/图鉴信息面板
    /// 继承自 InfoPanelBase1，用于处理特定类型（神通/秘术）的图鉴显示逻辑
    /// </summary>
    public class AchievementInfoPanel : InfoPanelBase1
    {
        /// <summary>
        /// 刷新数据列表
        /// 此方法负责根据当前的筛选条件（品质、属性、搜索关键词）过滤神通/秘术列表
        /// </summary>
        public override void RefreshDataList()
        {
            base.RefreshDataList();

            // 强制设置图鉴标签页为索引为 11
            TuJianItemTab.Inst.SetDropdown(11, 0);

            // 检查图鉴管理器是否需要刷新列表
            if (TuJianManager.Inst.NeedRefreshDataList)
            {
                if (TuJianManager.Inst.Searcher.SearchCount == 0)
                {
                    TuJianItemTab.Inst.FilterSSV.DataList = TuJianDB.ItemTuJianFilterData[11];
                }
                else
                {
                    DataList.Clear(); // 清空当前临时列表
                    // 遍历所有原始数据
                    foreach (Dictionary<int, string> item in TuJianDB.ItemTuJianFilterData[11])
                    {
                        int key = item.First().Key;   // 获取物品ID
                        string value = item.First().Value; // 获取物品名称
                        bool flag = true; // 标记该物品是否应该被保留

                        if (TuJianManager.Inst.Searcher.SearchCount > 0 && !TuJianManager.Inst.Searcher.IsContansSearch(value))
                        {
                            flag = false;
                        }

                        // 如果通过所有筛选，添加到列表
                        if (flag)
                        {
                            DataList.Add(new Dictionary<int, string> { { key, value } });
                        }
                    }

                    // 将过滤后的列表应用到 UI 滚动视图
                    TuJianItemTab.Inst.FilterSSV.DataList = DataList;
                }

                // --- UI 状态更新 ---
                // 如果列表为空，隐藏图标和品质背景
                if (TuJianItemTab.Inst.FilterSSV.DataList.Count == 0)
                {
                    _ItemIconImage.color = new Color(0f, 0f, 0f, 0f); // 透明
                    _QualityImage.color = new Color(0f, 0f, 0f, 0f);
                    _QualityUpImage.color = new Color(0f, 0f, 0f, 0f);
                }
                else
                {
                    // 列表不为空，显示图标
                    _ItemIconImage.color = Color.white;
                    _QualityImage.color = Color.white;
                    _QualityUpImage.color = Color.white;
                }

                // 重置刷新标志位
                TuJianManager.Inst.NeedRefreshDataList = false;
            }

            // --- 处理超链接跳转 ---
            if (isOnHyperlink)
            {
                TuJianItemTab.Inst.FilterSSV.NowSelectID = hylinkArgs[2]; // 设置选中的ID
                TuJianItemTab.Inst.FilterSSV.NeedResetToTop = false;     // 不需要重置到顶部
                isOnHyperlink = false; // 重置标志位
            }
        }

        /// <summary>
        /// 刷新面板数据（显示选中物品的详细信息）
        /// </summary>
        public override void RefreshPanelData()
        {
            base.RefreshPanelData();
            RefreshDataList();
            int nowSelectID = TuJianItemTab.Inst.FilterSSV.NowSelectID;
            if (nowSelectID < 1)
            {
                _HyText.text = "";
                return;
            }
            TuJianManager.Inst.NowPageHyperlink = $"1_11_{nowSelectID}";
            AchievementData nowSelectData = AchievementData.GetAchievementId(nowSelectID);
            StringBuilder stringBuilder = new StringBuilder();
            if (AchievementData.IsSuccess(nowSelectID))
            {
                stringBuilder.Append($"#c449491成就名称：#n{nowSelectData.AchievementTitle}");
                stringBuilder.Append("\n\n");
                stringBuilder.Append("#c449491成就描述：#n#s24");
                stringBuilder.Append($"{nowSelectData.AchievementInfo}");
            }
            else
            {
                stringBuilder.Append($"#c449491成就名称：#n{nowSelectData.AchievementTitle}(未完成)");
                stringBuilder.Append("\n\n");
                stringBuilder.Append("#c449491成就描述：#n#s24");
                stringBuilder.Append($"未知");
            }
            stringBuilder.Append("\n\n#n#s34");
            stringBuilder.Append("#c449491成就介绍：#n#s24");
            stringBuilder.Append($"{nowSelectData.AchievementDesc}");
            _HyText.text = stringBuilder.ToString();
            SetAchievementIcon(nowSelectID);
        }

        public void SetAchievementIcon(int id)
        {
            Sprite sprite = ResManager.inst.LoadSprite($"Achievement Icon/{id}");
            _ItemIconImage.sprite = sprite;
            _QualityImage.sprite = sprite;
            _QualityUpImage.sprite = sprite;
        }
    }
}