using System;
using System.Collections.Generic;
using GetWay;
using HarmonyLib;
using MaiJiu.MCS.HH.Data;
using MaiJiu.MCS.HH.Scene;
using MaiJiu.MCS.HH.Tool;
using Newtonsoft.Json;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using YSGame;


namespace top.Isteyft.MCS.JiuZhou.Scene
{
    public class AllMapBase : MonoBehaviour
    {
        // 地图实例
        public static AllMapBase inst;
        // 大地图数据
        public static AllMapJson MapData;
        // 地图对象
        public GameObject LevelsWorld0;


        // 当前地图名字
        private string sceneName;
        // 地图的起始索引
        public int startIndex;
        // 地图寻路组件的实例
        private MapGetWay mapGetWay;
        // 当前场景对应的 JSON 配置数据
        public JSONObject sceneJson;

        public static List<int> activeTasks = new List<int>();
        public static List<int> activeShijians = new List<int>();
        private void Awake()
        {
            // 设置单例实例
            //AllMapBase.Inst = this;
            //MaiJiu.MCS.HH.Scene.AllMapComponent;
            //MaiJiu.MCS.HH.Scene.AllMapBase;

            // 获取主摄像机并添加必要组件
            GameObject gameObject = base.transform.Find("/Main Camera").gameObject;
            gameObject.AddComponent<AllMapManage>();      // 地图管理组件，玩家加载
            gameObject.AddComponent<DialogProcess>();     // 对话处理组件
            //gameObject.AddComponent<CameraController>();  // 摄像机控制组件
            if (gameObject.GetComponent<CamaraFollow>() == null)
            {
                CamaraFollow camaraFollow = gameObject.AddComponent<CamaraFollow>();
                camaraFollow.levo = base.transform.Find("/dian1").GetComponent<Transform>();
                camaraFollow.desno = base.transform.Find("/dian2").GetComponent<Transform>();
            }
            // 获取大地图根节点并添加线路显示组件
            this.LevelsWorld0 = base.transform.Find("/AllMap/LevelsWorld0").gameObject;
            // 连线
            this.LevelsWorld0.AddComponent<AllMapLineShow>();
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
            this.initMap();
        }

        private void initMap()
        {
            sceneName = SceneManager.GetActiveScene().name;
            if (!MaiSaveData.Inst.allMapIndex.ContainsKey(sceneName))
            {
                MaiSaveData.Inst.allMapIndex[sceneName] = startIndex;
            }

            MaiSaveData.Inst.allMapIndex["AllMaps"] = PlayerEx.Player.NowMapIndex;
            // 将玩家当前的地图索引更新为当前场景对应的索引
            PlayerEx.Player.NowMapIndex = MaiSaveData.Inst.allMapIndex[sceneName];

            // 处理寻路系统
            if (MapGetWay.Inst != null)
            {
                // 创建一个新的 MapGetWay 实例
                mapGetWay = new MapGetWay();
                // 使用 Traverse 反射工具，强制将全局单例 _inst 字段替换为新的实例
                Traverse.Create(typeof(MapGetWay)).Field("_inst").SetValue(mapGetWay);
            }

            // 加载场景 JSON 配置
            // 尝试从全局 JSON 数据中获取当前场景的配置信息
            if (Jsondata.SceneJsonData.TryGetValue(sceneName, out var value))
            {
                sceneJson = value;
            }
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
                Transform transform = gameObject.transform.Find("flowchat/enter");
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
                
                // 查找task和shijian子物体
                Transform taskTransform = gameObject.transform.Find("Task");
                Transform shijianTransform = gameObject.transform.Find("shijian");

                if (taskTransform != null)
                {
                    // 为了该路点的task进行一个增加
                    allMapComponent.task = taskTransform.gameObject;
                    allMapComponent.SetTaskVisible(false); // 默认隐藏
                }

                if (shijianTransform != null)
                {
                    // 为了该路点的shijian进行一个增加
                    allMapComponent.shijian = shijianTransform.gameObject;
                    allMapComponent.SetShijianVisible(false); // 默认隐藏
                }
            }
        }

