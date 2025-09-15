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
        public new bool CanClick()
        {
            // 当玩家正在移动，或者玩家当前就在这个节点，或者有事件正在运行时，不能点击
            return AllMapManage.instance.isPlayMove || this.getAvatarNowMapIndex() == this.NodeIndex || DialogAnalysis.IsRunningEvent;
        }
        public override void AvatarMoveToThis()
        {
            // 获取地图玩家控制器实例
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool flag = playerController.ShowType != MapPlayerShowType.遁术;
            if (flag)
            {
                UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
            }
            else
            {
                KBEngine.Avatar player = PlayerEx.Player;
                // 获取目标地图组件
                BaseMapCompont baseMapCompont = AllMapManage.instance.mapIndex[this.NodeIndex];
                // 获取玩家当前所在位置的地图索引
                int avatarNowMapIndex = this.getAvatarNowMapIndex();
                // 获取玩家当前所在位置的地图组件
                BaseMapCompont Nowcomp = AllMapManage.instance.mapIndex[avatarNowMapIndex];
                // 设置移动状态为true，表示玩家正在移动
                AllMapManage.instance.isPlayMove = true;
                // 计算当前位置到目标位置的距离
                float num = Vector2.Distance(Nowcomp.transform.position, baseMapCompont.transform.position);
                // 计算移动速度：
                // 如果玩家遁术值大于200，则使用最大速度
                // 否则根据遁术值按比例计算速度
                float num2 = (player.dunSu > 200) ? ((this.MoveBaseSpeedMin + this.MoveBaseSpeed) * 2f) : (this.MoveBaseSpeedMin + this.MoveBaseSpeed * ((float)player.dunSu / 100f));
                // 速度乘以2（可能是为了调整移动时间）
                num2 *= 2f;
                // 计算移动所需时间 = 距离 / 速度
                float num3 = num / num2;
                // 创建动作队列
                Queue<UnityAction> queue = new Queue<UnityAction>();
                // 第一个动作：开始移动
                UnityAction item = delegate ()
                {
                    // 关闭当前地点的路点标记
                    Nowcomp.CloseLuDian();
                    // 设置玩家移动速度为1
                    playerController.SetSpeed(1);
                    iTween.MoveTo(playerController.gameObject, iTween.Hash(new object[]
                    {
                        "x",
                        this.transform.position.x,// 目标x坐标
                        "y",
                        this.transform.position.y - 0.2f, // 目标y坐标（稍微向下偏移0.2）
                        "z",
                        playerController.transform.position.z,// z坐标保持不变
                        "time",
                        num3,// 移动时间
                        "islocal",
                        false, // 不使用局部坐标
                        "EaseType",
                        "linear"// 线性移动
                    }));
                    // 设置WASD移动的等待时间和需要等待标志
                    WASDMove.waitTime = num3;
                    WASDMove.needWait = true;
                    // 在移动完成后调用callContinue方法
                    this.Invoke("callContinue", num3);
                };
                queue.Enqueue(item);
                YSFuncList.Ints.AddFunc(queue);
                // 创建第二个动作队列
                Queue<UnityAction> queue2 = new Queue<UnityAction>();
                // 第二个动作：移动完成后的处理
                UnityAction item2 = delegate ()
                {
                    // 更新玩家当前地图索引
                    this.setAvatarNowMapIndex();
                    // 停止玩家移动
                    playerController.SetSpeed(0);
                    // 设置移动状态为false
                    AllMapManage.instance.isPlayMove = false;
                    // 显示目标地点的路点标记
                    this.showLuDian();
                    // 继续执行后续动作
                    YSFuncList.Ints.Continue();
                };
                queue2.Enqueue(item2);
                YSFuncList.Ints.AddFunc(queue2);
            }
        }
        public override void EventRandom()
        {
            // 移动到这个节点
            this.AvatarMoveToThis();
            // 增加时间（可能是游戏内时间）
            this.BaseAddTime();
        }
    }
}
