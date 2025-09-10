using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Task
{
    [DialogEnvQuery("IS_GetTaskIndex")]
    public class GetTaskIndex : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            var taskId = context.GetMyArgs(0, 0);
            return taskId.GetTaskIndex();
        }
    }
}
