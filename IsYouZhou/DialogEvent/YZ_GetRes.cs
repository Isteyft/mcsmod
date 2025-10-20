using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Utils;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.DialogEvent
{
    [DialogEvent("YZ_GetRes")]
    public class YZ_GetRes : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            // 获取 UIManagerHandle 实例
            var uiManager = IsToolsMain.I.UIManagerHandle;
            string prefabName = "YZMapUI";
            if (uiManager.prefabBank.TryGetValue(prefabName, out GameObject prefab))
            {
                // 实例化预制体
                GameObject instance = GameObject.Instantiate(prefab);

                // 设置父对象为UI管理器，保持世界坐标
                instance.transform.SetParent(uiManager.transform, false);

                // 设置位置（可选）
                instance.transform.localPosition = new Vector3(0.3f, 0.0f, 0.0f);


                IsToolsMain.LogInfo($"成功实例化并显示预制体: {prefabName} 在位置 {instance.transform.position}");
            }
            else
            {
                IsToolsMain.Warning($"找不到预制体: {prefabName}");
            }
        callback?.Invoke();
        }
    }
}
