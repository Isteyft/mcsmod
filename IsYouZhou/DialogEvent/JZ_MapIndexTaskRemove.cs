using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.Scene;
using top.Isteyft.MCS.JiuZhou.Utils;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{
    [DialogEvent("JZ_MapIndexTaskRemove")]
    public class JZ_MapIndexTaskRemove : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            bool wasActive = AllMapBase.activeTasks.Contains(index);
            
            if (wasActive)
            {
                // 存在则移除（关闭显示）
                AllMapBase.activeTasks.Remove(index);
                AllMapBase.RefreshMarksFromStaticData();
                IsToolsMain.LogInfo($"YZ_MapIndexTask: 移除任务标记 - 节点 {index}");
            }
            
            IsToolsMain.LogInfo($"YZ_MapIndexTask: 当前 activeTasks 列表: [{string.Join(", ", AllMapBase.activeTasks)}]");
            callback?.Invoke();
        }
    }
}
