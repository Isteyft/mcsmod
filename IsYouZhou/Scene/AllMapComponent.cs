﻿using BepInEx;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using YSGame;
using GetWay;
using System.Collections;

namespace top.Isteyft.MCS.JiuZhou.Scene
{
    // 每个路点都有这么一个对象
    public class AllMapComponent : MapComponent
    {
        // 移动速度相关参数
        private float MoveBaseSpeedMin = 1.5f;// 最小基础移动速度
        private float MoveBaseSpeed = 4f;// 基础移动速度
        public UnityEngine.GameObject enter;// 路点游戏对象
        public bool NoEnter = false;// 隐藏进入
        public LudianJson Data;// 路点数据
        public GameObject task;      // 任务标记对象
        public GameObject shijian;   // 事件标记对象
        public bool showTask = false;    // 是否显示任务
        public bool showShijian = false;  // 是否显示事件

        protected override void Start()
        {
            base.Start();
            MapMoveData currentMapData = null;
            IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);
            if (currentMapData != null && currentMapData.canmoveIndex != null)
            {
                this.nextIndex = currentMapData.canmoveIndex;
            }
        }

        public override int getAvatarNowMapIndex()
        {
            // 从玩家副本控制数据中获取当前地图索引
            return PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
        }
        public override void CloseLuDian()
        {
            //base.CloseLuDian();
            UnityEngine.GameObject gameObject = this.enter;
            if (gameObject != null)
            {
                gameObject.SetActive(false);// 隐藏路点
            }
        }
        public override void showLuDian()
        {
            //base.showLuDian();
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
            this.ComAvatar.NowMapIndex = this.NodeIndex;
        }
        public void setAvatarNowMapIndex(int nodeIndex)
        {
            // 保存上一个地图索引
            Tools.instance.fubenLastIndex = this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex;
            // 更新当前地图索引为指定的节点索引
            this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex = nodeIndex;
            this.ComAvatar.NowMapIndex = this.NodeIndex;
        }
        public new bool CanClick()
        {
            // 当玩家正在移动，或者玩家当前就在这个节点，或者有事件正在运行时，不能点击
            return AllMapManage.instance.isPlayMove || this.getAvatarNowMapIndex() == this.NodeIndex || DialogAnalysis.IsRunningEvent;
        }

        public override void EventRandom()
        {
            // 移动到这个节点
            this.AvatarMoveToThis();
            // 增加时间
            this.BaseAddTime();
        }
        
        public override void AvatarMoveToThis()
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool isDunShu = playerController.ShowType == MapPlayerShowType.遁术;

