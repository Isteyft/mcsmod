using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Task
{
    [DialogEvent("IS_AddTask")]
    [DialogEvent("添加任务")]
    public class AddTask : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var taskId = command.GetInt(0);
            taskId.TryAddTask();
            env.player.taskMag.addTask(taskId);
            string str = Tools.instance.Code64ToString(jsonData.instance.TaskJsonData[taskId.ToString()]["Name"].str);
            string msg = (jsonData.instance.TaskJsonData[taskId.ToString()]["Type"].n == 0f) ? "获得一条新的传闻" : ("<color=#FF0000>" + str + "</color>任务已开启");
            UIPopTip.Inst.Pop(msg, "Task_start", PopTipIconType.任务进度);
            callback?.Invoke();
        }
    }
}
