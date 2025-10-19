using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.UI
{

    [DialogEvent("IS_OpenTieJian")]
    [DialogEvent("打开铁剑")]
    public class IS_OpenTieJian : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            UIJianLingPanel.OpenPanel();
            callback?.Invoke();
        }
    }
}
