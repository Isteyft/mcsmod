using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Player
{
    [DialogEvent("IS_SetSkillLevel")]
    public class IS_SetSkillLevel : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 直接获取参数
            int skillId = command.GetInt(0);
            int targetLevel = command.GetInt(1);

            // 获取玩家对象
            var player = PlayerEx.Player;

            // 直接设置功法等级
            var skill = player.hasStaticSkillList.Find(s => s.itemId == skillId);
            if (skill != null)
            {
                skill.level = Math.Min(targetLevel, 5); // 硬限制最大5级

                // 刷新装备的技能
                var equipIndex = player.equipStaticSkillList.FindIndex(s => s.itemId == skillId);
                if (equipIndex >= 0)
                {
                    player.UnEquipStaticSkill(skillId);
                    player.equipStaticSkill(skillId, equipIndex);
                }
            }

            callback?.Invoke();
        }
    }
}