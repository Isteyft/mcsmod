using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Fungus;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetTMYYear")]
    public class IS_GetTMYYear : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            var year = Tools.instance.getPlayer().worldTimeMag.getNowTime().Year;
            var month = Tools.instance.getPlayer().worldTimeMag.getNowTime().Month;
            if (year % 20 == 0 && month == 2) return true;
            return false;
        }
    }
}