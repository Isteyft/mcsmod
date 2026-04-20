using GetWay;
using HarmonyLib;
using MaiJiu.MCS.HH.Data;
using MaiJiu.MCS.HH.Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaiJiu.MCS.HH.Scene
{
    public class AllMapBase : MonoBehaviour
    {
        // 单例实例，用于在其他地方访问当前地图的基础数据
        public static AllMapBase inst;

        // 当前激活的场景名称
        private string sceneName;

        // 地图的起始索引（通常在编辑器中设置）
        public int startIndex;

        // 地图寻路组件的实例
        private MapGetWay mapGetWay;

        // 当前场景对应的 JSON 配置数据
        public JSONObject sceneJson;

        // 当脚本实例被加载时调用
        private void Awake()
        {
            // 1. 初始化单例
            inst = this;
            
            // 2. 获取并记录当前场景名称
            sceneName = SceneManager.GetActiveScene().name;

            // 3. 管理地图索引数据
            // 如果存档数据中还没有当前场景的索引记录，则使用本组件设置的 startIndex 进行初始化
            if (!MaiSaveData.Inst.allMapIndex.ContainsKey(sceneName))
            {
                MaiSaveData.Inst.allMapIndex[sceneName] = startIndex;
            }

            // 4. 切换地图索引上下文
            // 先将玩家当前的地图索引备份到 "AllMaps" 键下（作为临时存档）
            MaiSaveData.Inst.allMapIndex["AllMaps"] = PlayerEx.Player.NowMapIndex;
            // 然后将玩家当前的地图索引更新为当前场景对应的索引
            PlayerEx.Player.NowMapIndex = MaiSaveData.Inst.allMapIndex[sceneName];

            // 5. 处理寻路系统 (MapGetWay)
            // 检查全局的 MapGetWay.Inst 是否已存在（可能是持久化对象）
            if (MapGetWay.Inst != null)
            {
                // 创建一个新的 MapGetWay 实例
                mapGetWay = new MapGetWay();
                // 【关键点】使用 Harmony 的 Traverse 反射工具，强制将全局单例 _inst 字段替换为新的实例
                // 这通常用于在场景切换时重置或更新寻路数据
                Traverse.Create(typeof(MapGetWay)).Field("_inst").SetValue(mapGetWay);
            }

            // 6. 加载场景 JSON 配置
            // 尝试从全局 JSON 数据中获取当前场景的配置信息
            if (Jsondata.SceneJsonData.TryGetValue(sceneName, out var value))
            {
                sceneJson = value;
            }
        }

        // 当脚本实例被销毁时调用（例如切换场景）
        private void OnDestroy()
        {
            // 1. 清理寻路单例
            // 使用反射将 MapGetWay 的全局实例 (_inst) 设为 null，防止内存泄漏或引用旧场景数据
            Traverse.Create(typeof(MapGetWay)).Field("_inst").SetValue(null);

            // 2. 恢复地图索引
            // 将玩家的当前地图索引恢复为进入此场景前备份的值（即 "AllMaps" 中存储的值）
            PlayerEx.Player.NowMapIndex = MaiSaveData.Inst.allMapIndex["AllMaps"];
        }
    }
}