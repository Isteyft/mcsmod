using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine; // 确保引用
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools;
using YSGame.Fight;

[DialogEvent("IS_GetNowSkill")]
public class IS_GetNowSkill : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        if (UIFightPanel.Inst == null)
        {
            IsToolsMain.Error("UIFightPanel 为空");
            callback?.Invoke();
            return;
        }

        int num = 0;
        foreach (UIFightSkillItem uifightSkillItem in UIFightPanel.Inst.FightSkills)
        {
            // 防止列表中有空项
            if (uifightSkillItem == null)
            {
                num++;
                continue;
            }

            if (num >= 0 && num < 10)
            {
                // 1. 获取类型
                Type itemType = uifightSkillItem.GetType();

                // 2. 尝试反射获取 nowSkill 字段
                // 这里的 flags 组合是最全的：非公开 + 实例 + 声明在类上的
                FieldInfo field = itemType.GetField("nowSkill", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                if (field != null)
                {
                    // 3. 获取具体值
                    object nowSkillObj = field.GetValue(uifightSkillItem);

                    if (nowSkillObj != null)
                    {
                        // 4. 获取 SkillID
                        // 假设 SkillID 是 public 的，如果不行也需要反射
                        Type skillType = nowSkillObj.GetType();

                        FieldInfo skillIdField = skillType.GetField("skill_ID");

                        if (skillIdField != null)
                        {
                            object idVal = skillIdField.GetValue(nowSkillObj);
                            if (idVal != null)
                            {
                                int skillId = Convert.ToInt32(idVal);

                                if (FightNowSkills.NowSkill.ContainsKey(num))
                                {
                                    // 如果键存在，更新它
                                    FightNowSkills.NowSkill[num] = skillId;
                                }
                                else
                                {
                                    // 如果键不存在，添加它
                                    FightNowSkills.NowSkill.Add(num, skillId);
                                }
                            }
                        }
                    }
                }
            }
            int tempVal;
            if (!FightNowSkills.NowSkill.TryGetValue(num, out tempVal))
            {
                FightNowSkills.NowSkill.Add(num, 0);
            }
            num++;
        }
        callback?.Invoke();
    }
}