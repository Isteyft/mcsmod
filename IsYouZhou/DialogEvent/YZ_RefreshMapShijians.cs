using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Scene;

namespace top.Isteyft.MCS.YouZhou.DialogEvent
{
    /// <summary>
    /// 刷新地图事件：清空旧事件，随机生成3个新事件
    /// 用法：[YZ_RefreshMapShijians]
    /// </summary>
    [DialogEvent("YZ_RefreshMapShijians")]
    public class YZ_RefreshMapShijians : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 调用刷新函数
            AllMapBase.RefreshMapShijians();
            callback?.Invoke();
        }
    }
}
