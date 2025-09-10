using JSONClass;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Task
{

    [DialogEvent("IS_SetTaskCompleted")]
    [DialogEvent("IS_任务完成")]
    public class IS_SetTaskCompleted : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var taskId = command.GetInt(0);
            taskId.SetTaskCompleted();
            string name = TaskJsonData.DataDict[taskId].Name;
            string msg = "<color=#FF0000>" + name + "</color>任务已完成";
            UIPopTip.Inst.Pop(msg, "Task_complete", PopTipIconType.任务完成);
            callback?.Invoke();
        }
    }

}