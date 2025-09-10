using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Npc
{
    [DialogEnvQuery("IS_GetNpcStaticSkillLevel")]
    public class IS_GetNpcStaticSkillLevel : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            var npcid = context.GetMyArgs(0, 0);
            var staticskill = context.GetMyArgs(1, 0);

            npcid = NPCEx.NPCIDToNew(npcid);
            if (NPCEx.IsDeath(npcid))
            {
                //IsToolsMain.Warning($"npc已死亡");
                return -1;
            }
            var npcData = jsonData.instance.AvatarJsonData[npcid.ToString()];
            var staticSkills = npcData["staticSkills"].ToList();
            // 仅添加遍历日志（开始）

            foreach (var skill in staticSkills)
            {
                if (StaticSkillUtils.GetStaticSkillUniqueId(skill) ==
                    StaticSkillUtils.GetStaticSkillUniqueId(staticskill))
                {
                    return StaticSkillUtils.GetSkillLevelById(skill);
                }
            }
            return 0;
        }
    }
}