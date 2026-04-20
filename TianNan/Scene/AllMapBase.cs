using MaiJiu.MCS.HH.Scene;
using Newtonsoft.Json;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.TianNan.Scene
{
    // 大地图管理
    public class AllMapBase : MonoBehaviour
    {
        // 地图实例
        public static AllMapBase Inst;
        // 大地图数据
        public static AllMapJson MapData;
        // 怪物
        public GameObject monster;
        // 地图对象
        public GameObject LevelsWorld0;

        /**
         * 获取埋久工具库读取的地图数据
         */
        public static AllMapJson 天南
        {
            get
            {
                if (AllMapBase.MapData == null)
                {
                    // 从场景JSON数据中获取幽州地图数据
                    JSONObject jsonobject = Jsondata.SceneJsonData["天南"];
                    // 反序列化为AllMapJson对象
                    AllMapBase.MapData = JsonConvert.DeserializeObject<AllMapJson>(jsonobject.ToString());
                }
                return AllMapBase.MapData;
            }
        }

        /**
         * 初始化大地图
         */
        private void Awake()
        {
            AllMapBase.Inst = this;

            // 获取主摄像机并添加必要组件
            GameObject gameObject = base.transform.Find("/Main Camera").gameObject;
            gameObject.AddComponent<AllMapManage>();      // 增加地图管理组件
            gameObject.AddComponent<DialogProcess>();     // 增加对话处理组件
            // 如果摄像机的跟随不存在，挂载
            CamaraFollow camaraFollow = gameObject.AddComponent<CamaraFollow>();
            camaraFollow.levo = base.transform.Find("/dian1").GetComponent<Transform>();
            camaraFollow.desno = base.transform.Find("/dian2").GetComponent<Transform>();
            // 获取大地图根节点并添加线路显示组件
            this.LevelsWorld0 = base.transform.Find("/AllMap/LevelsWorld0").gameObject;
            // 获取当前场景名称
            string nowSceneName = SceneEx.NowSceneName;
            try
            {
                // 从场景JSON数据中获取当前场景数据
                JSONObject jsonobject = Jsondata.SceneJsonData[nowSceneName];
                // 反序列化为AllMapJson对象
                AllMapBase.MapData = JsonConvert.DeserializeObject<AllMapJson>(jsonobject.ToString());
            }
            catch
            {
                UIPopTip.Inst.Pop("找不到场景数据，停止加载场景！", PopTipIconType.叹号);
            }
            this.InitIndex();
        }

        /**
         * 初始化每个路点
         */
        private void InitIndex()
        {
            // 遍历LevelsWorld0下的所有子物体
            for (int i = 0; i < this.LevelsWorld0.transform.childCount; i++)
            {
                // 获取当前子物体
                GameObject gameObject = this.LevelsWorld0.transform.GetChild(i).gameObject;
                // 解析路点名称作为key
                int key = int.Parse(gameObject.name);

                // 添加AllMapComponent组件并注册到全局地图管理器
                AllMapComponent allMapComponent = gameObject.AddComponent<AllMapComponent>();
                AllMapManage.instance.mapIndex.Add(key, allMapComponent);

                gameObject.AddComponent<AllMapClick>();
                Transform transform = gameObject.transform.Find("enter");

                // 尝试从地图数据中获取当前关卡的数据
                LudianJson value;
                // 检查地图数据是否已加载
                if (AllMapBase.MapData != null && AllMapBase.MapData.LuDian.TryGetValue(key, out value))
                {
                    allMapComponent.Data = value;
                    // 如果有进入，给他绑定事件
                    if (transform != null)
                    {
                        allMapComponent.enter = transform.gameObject;
                        BtnClick btnClick = allMapComponent.enter.AddComponent<BtnClick>();
                        // 绑定next事件
                        if (value.Event != "")
                        {
                            btnClick.mouseUpEvent.AddListener(delegate ()
                            {
                                DialogAnalysis.StartDialogEvent(value.Event, null);
                            });
                        }
                        else
                        {
                            // 绑定next指令
                            if (value.Lua != "")
                            {
                                btnClick.mouseUpEvent.AddListener(delegate ()
                                {
                                    DialogAnalysis.StartTestDialogEvent(value.Lua, null);
                                });
                            }
                        }
                    }
                    else
                    {
                        // 标记路点没有进入
                        allMapComponent.NoEnter = true;
                    }
                }
            }
        }

        /**
         * 播放音乐和设置玩家位置
         */
        private void Start()
        {
            if (AllMapBase.MapData != null)
            {
                // 播放地图背景音乐
                MusicMag.instance.playMusic(AllMapBase.MapData.Music);
            }

            base.Invoke("SetPlayerIndex", 0.4f);
        }

        /**
         * 设置玩家位置
         */
        public void SetPlayerIndex()
        {
            // 获取玩家在当前场景的路点索引
            int nowIndex = PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
            BaseMapCompont baseMapCompont;

            // 根据索引找到对应的路点
            if (AllMapManage.instance.mapIndex.TryGetValue(nowIndex, out baseMapCompont))
            {
                // 设置玩家位置（稍微向下偏移0.4个单位）
                Vector3 position = new Vector3(baseMapCompont.transform.position.x, baseMapCompont.transform.position.y - 0.2f, baseMapCompont.transform.position.z);
                AllMapManage.instance.MapPlayerController.transform.position = position;

                // 如果是地图路点对象，激活入口点
                AllMapComponent allMapComponent = baseMapCompont as AllMapComponent;
                if (allMapComponent.enter != null)
                {
                    allMapComponent.enter.SetActive(true);
                }
            }
        }

    }
}