        private void OnDestroy()
        {
            // 1. 清理寻路单例
            Traverse.Create(typeof(MapGetWay)).Field("_inst").SetValue(null);
            // 2. 恢复地图索引
            PlayerEx.Player.NowMapIndex = MaiSaveData.Inst.allMapIndex["AllMaps"];
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
            base.Invoke("RefreshMarksFromStaticDataWrapper", 0.5f); // 延迟刷新标记
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
                Vector3 position = new Vector3(baseMapCompont.transform.position.x, baseMapCompont.transform.position.y - 0.2f, baseMapCompont.transform.position.z);
                AllMapManage.instance.MapPlayerController.transform.position = position;

                // 如果是AllMapComponent类型，激活入口点
                AllMapComponent allMapComponent = baseMapCompont as AllMapComponent;
                if (allMapComponent.enter != null)
                {
                    allMapComponent.enter.SetActive(true);
                }
            }
        }


        // 实例方法包装器，用于Invoke调用
        private void RefreshMarksFromStaticDataWrapper()
        {
            RefreshMarksFromStaticData();
        }
        // 刷新任务事件
        public static void RefreshMarksFromStaticData()
        {
            if (AllMapManage.instance == null || AllMapManage.instance.mapIndex == null)
            {
                IsToolsMain.Warning("AllMapManage.instance 或 mapIndex 为空，无法刷新标记");
                return;
            }

            IsToolsMain.LogInfo($"开始刷新标记 - activeTasks数量: {activeTasks.Count}, activeShijians数量: {activeShijians.Count}");
            // 遍历所有index
            foreach (var pair in AllMapManage.instance.mapIndex)
            {
                // 获取每个路点的value
                AllMapComponent component = pair.Value as AllMapComponent;
                if (component != null)
                {
                    // 获取是否有任务和事件元素
                    bool hasTask = activeTasks.Contains(pair.Key);
                    bool hasShijian = activeShijians.Contains(pair.Key);
                    
                    // 根据静态数组 activeTasks 和 activeShijians 设置显示状态
                    component.SetTaskVisible(hasTask);
                    component.SetShijianVisible(hasShijian);
                    
                    if (hasTask || hasShijian)
                    {
                        IsToolsMain.LogInfo($"节点 {pair.Key}: task={hasTask}(对象:{component.task != null}), shijian={hasShijian}(对象:{component.shijian != null})");
                    }
                }
            }
        }
        // 显示/隐藏所有路点的task标记
        public void SetAllTasksVisible(bool visible)
        {
            foreach (var mapComponent in AllMapManage.instance.mapIndex.Values)
            {
                AllMapComponent component = mapComponent as AllMapComponent;
                if (component != null)
                {
                    component.SetTaskVisible(visible);
                }
            }
        }

        // 显示/隐藏所有路点的shijian标记
        public void SetAllShijiansVisible(bool visible)
        {
            foreach (var mapComponent in AllMapManage.instance.mapIndex.Values)
            {
                AllMapComponent component = mapComponent as AllMapComponent;
                if (component != null)
                {
                    component.SetShijianVisible(visible);
                }
            }
        }

        public void ToggleShijianByIndex(int index)
        {
            if (AllMapManage.instance.mapIndex.TryGetValue(index, out var component))
            {
                AllMapComponent mapComponent = component as AllMapComponent;
                if (mapComponent != null)
                {
                    // 切换显示状态
                    bool newState = !mapComponent.showShijian;
                    mapComponent.SetShijianVisible(newState);

                    // 更新数组
                    if (newState)
                    {
                        if (!activeShijians.Contains(index))
                            activeShijians.Add(index);
                    }
                    else
                    {
                        activeShijians.Remove(index);
                    }
                }
            }
        }

        public void ToggleTaskByIndex(int index)
        {
            if (AllMapManage.instance.mapIndex.TryGetValue(index, out var component))
            {
                AllMapComponent mapComponent = component as AllMapComponent;
                if (mapComponent != null)
                {
                    // 切换显示状态
                    bool newState = !mapComponent.showTask;
                    mapComponent.SetTaskVisible(newState);

                    // 更新数组
                    if (newState)
                    {
                        if (!activeTasks.Contains(index))
                            activeTasks.Add(index);
                    }
                    else
                    {
                        activeTasks.Remove(index);
                    }
                }
            }
        }
        public bool IsShijianActive(int index)
        {
            return activeShijians.Contains(index);
        }

        public bool IsTaskActive(int index)
        {
            return activeTasks.Contains(index);
        }

        /// <summary>
        /// 地图事件刷新：清空旧事件，随机生成3个新事件（100-146之间，不与任务冲突）
        /// 适用于游戏结算后刷新事件，确保地图一直有3个事件
        /// </summary>
        public static void RefreshMapShijians()
        {
            // 清空所有旧的事件
            int oldCount = activeShijians.Count;
            activeShijians.Clear();
            IsToolsMain.LogInfo($"清空旧事件，原有数量: {oldCount}");
            
            // 生成新的3个事件ID
            List<int> newIds = GenerateRandomShijianIds();
            
            // 添加到activeShijians列表
            foreach (int id in newIds)
            {
                activeShijians.Add(id);
            }
            
            // 刷新地图标记显示
            if (AllMapManage.instance != null && AllMapManage.instance.mapIndex != null)
            {
                RefreshMarksFromStaticData();
            }
            
            IsToolsMain.LogInfo($"地图事件刷新完成，新事件ID: [{string.Join(", ", newIds)}]");
        }
        
        /// <summary>
        /// 随机生成三个事件ID，范围在100-146之间，不与任务冲突
        /// </summary>
        /// <returns>返回包含三个随机ID的列表</returns>
        private static List<int> GenerateRandomShijianIds()
        {
            List<int> result = new List<int>();
            System.Random random = new System.Random();
            int minId = 100;
            int maxId = 146;
            
            // 创建可用ID池（排除已激活的任务ID）
            List<int> availableIds = new List<int>();
            for (int i = minId; i <= maxId; i++)
            {
                if (!activeTasks.Contains(i))
                {
                    availableIds.Add(i);
                }
            }
            
            // 检查是否有足够的可用ID
            if (availableIds.Count < 3)
            {
                IsToolsMain.Warning($"可用的事件ID不足，当前可用数量: {availableIds.Count}");
                // 返回所有可用的ID
                return availableIds;
            }
            
            // 随机选择3个ID
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = random.Next(availableIds.Count);
                int selectedId = availableIds[randomIndex];
                result.Add(selectedId);
                availableIds.RemoveAt(randomIndex); // 移除已选择的ID，确保不重复
            }
            
            return result;
        }
    }
}