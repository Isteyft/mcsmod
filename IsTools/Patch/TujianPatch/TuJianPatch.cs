using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSGame.TuJian;
using MaiJiu.MCS.Pet.TuJian;
using UnityEngine;
using MaiJiu.MCS.Pet;
using Tab;
using System.Reflection;

namespace top.Isteyft.MCS.IsTools.Patch.TujianPatch
{
    [HarmonyPatch(typeof(TuJianItemTab))]
    public class TuJianPatch
    {
        // Token: 0x06000016 RID: 22 RVA: 0x00002BB4 File Offset: 0x00000DB4
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Postfix(TuJianItemTab __instance)
        {
            AchievementPatch(__instance);
            CGPatch(__instance);
        }

        public static void AchievementPatch(TuJianItemTab __instance)
        {
            __instance.TypeSSV.DataList.Add(new Dictionary<int, string>
            {
                {
                    11,
                    "成就"
                }
            });
            Dictionary<int, InfoPanelBase> panelDict = Traverse.Create(__instance).Field("PanelDict").GetValue<Dictionary<int, InfoPanelBase>>();

            AchievementInfoPanel achievementPanel = copyAchievementTujian(__instance);
            // 4. 判空保护 (防止因为找不到组件导致游戏报错崩溃)
            if (achievementPanel != null && panelDict != null)
            {
                // 5. 添加到字典中 (ID 11 对应上面的逻辑)
                if (!panelDict.ContainsKey(11))
                {
                    panelDict.Add(11, achievementPanel);
                }
                else
                {
                    // 如果已经存在，可以选择覆盖
                    panelDict[11] = achievementPanel;
                }
            }
            else
            {
                // 调试用：如果在控制台看到这条，说明你的预制体里没挂 AchievementInfoPanel 组件
                IsToolsMain.Warning("出错了，未找到成就图鉴页面");
            }
        }

        public static AchievementInfoPanel copyAchievementTujian(TuJianItemTab __instance)
        {
            IsToolsMain.LogInfo("复制矿石作为成就基底");
            // 1. 直接从 __instance 往下找，不再依赖 _go 字段
            Transform infoRoot = __instance.transform.Find("Root/InfoRoot");

            if (infoRoot == null)
            {
                IsToolsMain.Warning("未找到 Root/InfoRoot，请检查层级！");
                return null;
            }

            // 2. 找原版模板
            Transform template = infoRoot.Find("KuangShiInfo");

            if (template == null)
            {
                IsToolsMain.Warning("未找到 KuangShiInfo，请检查层级！");
                return null;
            }

            // 3. 复制
            Transform transform2 = UnityEngine.Object.Instantiate(template, infoRoot);
            transform2.name = "AchievementInfoPanel";
            transform2.gameObject.AddComponent<AchievementInfoPanel>();
            var oldScript = transform2.GetComponent<KuangShiInfoPanel>();
            UnityEngine.Object.DestroyImmediate(oldScript);

            return transform2.GetComponent<AchievementInfoPanel>();
        }


        public static void CGPatch(TuJianItemTab __instance)
        {
            __instance.TypeSSV.DataList.Add(new Dictionary<int, string>
            {
                {
                    99,
                    "CG鉴赏"
                }
            });
            Dictionary<int, InfoPanelBase> panelDict = Traverse.Create(__instance).Field("PanelDict").GetValue<Dictionary<int, InfoPanelBase>>();

            CGPanel cgPanel = copyCGTujian(__instance);
            // 4. 判空保护 (防止因为找不到组件导致游戏报错崩溃)
            if (cgPanel != null && panelDict != null)
            {
                // 5. 添加到字典中 (ID 99 对应上面的逻辑)
                if (!panelDict.ContainsKey(99))
                {
                    panelDict.Add(99, cgPanel);
                }
                else
                {
                    // 如果已经存在，可以选择覆盖
                    panelDict[99] = cgPanel;
                }
            }
            else
            {
                // 调试用：如果在控制台看到这条，说明你的预制体里没挂 cgPanel 组件
                IsToolsMain.Warning("出错了，未找到CG图鉴页面");
            }
        }

        public static CGPanel copyCGTujian(TuJianItemTab __instance)
        {
            IsToolsMain.LogInfo("复制矿石作为成就基底");
            // 1. 直接从 __instance 往下找，不再依赖 _go 字段
            Transform infoRoot = __instance.transform.Find("Root/InfoRoot");

            if (infoRoot == null)
            {
                IsToolsMain.Warning("未找到 Root/InfoRoot，请检查层级！");
                return null;
            }

            // 2. 找原版模板
            Transform template = infoRoot.Find("KuangShiInfo");

            if (template == null)
            {
                IsToolsMain.Warning("未找到 KuangShiInfo，请检查层级！");
                return null;
            }

            // 3. 复制
            Transform transform2 = UnityEngine.Object.Instantiate(template, infoRoot);
            transform2.name = "CGPanel";
            transform2.gameObject.AddComponent<CGPanel>();
            var oldScript = transform2.GetComponent<KuangShiInfoPanel>();
            UnityEngine.Object.DestroyImmediate(oldScript);

            Transform CG = transform2.Find("ItemBG");
            if (CG != null)
            {
                CG.transform.localPosition = new Vector3(-55f, 59f, 0);
                CG.localScale = new Vector3(7.5f, 4.5f, 0);
                Transform QualityBg = CG.Find("QualityBg");
                // 为图片添加简单的展开处理器
                QualityBg.gameObject.AddComponent<CGSimpleExpandHandler>();
            }

            Transform text = transform2.Find("HyTextSV");
            if (text != null)
            {
                text.transform.localPosition = new Vector3(200f, -515f, 0);
            }

            return transform2.GetComponent<CGPanel>();
        }

    }
}
