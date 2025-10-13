using System;
using System.Collections.Generic;
using MaiJiu.MCS.HH.Scene;
using Newtonsoft.Json;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using YSGame;


namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class AllMapBase : MonoBehaviour
    {
        // 地图实例
        public static AllMapBase Inst;
        // 大地图数据
        public static AllMapJson MapData;
        // 怪物
        public GameObject monster;
        // 怪物列表
        public Dictionary<int, AllMapNpcController> monsterlist;
        // 地图对象
        public GameObject LevelsWorld0;
        public static AllMapJson 幽州
        {
            get
            {
                if (AllMapBase.MapData == null)
                {
                    // 从场景JSON数据中获取幽州地图数据
                    JSONObject jsonobject = Jsondata.SceneJsonData["幽州"];
                    // 反序列化为AllMapJson对象
                    AllMapBase.MapData = JsonConvert.DeserializeObject<AllMapJson>(jsonobject.ToString());
                }
                return AllMapBase.MapData;
            }
        }

        public static Dictionary<int, List<int>> NPCLIST
        {
            get
            {
                Dictionary<int, List<int>> dictionary;
                // 尝试从NPC结算管理器中获取当前场景的NPC字典
                bool flag = NpcJieSuanManager.inst.npcMap.fuBenNPCDictionary.TryGetValue(Tools.getScreenName(), out dictionary);
                Dictionary<int, List<int>> result;
                if (flag)
                {
                    result = dictionary;
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }
        // 当前地图的npc
        public static List<int> NOWMAPNPCLIST
        {
            get
            {
                List<int> list = new List<int>();
                // 遍历所有NPC列表并合并
                foreach (List<int> collection in AllMapBase.NPCLIST.Values)
                {
                    list.AddRange(collection);
                }
                return list;
            }
        }
        private void Awake()
        {
            // 设置单例实例
            AllMapBase.Inst = this;

            // 获取主摄像机并添加必要组件
            GameObject gameObject = base.transform.Find("/Main Camera").gameObject;
            gameObject.AddComponent<AllMapManage>();      // 地图管理组件
            gameObject.AddComponent<DialogProcess>();     // 对话处理组件
            gameObject.AddComponent<CameraController>();  // 摄像机控制组件
            //if (gameObject.GetComponent<CamaraFollow>() == null)
            //{
            //    CamaraFollow camaraFollow = gameObject.AddComponent<CamaraFollow>();
            //    camaraFollow.levo = base.transform.Find("/dian1").GetComponent<Transform>();
            //    camaraFollow.desno = base.transform.Find("/dian2").GetComponent<Transform>();
            //}
            // 获取大地图根节点并添加线路显示组件
            this.LevelsWorld0 = base.transform.Find("/AllMap/LevelsWorld0").gameObject;
            this.LevelsWorld0.AddComponent<AllMapLineShow>();
            // 获取当前场景名称
            string nowSceneName = SceneEx.NowSceneName;
            // 如果地图数据未加载，尝试加载
            if (AllMapBase.MapData == null)
            {
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
            }
            this.InitIndex();
        }

        private void InitIndex()
        {
            // 遍历LevelsWorld0下的所有子物体
            for (int i = 0; i < this.LevelsWorld0.transform.childCount; i++)
            {
                // 获取当前子物体
                GameObject gameObject = this.LevelsWorld0.transform.GetChild(i).gameObject;
                // 解析路点名称作为key（假设名称是数字）
                int key = int.Parse(gameObject.name);
                // 添加AllMapComponent组件并注册到全局地图管理器
                AllMapComponent allMapComponent = gameObject.AddComponent<AllMapComponent>();
                AllMapManage.instance.mapIndex.Add(key, allMapComponent);
                // 添加点击组件
                gameObject.AddComponent<AllMapClick>();
                // 查找关卡中的"enter"子物体（入口点）
                Transform transform = gameObject.transform.Find("enter");
                // 尝试从地图数据中获取当前关卡的数据
                LudianJson value;
                // 检查地图数据是否已加载
                if (AllMapBase.MapData != null && AllMapBase.MapData.LuDian.TryGetValue(key, out value))
                {
                    // 设置组件数据
                    allMapComponent.Data = value;
                    // 如果该路点需要浮动效果
                    if (value.IsFloat)
                    {
                        // 在Level1Move子物体上添加浮动动画组件
                        GravityAnim gravityAnim = gameObject.transform.Find("Level1Move").gameObject.AddComponent<GravityAnim>();
                        gravityAnim.floatSpeed = value.Speed;
                    }
                    // 检查是否存在入口点
                    if (transform != null)
                    {
                        // 记录入口点对象
                        allMapComponent.enter = transform.gameObject;
                        // 添加按钮点击组件
                        BtnClick btnClick = allMapComponent.enter.AddComponent<BtnClick>();
                        // 如果有配置事件
                        if (value.Event != "")
                        {
                            // 绑定事件触发逻辑
                            btnClick.mouseUpEvent.AddListener(delegate ()
                            {
                                DialogAnalysis.StartDialogEvent(value.Event, null);
                            });
                        }
                        else
                        {
                            if (value.Lua != "")
                            {
                                // 绑定Lua脚本触发逻辑
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
        private void Start()
        {
            if (AllMapBase.MapData != null)
            {
                // 播放地图背景音乐
                MusicMag.instance.playMusic(AllMapBase.MapData.Music);
            }

            // 延迟设置玩家位置
            base.Invoke("SetPlayerIndex", 0.4f);
            // 延迟生成NPC
            base.Invoke("AutoSetNpcIndex", 1f);
        }
        public void SetPlayerIndex()
        {
            // 获取玩家在当前场景的路点索引
            int nowIndex = PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
            BaseMapCompont baseMapCompont;

            // 根据索引找到对应的路点
            if (AllMapManage.instance.mapIndex.TryGetValue(nowIndex, out baseMapCompont))
            {
                // 设置玩家位置（稍微向下偏移0.4个单位）
                Vector3 position = new Vector3(baseMapCompont.transform.position.x, baseMapCompont.transform.position.y - 0.4f, baseMapCompont.transform.position.z);
                AllMapManage.instance.MapPlayerController.transform.position = position;

                // 如果是AllMapComponent类型，激活入口点
                AllMapComponent allMapComponent = baseMapCompont as AllMapComponent;
                if (allMapComponent.enter != null)
                {
                    allMapComponent.enter.SetActive(true);
                }
            }
        }
        public void AutoSetNpcIndex()
        {
            bool flag = AllMapBase.NPCLIST == null;
            if (!flag)
            {
                // 遍历NPC字典（key为路点ID，value为NPC ID列表）
                this.monsterlist = new Dictionary<int, AllMapNpcController>();
                foreach (KeyValuePair<int, List<int>> keyValuePair in AllMapBase.NPCLIST)
                {
                    BaseMapCompont baseMapCompont;
                    // 根据路点ID找到对应路点
                    if (AllMapManage.instance.mapIndex.TryGetValue(keyValuePair.Key, out baseMapCompont))
                    {
                        // 获取路点位置
                        Vector3 position = baseMapCompont.gameObject.transform.position;
                        List<int> value = keyValuePair.Value;
                        // 为每个NPC ID创建怪物实例
                        for (int i = 0; i < value.Count; i++)
                        {
                            int npcID = value[i];
                            //this.CreateMonster(npcID, position, i);
                        }
                    }
                }
            }
        }
        private void UpdateNpcIndex()
        {
            Dictionary<int, AllMapNpcController> dictionary = new Dictionary<int, AllMapNpcController>();
            // 复制当前怪物列表
            foreach (KeyValuePair<int, AllMapNpcController> keyValuePair in this.monsterlist)
            {
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
            // 收集需要保留的NPC ID
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, AllMapNpcController> keyValuePair2 in dictionary)
            {
                if (!keyValuePair2.Value.CheckDestroy())
                {
                    list.Add(keyValuePair2.Key);
                }
            }
            // 获取当前NPC列表
            Dictionary<int, List<int>> npclist = AllMapBase.NPCLIST;
            // 更新NPC位置
            foreach (KeyValuePair<int, List<int>> keyValuePair3 in npclist)
            {
                List<int> value = keyValuePair3.Value;
                for (int i = 0; i < value.Count; i++)
                {
                    int num = value[i];
                    // 如果NPC已存在，更新其位置
                    if (list.Contains(num))
                    {
                        AllMapNpcController allMapNpcController = this.monsterlist[num];
                        allMapNpcController.SetNpcMove(keyValuePair3.Key);
                    }
                    else
                    {
                        // 否则创建新的NPC实例
                        BaseMapCompont baseMapCompont;
                        if (AllMapManage.instance.mapIndex.TryGetValue(keyValuePair3.Key, out baseMapCompont))
                        {
                            //this.CreateMonster(num, baseMapCompont.transform.position, i);
                            continue;
                        }
                    }
                }
            }
        }
        //private void CreateMonster(int npcID, Vector3 vector3, int index)
        //{
        //    // 实例化怪物预制体
        //    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.monster);
        //    gameObject.name = npcID.ToString();
        //    // 设置位置（根据index垂直偏移）
        //    gameObject.transform.position = new Vector3(vector3.x, vector3.y + (float)index * 0.4f, vector3.z);
        //    // 添加NPC控制器并添加到怪物列表
        //    AllMapNpcController value = gameObject.AddComponent<AllMapNpcController>();
        //    this.monsterlist.Add(npcID, value);
        //}
    }
}