using BepInEx;
using DebuggingEssentials;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using YSGame;

namespace top.Isteyft.MCS.TianNan.Scene
{
    // 地图路点对象
    public class AllMapComponent : MapComponent
    {
        private float MoveBaseSpeedMin = 1.5f;// 最小基础移动速度
        private float MoveBaseSpeed = 4f;// 基础移动速度
        public UnityEngine.GameObject enter;// 路点游戏对象
        public bool NoEnter = false;// 隐藏进入
        public LudianJson Data;// 路点数据

        /**
         * 从玩家副本数据中获取当前地图索引
         */
        public override int getAvatarNowMapIndex()
        {
            return PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
        }

        /**
         * 判断这个路点有没有进入键，默认隐藏掉进入键
         */
        public override void CloseLuDian()
        {
            UnityEngine.GameObject gameObject = this.enter;
            if (gameObject != null)
            {
                gameObject.SetActive(false);// 隐藏路点
            }
        }
        /**
         * 显示路点
         */
        public override void showLuDian()
        {
            // 如果有进入键就加载
            if (this.enter != null)
            {
                this.enter.SetActive(true);
            }
            else
            {
                // 存在路点数据 和 不隐藏进入 和 next没有事件进行
                if (this.Data != null && this.NoEnter && !DialogAnalysis.IsRunningEvent)
                {
                    // 检查是否有事件
                    if (!this.Data.Event.IsNullOrWhiteSpace())
                    {
                        // 进行next事件
                        DialogAnalysis.StartDialogEvent(this.Data.Event, null);
                    }
                    else
                    {
                        // 检查是否有Next指令要运行
                        if (!this.Data.Lua.IsNullOrWhiteSpace())
                        {
                            // 运行指令(一般为Lua)
                            DialogAnalysis.StartTestDialogEvent(this.Data.Lua, null);
                        }
                    }
                }
            }
        }

        /**
         * 设置玩家当前坐标
         */
        public override void setAvatarNowMapIndex()
        {
            // 保存上一个地图索引
            Tools.instance.fubenLastIndex = this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex;
            // 更新当前地图索引为这个节点
            this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex = this.NodeIndex;
        }
        
        /**
         * 设置玩家当前坐标，需要指定的坐标
         */
        public void setAvatarNowMapIndex(int nodeIndex)
        {
            // 保存上一个地图索引
            Tools.instance.fubenLastIndex = this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex;
            // 更新当前地图索引为指定的节点
            this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex = nodeIndex;
        }

        /**
         * 判断玩家是否可以点击
         * 当玩家正在移动，或者玩家当前就在这个节点，或者有事件正在运行时，不能点击
         */
        public new bool CanClick()
        {
            return AllMapManage.instance.isPlayMove || this.getAvatarNowMapIndex() == this.NodeIndex || DialogAnalysis.IsRunningEvent;
        }

        /**
         * 用遁术前往指定路点
         */
        public override void AvatarMoveToThis()
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            if (playerController.ShowType != MapPlayerShowType.遁术)
            {
                UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
            }
            else
            {
                KBEngine.Avatar player = PlayerEx.Player;
                BaseMapCompont baseMapCompont = AllMapManage.instance.mapIndex[this.NodeIndex];
                int avatarNowMapIndex = this.getAvatarNowMapIndex();
                BaseMapCompont Nowcomp = AllMapManage.instance.mapIndex[avatarNowMapIndex];
                AllMapManage.instance.isPlayMove = true;
                // 计算遁术
                float num = Vector2.Distance(Nowcomp.transform.position, baseMapCompont.transform.position);
                float num2 = (player.dunSu > 200) ? ((this.MoveBaseSpeedMin + this.MoveBaseSpeed) * 2f) : (this.MoveBaseSpeedMin + this.MoveBaseSpeed * ((float)player.dunSu / 100f));
                num2 *= 2f;
                float num3 = num / num2;
                // 创建动作队列
                Queue<UnityAction> queue = new Queue<UnityAction>();
                UnityAction item = delegate ()
                {
                    Nowcomp.CloseLuDian();
                    playerController.SetSpeed(1);
                    iTween.MoveTo(playerController.gameObject, iTween.Hash(new object[]
                    {
                        "x",
                        this.transform.position.x,
                        "y",
                        this.transform.position.y - 0.2f,
                        "z",
                        playerController.transform.position.z,
                        "time",
                        num3,
                        "islocal",
                        false,
                        "EaseType",
                        "linear"
                    }));
                    WASDMove.waitTime = num3;
                    WASDMove.needWait = true;
                    this.Invoke("callContinue", num3);
                };
                queue.Enqueue(item);
                YSFuncList.Ints.AddFunc(queue);

                // 移动完成后的处理
                Queue<UnityAction> queue2 = new Queue<UnityAction>();
                UnityAction item2 = delegate ()
                {
                    this.setAvatarNowMapIndex();
                    playerController.SetSpeed(0);
                    AllMapManage.instance.isPlayMove = false;
                    this.showLuDian();
                    YSFuncList.Ints.Continue();
                };
                queue2.Enqueue(item2);
                YSFuncList.Ints.AddFunc(queue2);
            }
        }

        /**
         * 增加事件和移动当前路点
         */
        public override void EventRandom()
        {
            // 移动
            this.AvatarMoveToThis();
            // 增加时间
            this.BaseAddTime();
        }
    }
}
