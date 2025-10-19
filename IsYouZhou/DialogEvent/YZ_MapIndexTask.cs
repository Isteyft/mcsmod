using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Scene;
using top.Isteyft.MCS.YouZhou.Utils;

namespace top.Isteyft.MCS.YouZhou.DialogEvent
{
    [DialogEvent("YZ_MapIndexTask")]
    public class YZ_MapIndexTask : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            bool wasActive = AllMapBase.activeTasks.Contains(index);
            
            if (wasActive)
            {
                // 存在则移除（关闭显示）
                AllMapBase.activeTasks.Remove(index);
                IsToolsMain.LogInfo($"YZ_MapIndexTask: 移除任务标记 - 节点 {index}");
            }
            else
            {
                // 不存在则添加（开启显示）
                AllMapBase.activeTasks.Add(index);
                IsToolsMain.LogInfo($"YZ_MapIndexTask: 添加任务标记 - 节点 {index}");
            }
            
            IsToolsMain.LogInfo($"YZ_MapIndexTask: 当前 activeTasks 列表: [{string.Join(", ", AllMapBase.activeTasks)}]");
            callback?.Invoke();
        }
    }
}
