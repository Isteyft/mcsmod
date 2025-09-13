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
                    JSONObject jsonobject = Jsondata.SceneJsonData["幽州"];
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
        public static List<int> NOWMAPNPCLIST
        {
            get
            {
                List<int> list = new List<int>();
                foreach (List<int> collection in AllMapBase.NPCLIST.Values)
                {
                    list.AddRange(collection);
                }
                return list;
            }
        }
        private void Awake()
        {
            AllMapBase.Inst = this;
            GameObject gameObject = base.transform.Find("/Main Camera").gameObject;
            gameObject.AddComponent<AllMapManage>();
            gameObject.AddComponent<DialogProcess>();
            gameObject.AddComponent<CameraController>();
            //if (gameObject.GetComponent<CamaraFollow>() == null)
            //{
            //    CamaraFollow camaraFollow = gameObject.AddComponent<CamaraFollow>();
            //    camaraFollow.levo = base.transform.Find("/dian1").GetComponent<Transform>();
            //    camaraFollow.desno = base.transform.Find("/dian2").GetComponent<Transform>();
            //}
            this.LevelsWorld0 = base.transform.Find("/AllMap/LevelsWorld0").gameObject;
            this.LevelsWorld0.AddComponent<AllMapLineShow>();
            string nowSceneName = SceneEx.NowSceneName;
            if (AllMapBase.MapData == null)
            {
                try
                {
                    JSONObject jsonobject = Jsondata.SceneJsonData[nowSceneName];
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
                // 音乐
                MusicMag.instance.playMusic(AllMapBase.MapData.Music);
            }
            base.Invoke("SetPlayerIndex", 0.4f);
            base.Invoke("AutoSetNpcIndex", 1f);
        }
        public void SetPlayerIndex()
        {
            int nowIndex = PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
            BaseMapCompont baseMapCompont;
            if (AllMapManage.instance.mapIndex.TryGetValue(nowIndex, out baseMapCompont))
            {
                Vector3 position = new Vector3(baseMapCompont.transform.position.x, baseMapCompont.transform.position.y - 0.4f, baseMapCompont.transform.position.z);
                AllMapManage.instance.MapPlayerController.transform.position = position;
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
                this.monsterlist = new Dictionary<int, AllMapNpcController>();
                foreach (KeyValuePair<int, List<int>> keyValuePair in AllMapBase.NPCLIST)
                {
                    BaseMapCompont baseMapCompont;
                    if (AllMapManage.instance.mapIndex.TryGetValue(keyValuePair.Key, out baseMapCompont))
                    {
                        Vector3 position = baseMapCompont.gameObject.transform.position;
                        List<int> value = keyValuePair.Value;
                        for (int i = 0; i < value.Count; i++)
                        {
                            int npcID = value[i];
                            this.CreateMonster(npcID, position, i);
                        }
                    }
                }
            }
        }
        private void UpdateNpcIndex()
        {
            Dictionary<int, AllMapNpcController> dictionary = new Dictionary<int, AllMapNpcController>();
            foreach (KeyValuePair<int, AllMapNpcController> keyValuePair in this.monsterlist)
            {
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, AllMapNpcController> keyValuePair2 in dictionary)
            {
                if (!keyValuePair2.Value.CheckDestroy())
                {
                    list.Add(keyValuePair2.Key);
                }
            }
            Dictionary<int, List<int>> npclist = AllMapBase.NPCLIST;
            foreach (KeyValuePair<int, List<int>> keyValuePair3 in npclist)
            {
                List<int> value = keyValuePair3.Value;
                for (int i = 0; i < value.Count; i++)
                {
                    int num = value[i];
                    if (list.Contains(num))
                    {
                        AllMapNpcController allMapNpcController = this.monsterlist[num];
                        allMapNpcController.SetNpcMove(keyValuePair3.Key);
                    }
                    else
                    {
                        BaseMapCompont baseMapCompont;
                        if (AllMapManage.instance.mapIndex.TryGetValue(keyValuePair3.Key, out baseMapCompont))
                        {
                            this.CreateMonster(num, baseMapCompont.transform.position, i);
                        }
                    }
                }
            }
        }
        private void CreateMonster(int npcID, Vector3 vector3, int index)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.monster);
            gameObject.name = npcID.ToString();
            gameObject.transform.position = new Vector3(vector3.x, vector3.y + (float)index * 0.4f, vector3.z);
            AllMapNpcController value = gameObject.AddComponent<AllMapNpcController>();
            this.monsterlist.Add(npcID, value);
        }
    }
}