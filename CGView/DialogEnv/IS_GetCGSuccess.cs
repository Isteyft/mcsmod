using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.CGView.DialogEnv
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
