using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Task
{
    [DialogEvent("IS_SetTaskIndex")]
    [DialogEvent("IS_任务跳转特定阶段")]
    public class IS_SetTaskIndex : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var taskId = command.GetInt(0);
            var taskIndex = command.GetInt(1);
            taskId.SetTaskIndex(taskIndex);
            JSONObject jsonobject = env.player.taskMag._TaskData["Task"][taskId.ToString()];
            string str = Tools.instance.Code64ToString(jsonData.instance.TaskJsonData[taskId.ToString()]["Name"].str);
            string msg = "<color=#FF0000>" + str + "</color>任务进度已更新";
            UIPopTip.Inst.Pop(msg, "Task_start", PopTipIconType.任务进度);
            callback?.Invoke();
        }
    }
}