using System.Collections.Generic;

namespace top.Isteyft.MCS.JiuZhou.Scene.walkMap
{
    /// <summary>
    /// WalkMap模式开关配置。
    /// 只在这个文件里决定哪些场景走新模式，其他代码不做场景硬编码。
    /// </summary>
    public static class WalkMapModeConfig
    {
        /// <summary>
        /// 启用WalkMap模式的场景名列表。
        /// 可按需增删，例如: "F幽州", "F雪剑域"。
        /// </summary>
        private static readonly HashSet<string> EnabledSceneNames = new HashSet<string>
        {
            "F雪剑域"
        };

        public static bool IsWalkMapScene(string sceneName)
        {
            return !string.IsNullOrEmpty(sceneName) && EnabledSceneNames.Contains(sceneName);
        }
    }
}
