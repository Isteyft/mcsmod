using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Npc
{
    [DialogEnvQuery("IS_SearchOneNpcByFav")]
    public class IS_SearchOneNpcByFav : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            var haogan = context.GetMyArgs(0, -201);

            int npcId = 0;
            List<int> list = NpcUtil.SearchNpc(haogan);
            if (list.Count > 0)
            {
                int j = ModUtil.GetRandom(0, list.Count);
                npcId = list[j];
            }
            return npcId;
        }
    }
}
