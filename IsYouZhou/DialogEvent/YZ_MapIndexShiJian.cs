﻿using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.Scene;
using top.Isteyft.MCS.JiuZhou.Utils;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{
    [DialogEvent("YZ_MapIndexShiJian")]
    public class YZ_MapIndexShiJian : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int index = command.GetInt(0, 1);
            bool wasActive = AllMapBase.activeShijians.Contains(index);
            
            if (wasActive)
            {
                // 存在则移除（关闭显示）
                AllMapBase.activeShijians.Remove(index);
                IsToolsMain.LogInfo($"YZ_MapIndexShiJian: 移除事件标记 - 节点 {index}");
            }
            else
            {
                // 不存在则添加（开启显示）
                AllMapBase.activeShijians.Add(index);
                IsToolsMain.LogInfo($"YZ_MapIndexShiJian: 添加事件标记 - 节点 {index}");
            }
            
            IsToolsMain.LogInfo($"YZ_MapIndexShiJian: 当前 activeShijians 列表: [{string.Join(", ", AllMapBase.activeShijians)}]");
            callback?.Invoke();
        }
    }
}
