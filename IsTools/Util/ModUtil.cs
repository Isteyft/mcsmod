using script.Steam;
using System.Linq;

namespace top.Isteyft.MCS.IsTools.Util
{
    // <summary>
    /// Mod 相关工具类
    /// </summary>
    public static class ModUtil
    {
        /// <summary>
        /// 验证当前 Mod 是否激活
        /// </summary>
        /// <param name="steamId">steam 工坊编号</param>
        /// <returns>验证结果</returns>
        public static bool CheckModActive(this ulong steamId) { return CheckModActive(steamId.ToString()); }

        /// <summary>
        /// 验证当前 Mod 是否激活
        /// </summary>
        /// <param name="steamId">steam 工坊编号</param>
        /// <returns>验证结果</returns>
        public static bool CheckModActive(this string steamId)
        {
            return WorkshopTool.GetAllModDirectory()
                .Where(directory => directory.Name.Equals(steamId))
                .Any(_ => !WorkshopTool.CheckModIsDisable(steamId));
        }

        /// <summary>
        /// 获取指定工坊编号的 Mod 信息
        /// </summary>
        /// <param name="steamId">Mod 工坊编号</param>
        /// <param name="modInfo">Mod 信息</param>
        /// <returns>获取结果</returns>
        public static bool TryGetModInfo(this ulong steamId, out ModInfo modInfo)
        {
            modInfo = default;

            if (WorkShopMag.Inst != null && WorkShopMag.Inst.ModInfoDict.TryGetValue(steamId, out var value))
            {
                IsToolsMain.Log($"已查询的有效的 Mod 信息: {steamId}");
                modInfo = value;
                return true;
            }
            IsToolsMain.Warning($"未能查询的有效的 Mod 信息: {steamId}");
            return false;
        }

        /// <summary>
        /// 获取指定工坊编号的 Mod 名称信息
        /// </summary>
        /// <param name="steamId">Mod 工坊编号</param>
        /// <returns>Mod 名称</returns>
        public static string GetModName(this ulong steamId)
        {
            if (WorkShopMag.Inst != null && WorkShopMag.Inst.ModInfoDict.TryGetValue(steamId, out var modInfo))
            {
                IsToolsMain.Log($"已查询的有效的 Mod 名称: {steamId} - {modInfo.Name}");
                return modInfo.Name;
            }
            IsToolsMain.Log($"未能查询的有效的 Mod 名称: {steamId}");
            return steamId.ToString();
        }

        /// <summary>
        /// 获取指定工坊编号的 Mod 完整路径
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        public static string GetModPath(this string steamId)
        {
            return $"{WorkshopTool.GetModDirectory(steamId)?.FullName}/plugins";
        }

        ///   <summary>  
        ///   返回大于等于min且小于max的随机数
        ///   </summary>
        public static int GetRandom(int min, int max)
        {
            return new System.Random().Next(min, max);
        }


        // --- 这里是手写的 XxHash32 算法核心代码 ---
        private static uint XXH32(byte[] input, int seed = 0)
        {
            if (input == null || input.Length == 0)
                return 0;

            const uint prime32 = 0x1000193; // 808530493
            const uint init32 = 0x9E3779B1; // 2654435761

            uint length = (uint)input.Length;
            uint hash = (uint)(init32 + length + seed);

            int i = 0;
            // 处理 4 字节块
            while (i <= input.Length - 4)
            {
                uint k = (uint)(input[i] | input[i + 1] << 8 | input[i + 2] << 16 | input[i + 3] << 24);
                k *= prime32;
                k = RotateLeft(k, 13);
                k *= prime32;
                hash ^= k;
                hash = RotateLeft(hash, 17);
                hash = hash * prime32 + prime32;
                i += 4;
            }

            // 处理剩余字节
            switch (length % 4)
            {
                case 3: hash ^= (uint)(input[i + 2] << 16); goto case 1;
                case 2: hash ^= (uint)(input[i + 1] << 8); goto case 1;
                case 1: hash ^= input[i]; hash *= prime32; break;
            }

            hash ^= hash >> 15;
            hash *= prime32;
            hash ^= hash >> 17;

            return hash;
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        /// <summary>
        /// 使用哈希映射法获取固定随机数结果（防SL）
        /// </summary>
        /// <param name="seed">开局生成的固定种子</param>
        /// <param name="count">当前已经触发的次数（第几次）</param>
        /// <param name="prv">成功率（0-100的整数，例如1代表1%）</param>
        /// <returns>是否成功触发</returns>
        public static bool GetSeedRandom(int seed, int count, int prv)
        {
            // 1. 拼接输入数据
            string uniqueInput = $"{seed}_{count}_{prv}";
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(uniqueInput);

            // 2. 使用手写的 XXH32 进行哈希运算
            uint hashValue = XXH32(inputBytes, 0);

            // 3. 映射到 0-99
            int result = (int)(hashValue % 100);

            // 4. 判定
            return result < prv;
        }
    }
}