using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;
using top.Isteyft.MCS.JiuZhou.GameData;

namespace top.Isteyft.MCS.JiuZhou.DialogEnv
{
    [DialogEnvQuery("YZ_CheckLuDing")]
    public class YZ_CheckLuDing : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            // 获取传入的NPC ID
            int npcid = context.GetMyArgs(0, 0).ToNpcNewId();

            // 检查IsToolsMain.YouZhouData.Data是否包含LuDing
            if (IsToolsMain.YouZhouData != null && IsToolsMain.YouZhouData.Data != null)
            {
                if (IsToolsMain.YouZhouData.Data.TryGetValue("LuDing", out string luDingStr) && !string.IsNullOrEmpty(luDingStr))
                {
                    var luDing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(luDingStr);

                    // 检查NPC ID是否在数组中
                    return luDing.Any(npc => npc.ContainsKey("id") && npc["id"] == npcid);
                }
            }

            // 默认返回false
            return false;
        }
    }
}