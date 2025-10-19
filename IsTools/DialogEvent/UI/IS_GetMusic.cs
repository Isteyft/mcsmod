using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Patch;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.UI
{

    [DialogEvent("IS_GetMusic")]
    public class IS_GetMusic : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 扫描音乐文件
            // 获取所有音乐名称
            List<string> allMusic = MusicScanner.GetAllMusicNames();

            // 遍历输出
            foreach (string name in allMusic)
            {
                IsToolsMain.Log($"找到音乐: {name}");
            }
            callback?.Invoke();
        }
    }
}
