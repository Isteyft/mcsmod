using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetWay;
using GUIPackage;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{

    [DialogEvent("JZ_GetNodes")]
    public class JZ_GetNodes : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            UIPopTip.Inst.Pop("获取节点。", PopTipIconType.叹号);
            IsToolsMain.LogInfo($"开始获取");
            // 调用刷新函数
            foreach (var pair in MapGetWay.Inst.Dict)
            {
                int key = pair.Key; // 获取键 (int)
                List<int> valueList = pair.Value; // 获取值 (List<int>)

                // 打印或处理键
                IsToolsMain.LogInfo($"当前键: {key}");

                // 遍历列表中的每一个 int
                foreach (int item in valueList)
                {
                    IsToolsMain.LogInfo($"  - 包含的值: {item}");
                }
            }

            // --- 遍历字典并访问 MapNode 属性 ---
            foreach (var pair in MapGetWay.Inst.NodeDict)
            {
                int nodeId = pair.Key;        // 节点的 ID
                MapNode node = pair.Value;    // 节点对象本身

                // 访问 MapNode 类的具体字段
                IsToolsMain.LogInfo($"节点索引: {node.Index}, 坐标: ({node.X}, {node.Y})");
                IsToolsMain.LogInfo($"F值: {node.F}, G值: {node.G}, H值: {node.H}");

                // 如果需要访问父节点 (Parent)
                if (node.Parent != null)
                {
                    IsToolsMain.LogInfo($"  父节点索引: {node.Parent.Index}");
                }
            }
            IsToolsMain.LogInfo($"结束获取");

            callback?.Invoke();
        }
    }
}
