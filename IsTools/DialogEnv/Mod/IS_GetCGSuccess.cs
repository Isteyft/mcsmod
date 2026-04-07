using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Data;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetCGSuccess")]
    public class IS_GetCGSuccess : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            int id = context.GetMyArgs(0, 0);
            return CGData.IsSuccess(id);
        }
    }
}
