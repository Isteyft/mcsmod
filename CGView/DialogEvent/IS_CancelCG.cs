using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.CGView.Patch;
using top.Isteyft.MCS.IsTools.Data;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.CGView.DialogEvent
{
    [DialogEvent("IS_CancelCG")]
    [DialogEvent("IS_取消成就")]
    public class IS_CancelCG : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var id = command.GetInt(0);

            if (CGData.IsSuccess(id))
            {
                CGData.successData.Remove(id);
                ModConfigUtils.SetConfigIntList("successCG", CGData.successData);
                UIPopTip.Inst.Pop($"「{CGData.GetCGId(id).CGName}」已经完成");
                TuJianDBPatch.TujianDBRefresh();
            }
            callback?.Invoke();
        }
    }
}
