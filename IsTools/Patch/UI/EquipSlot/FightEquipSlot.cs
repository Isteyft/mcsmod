using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YSGame.Fight;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Util;
using GUIPackage;

namespace top.Isteyft.MCS.IsTools.Patch.UI.EquipSlot
{
    [HarmonyPatch(typeof(KBEngine.Avatar))]
    public class FightEquipSlot
    {
        // 游戏原始技能栏数量（前 10 个为基础槽位）
        private const int BaseSkillSlotCount = 10;
        // 你指定的灵兽技能栏固定位置
        private static readonly Vector3 LingShouSkillSlotPosition = new Vector3(-500.0f, 500.0f, 0f);

        [HarmonyPatch("addEquipSeid")]
        [HarmonyPostfix]
        public static void addEquipSeidPatch(KBEngine.Avatar __instance)
        {

            var uiFightPanel = UIFightPanel.Inst;
            if (uiFightPanel == null || uiFightPanel.FightSkills == null || uiFightPanel.FightSkills.Count == 0)
                return;

            // 扫描已装备物品，找出类型为 18（灵兽）的装备
            int itemId = -1;
            foreach (KBEngine.ITEM_INFO value in __instance.equipItemList.values)
            {
                if (ItemUtil.GetItemType(value.itemId) == 18 || ItemUtil.GetItemType(value.itemId) == 19)
                {
                    itemId = value.itemId;
                    break;
                }
            }

            if (itemId <= -1)
            {
                return;
            }

            // 从配置中读取该灵兽对应的 buff 与技能
            LingShouData skill = LingShouData.LSData.Find(i => i.LingShouItem == itemId);
            if (skill == null)
            {
                return;
            }

            // 先补上灵兽附带 buff
            for (int i = 0; i < skill.LingShouBuff.Length; i++)
            {
                __instance.spell.addBuff(skill.LingShouBuff[i], skill.LingShouBuffCount[i]);
            }

            for (int i = 0; i < skill.LingShouStaticSkill.Length; i++)
            {
                new StaticSkill(skill.LingShouStaticSkill[i], 0, 5).Puting(__instance, __instance, 1); ;
            }

            if (__instance == PlayerEx.Player)
            {
                // 再把灵兽神通挂到战斗技能栏
                AddLingShouFightSkill(__instance, uiFightPanel, skill.LingShouSkill);
            }
        }

        private static void AddLingShouFightSkill(KBEngine.Avatar avatar, UIFightPanel uiFightPanel, int skillId)
        {
            if (avatar == null || uiFightPanel == null || skillId <= 0)
            {
                return;
            }

            // 确保存在第 11 个技能槽（索引 10）
            EnsureLingShouSkillSlot(uiFightPanel);
            if (uiFightPanel.FightSkills == null || uiFightPanel.FightSkills.Count <= BaseSkillSlotCount)
            {
                return;
            }

            int skillIndex = BaseSkillSlotCount;
            // 创建战斗技能对象并同步到角色技能列表
            var fightSkill = new GUIPackage.Skill(skillId, 0, 10);
            avatar.skill.Add(fightSkill);

            UIFightSkillItem skillItem = uiFightPanel.FightSkills[skillIndex];
            skillItem.SetSkill(fightSkill);

            // 设置热键为固定 F2，并刷新左上角热键文本
            KeyCode hotKey = KeyCode.F2;
            skillItem.HotKey = hotKey;
            AddDaoJuPatch.UpdateHotKeyText(skillItem.gameObject, hotKey);
        }

        private static void EnsureLingShouSkillSlot(UIFightPanel uiFightPanel)
        {
            if (uiFightPanel.FightSkills == null || uiFightPanel.FightSkills.Count < BaseSkillSlotCount)
            {
                IsToolsMain.Error("技能栏数量不足，无法添加灵兽技能");
                return;
            }

            // 已存在第 11 格时，仅校正其布局属性和固定位置
            if (uiFightPanel.FightSkills.Count > BaseSkillSlotCount)
            {
                Transform exist = uiFightPanel.FightSkills[BaseSkillSlotCount].transform;
                SetOutOfLayout(exist.gameObject);
                exist.localPosition = LingShouSkillSlotPosition;
                return;
            }

            // 不存在则克隆一个基础按钮作为灵兽技能槽
            GameObject template = uiFightPanel.FightSkills[0].gameObject;
            Transform parentTransform = template.transform.parent;

            GameObject newBtn = UnityEngine.Object.Instantiate(template, parentTransform, false);
            newBtn.name = "FightSkillItem_LingShou";
            SetOutOfLayout(newBtn);
            newBtn.transform.localPosition = LingShouSkillSlotPosition;

            UIFightSkillItem newItem = newBtn.GetComponent<UIFightSkillItem>();
            if (newItem == null)
            {
                IsToolsMain.Error("灵兽技能栏创建失败：未找到 UIFightSkillItem 组件");
                return;
            }

            // 清空模板遗留状态并加入技能栏列表
            newItem.Clear();
            uiFightPanel.FightSkills.Add(newItem);
        }

        private static void SetOutOfLayout(GameObject go)
        {
            var layoutElement = go.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = go.AddComponent<LayoutElement>();
            }
            layoutElement.ignoreLayout = true;
        }

    }
}