            // 获取当前地图数据
            MapMoveData currentMapData = null;
            IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);

            // 获取当前所在节点
            int avatarNowMapIndex = getAvatarNowMapIndex();

            if (!isDunShu)
            {
                // 非遁术状态
                if (currentMapData != null && currentMapData.canmove)
                {
                    IsToolsMain.LogInfo(ComAvatar.NowMapIndex);
                    IsToolsMain.LogInfo(NodeIndex);
                    AllMapManage.instance.mapIndex[avatarNowMapIndex].CloseLuDian();
                    
                    // 检查是否是相邻节点，如果是则使用 MapMoveNode 路径移动
                    if (IsAdjacentNode(avatarNowMapIndex, this.NodeIndex))
                    {
                        StartCoroutine(MoveToAdjacentNode(avatarNowMapIndex, this.NodeIndex, playerController));
                    }
                    else
                    {
                        // 非相邻节点，使用路径查找
                        MoveToNode(this.NodeIndex, false);
                    }
                    
                    AllMapBase.RefreshMarksFromStaticData();
                    OnArriveAtNode(this.NodeIndex);
                }
                else
                {
                    // 没有飞行遁术且不允许普通移动
                    UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
                }
            }
            else
            {
                // 遁术状态
                if (currentMapData != null && currentMapData.flyAny)
                {
                    // 使用遁术移动
                    MoveToNode(this.NodeIndex, true);
                }
                else
                {
                    // 可以普通移动，计算路径
                    MoveToNode(this.NodeIndex, false);
                }
            }
        }

        /// <summary>
        /// 检查两个节点是否相邻（可以直接通过 MapMoveNode 到达）
        /// </summary>
        private bool IsAdjacentNode(int fromIndex, int toIndex)
        {
            // 查找 MapMoveNode 根对象
            UnityEngine.GameObject mapMoveNodeRoot = UnityEngine.GameObject.Find("MapMoveNode");
            if (mapMoveNodeRoot == null)
            {
                return false;
            }

            // 获取所有 MapMoveNode
            MapMoveNode[] allMoveNodes = mapMoveNodeRoot.GetComponentsInChildren<MapMoveNode>();
            
            // 查找连接两个节点的 MapMoveNode
            foreach (MapMoveNode moveNode in allMoveNodes)
            {
                if ((moveNode.StartNode == fromIndex && moveNode.EndNode == toIndex) ||
                    (moveNode.EndNode == fromIndex && moveNode.StartNode == toIndex))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// 移动到相邻节点 - 使用 MapMoveNode 路径
        /// </summary>
        private System.Collections.IEnumerator MoveToAdjacentNode(int fromIndex, int toIndex, MapPlayerController playerController)
        {
            AllMapManage.instance.isPlayMove = true;
            
            yield return StartCoroutine(MoveAlongMapMoveNodes(fromIndex, toIndex, playerController));
            
            // 更新当前位置
            setAvatarNowMapIndex(toIndex);
            playerController.SetSpeed(0);
            AllMapManage.instance.isPlayMove = false;
            this.showLuDian();
            
            // 到达路点后触发任务和事件检查
            OnArriveAtNode(toIndex);
        }



        #region
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
                AllMapBase.RefreshMarksFromStaticData();
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
                    
                    // 到达路点后触发任务和事件检查
                    OnArriveAtNode(targetNodeIndex);
                    
                    YSFuncList.Ints.Continue();
                };
                completeQueue.Enqueue(completeAction);
                YSFuncList.Ints.AddFunc(completeQueue);
                // 刷新任务事件
                AllMapBase.RefreshMarksFromStaticData();
            }
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
        // 沿路径移动的协程 - 使用 MapMoveNode 路径点
        private System.Collections.IEnumerator MoveAlongPath(List<int> path, MapPlayerController playerController, BaseMapCompont startComp)
        {
            KBEngine.Avatar player = PlayerEx.Player;

            // 关闭起点路点
            startComp.CloseLuDian();

            // 遍历路径中的每个节点（除了起点）
            for (int i = 1; i < path.Count; i++)
            {
                int fromIndex = path[i - 1];
                int targetIndex = path[i];
                BaseMapCompont targetComp = AllMapManage.instance.mapIndex[targetIndex];

                // 使用 MapMoveNode 路径点进行移动
                yield return StartCoroutine(MoveAlongMapMoveNodes(fromIndex, targetIndex, playerController));

                // 更新当前地图索引
                this.NodeIndex = targetIndex;
                this.setAvatarNowMapIndex();
                AllMapBase.RefreshMarksFromStaticData();
            }

            // 移动完成
            playerController.SetSpeed(0);
            AllMapManage.instance.isPlayMove = false;
            this.showLuDian();

            // 到达最终目标路点后触发任务和事件检查
            OnArriveAtNode(path[path.Count - 1]);
        }

        /// <summary>
        /// 沿着 MapMoveNode 路径点移动 - 原游戏寻路方式
        /// </summary>
        private System.Collections.IEnumerator MoveAlongMapMoveNodes(int fromIndex, int toIndex, MapPlayerController playerController)
        {
            KBEngine.Avatar player = PlayerEx.Player;

            // 查找 MapMoveNode 根对象
            UnityEngine.GameObject mapMoveNodeRoot = UnityEngine.GameObject.Find("MapMoveNode");
            if (mapMoveNodeRoot == null)
            {
                // 如果没有 MapMoveNode，直接直线移动
                yield return StartCoroutine(MoveDirectly(toIndex, playerController));
                yield break;
            }

            // 获取所有 MapMoveNode
            MapMoveNode[] allMoveNodes = mapMoveNodeRoot.GetComponentsInChildren<MapMoveNode>();
            List<UnityEngine.GameObject> pathNodes = new List<UnityEngine.GameObject>();
            
            // 添加玩家当前位置作为起点
            pathNodes.Add(playerController.gameObject);
            List<UnityEngine.GameObject> movePathNodes = new List<UnityEngine.GameObject>();

            // 查找连接 fromIndex 和 toIndex 的 MapMoveNode
            foreach (MapMoveNode moveNode in allMoveNodes)
            {
                if ((moveNode.StartNode == fromIndex && moveNode.EndNode == toIndex) ||
                    (moveNode.EndNode == fromIndex && moveNode.StartNode == toIndex))
                {
                    movePathNodes.Add(moveNode.gameObject);
                }
            }

            // 如果当前位置是路径终点，反转路径方向
            if (movePathNodes.Count > 0 && fromIndex == movePathNodes[0].GetComponent<MapMoveNode>().EndNode)
            {
                movePathNodes.Reverse();
            }

            // 添加路径节点
            foreach (UnityEngine.GameObject node in movePathNodes)
            {
                pathNodes.Add(node);
            }

            // 添加目标位置
            BaseMapCompont targetComp = AllMapManage.instance.mapIndex[toIndex];
            Transform playerPosTransform = targetComp.transform.Find("PlayerPosition");
            pathNodes.Add((playerPosTransform != null) ? playerPosTransform.gameObject : targetComp.gameObject);

            // 沿着路径节点移动
            for (int j = 1; j < pathNodes.Count; j++)
            {
                int nodeIndex = j;
                
                // 计算两点间距离
                float distance = Vector2.Distance(pathNodes[nodeIndex - 1].transform.position, pathNodes[nodeIndex].transform.position);
                
                // 根据玩家遁速计算移动速度
                float speed = (player.dunSu > 100) ? 
                    (MoveBaseSpeedMin + MoveBaseSpeed) : 
                    (MoveBaseSpeedMin + MoveBaseSpeed * ((float)player.dunSu / 100f));
                
                // 遁术状态下速度翻倍
                if (playerController.ShowType == MapPlayerShowType.遁术)
                {
                    speed *= 2f;
                }

                // 计算移动时间
                float duration = distance / speed;

                // 开始移动
                playerController.SetSpeed(1);
                iTween.MoveTo(playerController.gameObject, iTween.Hash(
                    "x", pathNodes[nodeIndex].transform.position.x,
                    "y", pathNodes[nodeIndex].transform.position.y,
                    "z", playerController.transform.position.z,
                    "time", duration,
                    "islocal", false,
                    "EaseType", "linear"
                ));

                WASDMove.waitTime = duration;
                WASDMove.needWait = true;

                // 等待移动完成
                yield return new WaitForSeconds(duration);
            }
        }

        /// <summary>
        /// 直接移动到目标节点（当没有 MapMoveNode 时使用）
        /// </summary>
        private System.Collections.IEnumerator MoveDirectly(int targetIndex, MapPlayerController playerController)
        {
            KBEngine.Avatar player = PlayerEx.Player;
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
        }
        #endregion

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

        #region 自定义任务事件
        public void SetTaskVisible(bool visible)
        {
            showTask = visible;
            if (task != null)
            {
                task.SetActive(visible);
            }
            else if (visible)
            {
                IsToolsMain.Warning($"节点 {this.NodeIndex} 没有task子物体，无法显示任务标记");
            }
        }

        public void SetShijianVisible(bool visible)
        {
            showShijian = visible;
            if (shijian != null)
            {
                shijian.SetActive(visible);
            }
            else if (visible)
            {
                IsToolsMain.Warning($"节点 {this.NodeIndex} 没有shijian子物体，无法显示事件标记");
            }
        }
        public void ToggleTask()
        {
            // 切换任务可见
            SetTaskVisible(!showTask);
        }

        public void ToggleShijian()
        {
            // 切换事件可见
            SetShijianVisible(!showShijian);
        }
        
        /// <summary>
        /// 到达路点时触发任务和事件检查
        /// </summary>
        /// <param name="nodeIndex">到达的节点索引</param>
        private void OnArriveAtNode(int nodeIndex)
        {
            
            // 检查并触发任务
            if (AllMapBase.activeTasks.Contains(nodeIndex))
            {
                IsToolsMain.LogInfo($"到达路点 {nodeIndex}，触发任务检查");
                
                // 从列表中移除该任务标记
                AllMapBase.activeTasks.Remove(nodeIndex);
                
                // 刷新UI显示（隐藏该节点的任务标记）
                AllMapBase.RefreshMarksFromStaticData();
                
                // 触发任务对话事件
                DialogAnalysis.StartTestDialogEvent("RunLua*九州任务#九州任务点", null);
            }
            
            // 检查并触发事件（使用else if避免同时触发）
            else if (AllMapBase.activeShijians.Contains(nodeIndex))
            {
                IsToolsMain.LogInfo($"到达路点 {nodeIndex}，触发事件检查");
                
                // 从列表中移除该事件标记
                AllMapBase.activeShijians.Remove(nodeIndex);
                
                // 刷新UI显示（隐藏该节点的事件标记）
                AllMapBase.RefreshMarksFromStaticData();
                
                // 触发事件对话事件
                DialogAnalysis.StartTestDialogEvent("RunLua*九州事件#九州事件点", null);
            }
        }
        #endregion
    }
}
