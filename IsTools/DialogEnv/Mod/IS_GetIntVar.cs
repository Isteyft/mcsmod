using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Fungus;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetIntVar")]
    public class IS_GetIntVar : IDialogEnvQuery
    {
        private string key;

        public object Execute(DialogEnvQueryContext context)
        {
            this.key = context.GetMyArgs(0, "");
            //DialogEnvironment env = context.Env;
            Flowchart flowchart = context.Env.flowchart;
            //Flowchart flowchart = TempFlowchart.GetFlowchart(this.key);
            IsToolsMain.Log($"获取原版{key}变量");
            try
            {
                
                return flowchart.GetIntegerVariable(key);
            } catch
            {
                IsToolsMain.Error($"获取原版{key}变量出错");
                return "0";
            }
        }
    }
}