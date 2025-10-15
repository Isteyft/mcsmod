using BepInEx;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class AllMapComponent : MapComponent
    {
        // 移动速度相关参数
        private float MoveBaseSpeedMin = 1.5f;// 最小基础移动速度
        private float MoveBaseSpeed = 4f;// 基础移动速度
        public UnityEngine.GameObject enter;// 路点游戏对象
        public bool NoEnter = false;// 是否没有路点
        public LudianJson Data;// 路点数据
        public override int getAvatarNowMapIndex()
        {
            // 从玩家副本控制数据中获取当前地图索引
            return PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
        }
        public override void CloseLuDian()
        {
            UnityEngine.GameObject gameObject = this.enter;
            if (gameObject != null)
            {
                gameObject.SetActive(false);// 隐藏路点
            }
        }
        public override void showLuDian()
        {
            if (this.enter != null)
            {
                // 如果有路点，则显示它
                this.enter.SetActive(true);
            }
            else
            {
                // 如果没有入口对象但有数据，且没有入口标志，且没有正在运行的事件
                if (this.Data != null && this.NoEnter && !DialogAnalysis.IsRunningEvent)
                {
                    // 检查是否有事件要触发
                    if (!this.Data.Event.IsNullOrWhiteSpace())
                    {
                        // 开始对话事件
                        DialogAnalysis.StartDialogEvent(this.Data.Event, null);
                    }
                    else
                    {
                        // 检查是否有Lua脚本要执行
                        if (!this.Data.Lua.IsNullOrWhiteSpace())
                        {
                            // 开始测试对话事件
                            DialogAnalysis.StartTestDialogEvent(this.Data.Lua, null);
                        }
                    }
                }
            }
        }
        public override void setAvatarNowMapIndex()
        {
            // 保存上一个地图索引
            Tools.instance.fubenLastIndex = this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex;
            // 更新当前地图索引为这个节点的索引
            this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex = this.NodeIndex;
        }
        public void setAvatarNowMapIndex(int nodeIndex)
        {
            // 保存上一个地图索引
            Tools.instance.fubenLastIndex = this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex;
            // 更新当前地图索引为指定的节点索引
            this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex = nodeIndex;
        }
        public new bool CanClick()
        {
            // 当玩家正在移动，或者玩家当前就在这个节点，或者有事件正在运行时，不能点击
            return AllMapManage.instance.isPlayMove || this.getAvatarNowMapIndex() == this.NodeIndex || DialogAnalysis.IsRunningEvent;
        }
        //public override void AvatarMoveToThis()
        //{
        //    // 获取地图玩家控制器实例
        //    MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
        //    bool hasflyDunshu = playerController.ShowType != MapPlayerShowType.遁术;
        //    if (hasflyDunshu)
        //    {
        //        UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
        //    }
        //    else
        //    {
        //        KBEngine.Avatar player = PlayerEx.Player;
        //        // 获取目标地图组件
        //        BaseMapCompont baseMapCompont = AllMapManage.instance.mapIndex[this.NodeIndex];
        //        IsToolsMain.Log("点击"+this.NodeIndex.ToString());
        //        IsToolsMain.Log(baseMapCompont.ToString());
        //        // 获取玩家当前所在位置的地图索引
        //        int avatarNowMapIndex = this.getAvatarNowMapIndex();
        //        // 获取玩家当前所在位置的地图组件
        //        BaseMapCompont Nowcomp = AllMapManage.instance.mapIndex[avatarNowMapIndex];
        //        // 设置移动状态为true，表示玩家正在移动
        //        AllMapManage.instance.isPlayMove = true;
        //        // 计算当前位置到目标位置的距离
        //        float num = Vector2.Distance(Nowcomp.transform.position, baseMapCompont.transform.position);
        //        // 计算移动速度：
        //        // 如果玩家遁术值大于200，则使用最大速度
        //        // 否则根据遁术值按比例计算速度
        //        float num2 = (player.dunSu > 200) ? ((this.MoveBaseSpeedMin + this.MoveBaseSpeed) * 2f) : (this.MoveBaseSpeedMin + this.MoveBaseSpeed * ((float)player.dunSu / 100f));
        //        // 速度乘以2（可能是为了调整移动时间）
        //        num2 *= 2f;
        //        // 计算移动所需时间 = 距离 / 速度
        //        float num3 = num / num2;
        //        // 创建动作队列
        //        Queue<UnityAction> queue = new Queue<UnityAction>();
        //        // 第一个动作：开始移动
        //        UnityAction item = delegate ()
        //        {
        //            // 关闭当前地点的路点标记
        //            Nowcomp.CloseLuDian();
        //            // 设置玩家移动速度为1
        //            playerController.SetSpeed(1);
        //            iTween.MoveTo(playerController.gameObject, iTween.Hash(new object[]
        //            {
        //                "x",
        //                this.transform.position.x,// 目标x坐标
        //                "y",
        //                this.transform.position.y - 0.2f, // 目标y坐标（稍微向下偏移0.2）
        //                "z",
        //                playerController.transform.position.z,// z坐标保持不变
        //                "time",
        //                num3,// 移动时间
        //                "islocal",
        //                false, // 不使用局部坐标
        //                "EaseType",
        //                "linear"// 线性移动
        //            }));
        //            // 设置WASD移动的等待时间和需要等待标志
        //            WASDMove.waitTime = num3;
        //            WASDMove.needWait = true;
        //            // 在移动完成后调用callContinue方法
        //            this.Invoke("callContinue", num3);
        //        };
        //        queue.Enqueue(item);
        //        YSFuncList.Ints.AddFunc(queue);
        //        // 创建第二个动作队列
        //        Queue<UnityAction> queue2 = new Queue<UnityAction>();
        //        // 第二个动作：移动完成后的处理
        //        UnityAction item2 = delegate ()
        //        {
        //            // 更新玩家当前地图索引
        //            this.setAvatarNowMapIndex();
        //            // 停止玩家移动
        //            playerController.SetSpeed(0);
        //            // 设置移动状态为false
        //            AllMapManage.instance.isPlayMove = false;
        //            // 显示目标地点的路点标记
        //            this.showLuDian();
        //            // 继续执行后续动作
        //            YSFuncList.Ints.Continue();
        //        };
        //        queue2.Enqueue(item2);
        //        YSFuncList.Ints.AddFunc(queue2);
        //    }
        //}
        //public override void AvatarMoveToThis()
        //{
        //    MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
        //    bool hasDunShu = playerController.ShowType != MapPlayerShowType.遁术;

        //    // 获取当前地图数据
        //    MapMoveData currentMapData = null;
        //    IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);
        //    //bool hasData = IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);
        //    //// 调试2：打印地图数据情况
        //    //IsToolsMain.Log($"节点 {this.NodeIndex} 的数据: {(hasData ? "存在" : "不存在")}");
        //    //if (hasData)
        //    //{
        //    //    IsToolsMain.Log($"可移动: {currentMapData.canmove}, 可移动目标: [{string.Join(",", currentMapData.canmoveIndex)}]");
        //    //}
        //    if (hasDunShu)
        //    {
        //        if (currentMapData != null && currentMapData.canmove)
        //        {
        //            // 可以普通移动，计算路径
        //            KBEngine.Avatar player = PlayerEx.Player;
        //            BaseMapCompont targetMapCompont = AllMapManage.instance.mapIndex[this.NodeIndex];
        //            int avatarNowMapIndex = this.getAvatarNowMapIndex();
        //            BaseMapCompont nowComp = AllMapManage.instance.mapIndex[avatarNowMapIndex];

        //            // 查找从当前位置到目标位置的路径
        //            List<int> path = FindPath(avatarNowMapIndex, this.NodeIndex);
        //            if (path == null || path.Count == 0)
        //            {
        //                UIPopTip.Inst.Pop("无法找到前往目标地点的路径。", PopTipIconType.叹号);
        //                return;
        //            }

        //            // 开始移动
        //            AllMapManage.instance.isPlayMove = true;
        //            StartCoroutine(MoveAlongPath(path, playerController, nowComp));
        //        }
        //        else
        //        {
        //            // 没有飞行遁术且不允许普通移动
        //            UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
        //        }
        //    }
        //    else
        //    {
        //        KBEngine.Avatar player = PlayerEx.Player;
        //        // 获取目标地图组件
        //        BaseMapCompont baseMapCompont = AllMapManage.instance.mapIndex[this.NodeIndex];
        //        //IsToolsMain.Log("点击" + this.NodeIndex.ToString());
        //        //IsToolsMain.Log(baseMapCompont.ToString());
        //        // 获取玩家当前所在位置的地图索引
        //        int avatarNowMapIndex = this.getAvatarNowMapIndex();
        //        // 获取玩家当前所在位置的地图组件
        //        BaseMapCompont Nowcomp = AllMapManage.instance.mapIndex[avatarNowMapIndex];
        //        // 设置移动状态为true，表示玩家正在移动
        //        AllMapManage.instance.isPlayMove = true;
        //        // 计算当前位置到目标位置的距离
        //        float num = Vector2.Distance(Nowcomp.transform.position, baseMapCompont.transform.position);
        //        // 计算移动速度：
        //        // 如果玩家遁术值大于200，则使用最大速度
        //        // 否则根据遁术值按比例计算速度
        //        float num2 = (player.dunSu > 200) ? ((this.MoveBaseSpeedMin + this.MoveBaseSpeed) * 2f) : (this.MoveBaseSpeedMin + this.MoveBaseSpeed * ((float)player.dunSu / 100f));
        //        // 速度乘以2（可能是为了调整移动时间）
        //        num2 *= 2f;
        //        // 计算移动所需时间 = 距离 / 速度
        //        float num3 = num / num2;
        //        // 创建动作队列
        //        Queue<UnityAction> queue = new Queue<UnityAction>();
        //        // 第一个动作：开始移动
        //        UnityAction item = delegate ()
        //        {
        //            // 关闭当前地点的路点标记
        //            Nowcomp.CloseLuDian();
        //            // 设置玩家移动速度为1
        //            playerController.SetSpeed(1);
        //            iTween.MoveTo(playerController.gameObject, iTween.Hash(new object[]
        //            {
        //                        "x",
        //                        this.transform.position.x,// 目标x坐标
        //                        "y",
        //                        this.transform.position.y - 0.2f, // 目标y坐标（稍微向下偏移0.2）
        //                        "z",
        //                        playerController.transform.position.z,// z坐标保持不变
        //                        "time",
        //                        num3,// 移动时间
        //                        "islocal",
        //                        false, // 不使用局部坐标
        //                        "EaseType",
        //                        "linear"// 线性移动
        //            }));
        //            // 设置WASD移动的等待时间和需要等待标志
        //            WASDMove.waitTime = num3;
        //            WASDMove.needWait = true;
        //            // 在移动完成后调用callContinue方法
        //            this.Invoke("callContinue", num3);
        //        };
        //        queue.Enqueue(item);
        //        YSFuncList.Ints.AddFunc(queue);
        //        // 创建第二个动作队列
        //        Queue<UnityAction> queue2 = new Queue<UnityAction>();
        //        // 第二个动作：移动完成后的处理
        //        UnityAction item2 = delegate ()
        //        {
        //            // 更新玩家当前地图索引
        //            this.setAvatarNowMapIndex();
        //            // 停止玩家移动
        //            playerController.SetSpeed(0);
        //            // 设置移动状态为false
        //            AllMapManage.instance.isPlayMove = false;
        //            // 显示目标地点的路点标记
        //            this.showLuDian();
        //            // 继续执行后续动作
        //            YSFuncList.Ints.Continue();
        //        };
        //        queue2.Enqueue(item2);
        //        YSFuncList.Ints.AddFunc(queue2);
        //    }
        //}
        public override void AvatarMoveToThis()
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool hasDunShu = playerController.ShowType != MapPlayerShowType.遁术;

            // 获取当前地图数据
            MapMoveData currentMapData = null;
            IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);

            if (hasDunShu)
            {
                if (currentMapData != null && currentMapData.canmove)
                {
                    // 可以普通移动，计算路径
                    MoveToNode(this.NodeIndex, false);
                }
                else
                {
                    // 没有飞行遁术且不允许普通移动
                    UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
                }
            }
            else
            {
                // 使用遁术移动
                MoveToNode(this.NodeIndex, true);
            }
        }

        /// <summary>
        /// 移动到指定节点
        /// </summary>
        /// <param name="targetNodeIndex">目标节点索引</param>
        /// <param name="useDunShu">是否使用遁术移动</param>
        public void MoveToNode(int targetNodeIndex, bool useDunShu)
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            KBEngine.Avatar player = PlayerEx.Player;

            // 获取目标地图组件
            BaseMapCompont targetMapCompont = AllMapManage.instance.mapIndex[targetNodeIndex];

            // 获取玩家当前所在位置的地图索引
            int avatarNowMapIndex = getAvatarNowMapIndex();
            BaseMapCompont nowComp = AllMapManage.instance.mapIndex[avatarNowMapIndex];

            // 设置移动状态为true，表示玩家正在移动
            AllMapManage.instance.isPlayMove = true;

            if (!useDunShu)
            {
                // 普通移动模式 - 使用路径查找
                List<int> path = FindPath(avatarNowMapIndex, targetNodeIndex);
                if (path == null || path.Count == 0)
                {
                    UIPopTip.Inst.Pop("无法找到前往目标地点的路径。", PopTipIconType.叹号);
                    return;
                }

                StartCoroutine(MoveAlongPath(path, playerController, nowComp));
            }
            else
            {
                // 遁术移动模式
                float distance = Vector2.Distance(nowComp.transform.position, targetMapCompont.transform.position);
                float speed = (player.dunSu > 200) ?
                    ((MoveBaseSpeedMin + MoveBaseSpeed) * 2f) :
                    (MoveBaseSpeedMin + MoveBaseSpeed * ((float)player.dunSu / 100f));
                speed *= 2f;
                float duration = distance / speed;

                // 创建动作队列
                Queue<UnityAction> moveQueue = new Queue<UnityAction>();
                UnityAction moveAction = delegate ()
                {
                    nowComp.CloseLuDian();
                    playerController.SetSpeed(1);
                    iTween.MoveTo(playerController.gameObject, iTween.Hash(
                        "x", targetMapCompont.transform.position.x,
                        "y", targetMapCompont.transform.position.y - 0.2f,
                        "z", playerController.transform.position.z,
                        "time", duration,
                        "islocal", false,
                        "EaseType", "linear"
                    ));
                    WASDMove.waitTime = duration;
                    WASDMove.needWait = true;
                    Invoke("callContinue", duration);
                };
                moveQueue.Enqueue(moveAction);
                YSFuncList.Ints.AddFunc(moveQueue);

                // 移动完成后的处理
                Queue<UnityAction> completeQueue = new Queue<UnityAction>();
                UnityAction completeAction = delegate ()
                {
                    setAvatarNowMapIndex(targetNodeIndex);
                    playerController.SetSpeed(0);
                    AllMapManage.instance.isPlayMove = false;
                    targetMapCompont.showLuDian();
                    YSFuncList.Ints.Continue();
                };
                completeQueue.Enqueue(completeAction);
                YSFuncList.Ints.AddFunc(completeQueue);
            }
        }

        /// <summary>
        /// 静态方法：通过命令移动到指定节点
        /// </summary>
        /// <param name="targetNodeIndex">目标节点索引</param>
        public static void CommandMoveToNode(int targetNodeIndex)
        {
            // 获取玩家控制器
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool hasDunShu = playerController.ShowType != MapPlayerShowType.遁术;

            // 获取地图数据
            MapMoveData currentMapData;
            IsToolsMain.MapMoveDatas.TryGetValue(targetNodeIndex, out currentMapData);

            if (currentMapData == null)
            {
                UIPopTip.Inst.Pop("目标地点不存在或不可达。", PopTipIconType.叹号);
                return;
            }

            // 获取目标地图组件（确保是AllMapComponent类型）
            if (!AllMapManage.instance.mapIndex.TryGetValue(targetNodeIndex, out var targetMapComponent))
            {
                UIPopTip.Inst.Pop($"无法找到节点 {targetNodeIndex} 的地图组件", PopTipIconType.叹号);
                return;
            }

            // 检查目标组件是否是AllMapComponent
            AllMapComponent allMapComponent = targetMapComponent as AllMapComponent;
            if (allMapComponent == null)
            {
                UIPopTip.Inst.Pop($"节点 {targetNodeIndex} 不是有效的可移动类型", PopTipIconType.叹号);
                return;
            }

            // 如果玩家有遁术或者目标地点允许普通移动，则可以移动
            if (hasDunShu || currentMapData.canmove)
            {
                // 调用实例方法 MoveToNode
                allMapComponent.MoveToNode(targetNodeIndex, hasDunShu);
            }
            else
            {
                UIPopTip.Inst.Pop("无法抵达目标地点。", PopTipIconType.叹号);
            }
        }
        // 强制移动
        public static void SetAvatarNowMapIndexStatic(int nodeIndex)
        {
            // 获取当前玩家
            KBEngine.Avatar player = PlayerEx.Player;

            // 获取当前场景名称
            string screenName = Tools.getScreenName();

            // 保存上一个地图索引
            Tools.instance.fubenLastIndex = player.fubenContorl[screenName].NowIndex;

            // 更新当前地图索引为指定的节点索引
            player.fubenContorl[screenName].NowIndex = nodeIndex;
        }
        // 路径查找方法 - 使用广度优先搜索
        private List<int> FindPath(int startIndex, int targetIndex)
        {
            if (startIndex == targetIndex) return new List<int>() { startIndex };

            // 记录已访问的节点
            HashSet<int> visited = new HashSet<int>();
            // 记录路径的队列
            Queue<List<int>> queue = new Queue<List<int>>();

            // 从起点开始
            queue.Enqueue(new List<int>() { startIndex });
            visited.Add(startIndex);

            while (queue.Count > 0)
            {
                List<int> currentPath = queue.Dequeue();
                int lastNode = currentPath.Last();

                // 检查是否到达目标
                if (lastNode == targetIndex)
                {
                    return currentPath;
                }

                // 获取当前节点的可移动目标
                MapMoveData nodeData;
                if (IsToolsMain.MapMoveDatas.TryGetValue(lastNode, out nodeData))
                {
                    foreach (int neighbor in nodeData.canmoveIndex)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            List<int> newPath = new List<int>(currentPath);
                            newPath.Add(neighbor);
                            queue.Enqueue(newPath);
                        }
                    }
                }
            }

            // 没有找到路径
            return null;
        }
        // 沿路径移动的协程
        private System.Collections.IEnumerator MoveAlongPath(List<int> path, MapPlayerController playerController, BaseMapCompont startComp)
        {
            KBEngine.Avatar player = PlayerEx.Player;

            // 关闭起点路点
            startComp.CloseLuDian();

            // 遍历路径中的每个节点（除了起点）
            for (int i = 1; i < path.Count; i++)
            {
                int targetIndex = path[i];
                BaseMapCompont targetComp = AllMapManage.instance.mapIndex[targetIndex];

                // 计算移动参数
                float distance = Vector2.Distance(playerController.transform.position, targetComp.transform.position);
                float speed = (player.dunSu > 200) ?
                    ((MoveBaseSpeedMin + MoveBaseSpeed) * 2f) :
                    (MoveBaseSpeedMin + MoveBaseSpeed * ((float)player.dunSu / 100f));
                speed *= 2f;
                float duration = distance / speed;

                // 开始移动
                playerController.SetSpeed(1);
                iTween.MoveTo(playerController.gameObject, iTween.Hash(
                    "x", targetComp.transform.position.x,
                    "y", targetComp.transform.position.y - 0.08f,
                    "z", playerController.transform.position.z,
                    "time", duration,
                    "islocal", false,
                    "EaseType", "linear"
                ));

                WASDMove.waitTime = duration;
                WASDMove.needWait = true;

                // 等待移动完成
                yield return new WaitForSeconds(duration);

                // 更新当前地图索引
                this.NodeIndex = targetIndex;
                this.setAvatarNowMapIndex();
            }

            // 移动完成
            playerController.SetSpeed(0);
            AllMapManage.instance.isPlayMove = false;
            this.showLuDian();
        }
        public override void EventRandom()
        {
            // 移动到这个节点
            this.AvatarMoveToThis();
            // 增加时间
            this.BaseAddTime();
        }
    }
}
