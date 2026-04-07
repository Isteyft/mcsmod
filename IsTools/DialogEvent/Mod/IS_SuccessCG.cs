using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Patch.TujianPatch;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Mod
{

    [DialogEvent("IS_SuccessCG")]
    [DialogEvent("IS_解锁CG")]
    public class IS_SuccessCG : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var id = command.GetInt(0);

            if (!CGData.IsSuccess(id))
            {
                CGData.successData.Add(id);
                ModConfigUtils.SetConfigIntList("successCG", CGData.successData);
                UIPopTip.Inst.Pop($"「{CGData.GetCGId(id).CGName}」已经完成");
                TuJianDBPatch.TujianDBRefresh();
            }
            callback?.Invoke();
        }
    }
}
