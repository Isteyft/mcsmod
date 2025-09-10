using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Task
{

    [DialogEvent("IS_NextTask")]
    [DialogEvent("IS_任务跳转阶段")]
    public class IS_NextTask : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var taskId = command.GetInt(0);
            taskId.NextTask();
            JSONObject jsonobject = env.player.taskMag._TaskData["Task"][taskId.ToString()];
            string str = Tools.instance.Code64ToString(jsonData.instance.TaskJsonData[taskId.ToString()]["Name"].str);
            string msg = "<color=#FF0000>" + str + "</color>任务进度已更新";
            UIPopTip.Inst.Pop(msg, "Task_start", PopTipIconType.任务进度);
            callback?.Invoke();
        }
    }

}