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
using GetWay;
using System.Collections;
using MaiJiu.MCS.HH.Scene;
using MaiJiu.MCS.HH.Tool;

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

        private MCommand reachEvent;
        //private MCommand enterEvent;

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
            // 注意：返回 true 表示"不可点击"（被 AllMapClick 以 !CanClick() 方式调用）
            return AllMapManage.instance.isPlayMove || this.getAvatarNowMapIndex() == this.NodeIndex || DialogAnalysis.IsRunningEvent;
        }

        public override void EventRandom()
        {
            int avatarNowMapIndex = getAvatarNowMapIndex();
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool isDunShu = playerController.ShowType == MapPlayerShowType.遁术;

            // 安全判断相邻：MapGetWay.Dict 可能不含该 key，直接用基类 nextIndex 判断
            bool isNearly = false;
            if (AllMapManage.instance.mapIndex.ContainsKey(avatarNowMapIndex))
            {
                isNearly = AllMapManage.instance.mapIndex[avatarNowMapIndex].nextIndex.Contains(this.NodeIndex);
            }

            if (isDunShu || isNearly || avatarNowMapIndex == this.NodeIndex)
            {
                // 遁术、相邻、或已在目标节点：单步直接移动
                this.AvatarMoveToThis();
            }
            else
            {
                // 非相邻节点：启动自动寻路协程，逐节点移动
                StartCoroutine(AutoMoveToNode(this.NodeIndex));
            }
        }

        public override void AvatarMoveToThis()
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool isDunShu = playerController.ShowType == MapPlayerShowType.遁术;
            int avatarNowMapIndex = getAvatarNowMapIndex();

            // MapMoveData 权限检查
            MapMoveData currentMapData = null;
            IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);

            if (!isDunShu && currentMapData != null && !currentMapData.canmove)
            {
                UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
                return;
            }

            // 单步物理移动
            AllMapManage.instance.mapIndex[avatarNowMapIndex].CloseLuDian();
            StartCoroutine(MoveSingleStep(avatarNowMapIndex, this.NodeIndex, playerController, isFinalDestination: true));
        }

        /// <summary>
        /// 单步移动协程：从 fromIndex 物理移动到 toIndex，沿 MapMoveNode 路径点
        /// </summary>
        /// <param name="isFinalDestination">是否为最终目的地（决定是否触发 showLuDian 和 OnArriveAtNode）</param>
        private System.Collections.IEnumerator MoveSingleStep(int fromIndex, int toIndex, MapPlayerController playerController, bool isFinalDestination)
        {
            AllMapManage.instance.isPlayMove = true;

            yield return StartCoroutine(MoveAlongMapMoveNodes(fromIndex, toIndex, playerController));

            setAvatarNowMapIndex(toIndex);
            playerController.SetSpeed(0);
            AllMapManage.instance.isPlayMove = false;

            if (isFinalDestination)
            {
                this.showLuDian();
                OnArriveAtNode(toIndex);
            }

            AllMapBase.RefreshMarksFromStaticData();

            if (Tools.getScreenName() != "S74000") this.BaseAddTime();
        }

        /// <summary>
        /// 自动寻路协程：计算从当前位置到目标节点的完整路径，逐节点移动
        /// 优先使用 MapGetWay 的 A* 寻路，失败时回退到 BFS
        /// </summary>
        private System.Collections.IEnumerator AutoMoveToNode(int targetIndex)
        {
            int startIndex = getAvatarNowMapIndex();

            // 优先使用 MapGetWay 的 A* 寻路，失败时回退到 BFS
            List<int> path = null;
            try { path = MapGetWay.Inst.GetBestList(startIndex, targetIndex); } catch { }
            if (path == null || path.Count == 0)
                path = FindPath(startIndex, targetIndex);

            if (path == null || path.Count == 0)
            {
                UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
                yield break;
            }

            // MapMoveData 权限检查：路径中每个节点都必须可达
            foreach (int nodeIndex in path)
            {
                MapMoveData data;
                if (IsToolsMain.MapMoveDatas.TryGetValue(nodeIndex, out data) && !data.canmove)
                {
                    UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
                    yield break;
                }
            }

            MapGetWay.Inst.IsStop = false;
            if (path.Count > 1)
                MapMoveTips.Show();

            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            int currentIndex = startIndex;

            for (int i = 0; i < path.Count; i++)
            {
                int nextIndex = path[i];
                bool isFinal = (i == path.Count - 1);

                yield return StartCoroutine(MoveSingleStep(currentIndex, nextIndex, playerController, isFinal));
                currentIndex = nextIndex;

                yield return new WaitForSeconds(0.3f);

                if (MapGetWay.Inst.IsStop || DialogAnalysis.IsRunningEvent)
                    break;
            }

            MapGetWay.Inst.StopAuToMove();
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

        // 路径查找方法 - 使用广度优先搜索（MapGetWay 寻路失败时的回退方案）
        private List<int> FindPath(int startIndex, int targetIndex)
        {
            if (startIndex == targetIndex) return new List<int>() { startIndex };

            HashSet<int> visited = new HashSet<int>();
            Queue<List<int>> queue = new Queue<List<int>>();

            queue.Enqueue(new List<int>() { startIndex });
            visited.Add(startIndex);

            while (queue.Count > 0)
            {
                List<int> currentPath = queue.Dequeue();
                int lastNode = currentPath.Last();

                if (lastNode == targetIndex)
                    return currentPath;

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

            return null;
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

        /// <summary>
        /// 静态方法：通过对话命令移动到指定节点
        /// </summary>
        public static void CommandMoveToNode(int targetNodeIndex)
        {
            if (!AllMapManage.instance.mapIndex.TryGetValue(targetNodeIndex, out var targetMapComponent))
            {
                UIPopTip.Inst.Pop($"无法找到节点 {targetNodeIndex} 的地图组件", PopTipIconType.叹号);
                return;
            }

            AllMapComponent allMapComponent = targetMapComponent as AllMapComponent;
            if (allMapComponent == null)
            {
                UIPopTip.Inst.Pop($"节点 {targetNodeIndex} 不是有效的可移动类型", PopTipIconType.叹号);
                return;
            }

            // 委托给实例的 EventRandom，它会自动判断遁术/相邻/寻路
            allMapComponent.EventRandom();
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

            // 如果该节点没有enter且配置了Data事件/Lua，直接触发
            else if (this.NoEnter && this.Data != null && !DialogAnalysis.IsRunningEvent)
            {
                if (!this.Data.Event.IsNullOrWhiteSpace())
                {
                    IsToolsMain.LogInfo($"到达路点 {nodeIndex}，触发Data.Event: {this.Data.Event}");
                    DialogAnalysis.StartDialogEvent(this.Data.Event, null);
                    return;
                }
                if (!this.Data.Lua.IsNullOrWhiteSpace())
                {
                    IsToolsMain.LogInfo($"到达路点 {nodeIndex}，触发Data.Lua: {this.Data.Lua}");
                    DialogAnalysis.StartTestDialogEvent(this.Data.Lua, null);
                    return;
                }
            }
        }
        #endregion
    }
}
