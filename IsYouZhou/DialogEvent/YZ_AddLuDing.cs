using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using top.Isteyft.MCS.JiuZhou.GameData;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{
    [DialogEvent("YZ_AddLuDing")]
    public class YZ_AddLuDing : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 获取NPC ID
            int npcid = command.GetInt(0, 0).ToNpcNewId();
            // 检查IsToolsMain.YouZhouData.Data是否包含LuDing
            if (IsToolsMain.YouZhouData != null && IsToolsMain.YouZhouData.Data != null)
            {
                List<Dictionary<string, int>> luDing;

                if (IsToolsMain.YouZhouData.Data.TryGetValue("LuDing", out string luDingStr) && !string.IsNullOrEmpty(luDingStr))
                {
                    luDing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(luDingStr);
                }
                else
                {
                    luDing = new List<Dictionary<string, int>>();
                }

                // 检查NPC是否已经在数组中
                bool exists = luDing.Any(npc => npc.ContainsKey("id") && npc["id"] == npcid);

                if (!exists)
                {
                    // 添加新的NPC对象
                    luDing.Add(new Dictionary<string, int> { { "id", npcid } });

                    // 保存回数据中
                    IsToolsMain.YouZhouData.Data["LuDing"] = Newtonsoft.Json.JsonConvert.SerializeObject(luDing, Newtonsoft.Json.Formatting.Indented);
                }
                callback?.Invoke();
            }
        }
    }
}