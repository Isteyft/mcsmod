//using System;
//using BepInEx;
//using BepInEx.Configuration;
//using Fungus;
//using HarmonyLib;
//using KBEngine;

//namespace MCS_MOD_Assassin
//{
//    /// <summary>
//    /// 主插件类，继承自BaseUnityPlugin
//    /// </summary>
//    [BepInPlugin("mcs.wfcpc.plugin.Assassin", "Assassin", "1.0.0")]
//    public class Assassin : BaseUnityPlugin
//    {
//        // 插件启动时初始化配置
//        private void Start()
//        {
//            // 绑定配置项：
//            // killPrestige - 是否在击杀NPC时保留声望
//            this.killPrestige = base.Config.Bind<bool>("config", "killPrestige", true, null);
//            // NPCLimit - NPC数量限制
//            this.NPCLimit = base.Config.Bind<int>("config", "NPCLimit", 1, null);
//            // sectPrestige - 是否保留门派好感度
//            this.sectPrestige = base.Config.Bind<bool>("config", "sectPrestige", false, null);
//            // requestPrestige - 是否在请求时保留声望
//            this.requestPrestige = base.Config.Bind<bool>("config", "requestPrestige", false, null);

//            // 应用Harmony补丁
//            Harmony harmony = new Harmony("mcs.wfcpc.plugin.HelloMod");
//            harmony.PatchAll();
//        }

//        // 空Update方法
//        private void Update()
//        {
//        }

//        // Awake方法，设置单例实例
//        private void Awake()
//        {
//            Assassin.Instance = this;
//        }

//        // 单例实例
//        public static Assassin Instance;

//        // 配置项：击杀NPC时是否保留声望
//        private ConfigEntry<bool> killPrestige;

//        // 配置项：NPC数量限制
//        private ConfigEntry<int> NPCLimit;

//        // 配置项：是否保留门派好感度
//        private ConfigEntry<bool> sectPrestige;

//        // 配置项：请求时是否保留声望
//        private ConfigEntry<bool> requestPrestige;

//        // NPC计数
//        private int NPCCount = 99;

//        /// <summary>
//        /// PlayerEx类的补丁，用于修改声望系统
//        /// </summary>
//        [HarmonyPatch(typeof(PlayerEx))]
//        private class PlayerEx_Patch
//        {
//            /// <summary>
//            /// 拦截添加声望的方法，总是返回true以阻止原方法执行
//            /// </summary>
//            [HarmonyPrefix]
//            [HarmonyPatch("AddShengWang")]
//            [HarmonyPatch(new Type[]
//            {
//                typeof(int),
//                typeof(int),
//                typeof(bool)
//            })]
//            public static bool PlayerEx_AddShengWang_Patch(int id, int add, bool show)
//            {
//                return true;
//            }
//        }

//        /// <summary>
//        /// Avatar类的补丁，用于修改门派好感度系统
//        /// </summary>
//        [HarmonyPatch(typeof(Avatar))]
//        private class Avater_Patch
//        {
//            /// <summary>
//            /// 修改门派好感度设置方法
//            /// 如果配置启用且值为负，则将其设为0
//            /// </summary>
//            [HarmonyPrefix]
//            [HarmonyPatch("SetMenPaiHaoGandu")]
//            public static bool Avatar_SetMenPaiHaoGandu_Patch(int MenPaiID, ref int Value)
//            {
//                bool flag = Value < 0 && Assassin.Instance.sectPrestige.Value;
//                if (flag)
//                {
//                    Value = 0;
//                }
//                return true;
//            }
//        }

//        /// <summary>
//        /// NPCEx类的补丁，用于修改请求声望系统
//        /// </summary>
//        [HarmonyPatch(typeof(NPCEx))]
//        private class NPCEx_Patch
//        {
//            /// <summary>
//            /// 修改请求声望变化方法
//            /// 如果配置启用且声望为负，则将其设为0
//            /// </summary>
//            [HarmonyPrefix]
//            [HarmonyPatch("SuoQuChangeShengWang")]
//            public static bool Avatar_SuoQuChangeShengWang_Patch(ref int shengWang)
//            {
//                bool flag = shengWang < 0 && Assassin.Instance.requestPrestige.Value;
//                if (flag)
//                {
//                    shengWang = 0;
//                }
//                return true;
//            }
//        }

//        /// <summary>
//        /// setMenPaiHaoGanDu类的补丁，用于修改门派好感度设置
//        /// </summary>
//        [HarmonyPatch(typeof(setMenPaiHaoGanDu))]
//        private class setMenPaiHaoGanDu_Patch
//        {
//            /// <summary>
//            /// 修改门派好感度设置入口方法
//            /// 如果配置启用、值为负且NPC数量未达限制，则将其设为0
//            /// </summary>
//            [HarmonyPrefix]
//            [HarmonyPatch("OnEnter")]
//            public static bool setMenPaiHaoGanDu_OnEnter_Patch(setMenPaiHaoGanDu __instance)
//            {
//                int value = Traverse.Create(__instance).Field("Value").GetValue<int>();
//                bool flag = value < 0 && Assassin.Instance.killPrestige.Value && Assassin.Instance.NPCCount < Assassin.Instance.NPCLimit.Value;
//                if (flag)
//                {
//                    Traverse.Create(__instance).Field("Value").SetValue(0);
//                }
//                return true;
//            }
//        }

//        /// <summary>
//        /// UINPCLeftList类的补丁，用于更新NPC计数
//        /// </summary>
//        [HarmonyPatch(typeof(UINPCLeftList))]
//        private class UINPCLeftList_Patch
//        {
//            /// <summary>
//            /// 在刷新NPC列表后更新NPC计数
//            /// </summary>
//            [HarmonyPostfix]
//            [HarmonyPatch("RefreshNPC")]
//            public static void UINPCLeftList_RefreshNPC_Patch(UINPCLeftList __instance)
//            {
//                int nowShowedNPCCount = __instance.NowShowedNPCCount;
//                Assassin.Instance.NPCCount = nowShowedNPCCount;
//            }
//        }
//    }
//}