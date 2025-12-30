//using HarmonyLib;
//using JSONClass;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine.Events;
//using UnityEngine;
//using UnityEngine.UI;

//namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
//{



//    [HarmonyPatch(typeof(UIBiGuanXiuLianPanel))]
//    public class UIBiGuanXiuLianPanelPatch
//    {
//        [HarmonyPatch("CalcShuangXiu")]
//        [HarmonyPrefix]
//        public static bool CalcShuangXiu(int biGuanTime)
//        {
//            KBEngine.Avatar player = PlayerEx.Player;
//            if (player.ShuangXiuData.HasField("JingYuan"))
//            {
//                JSONObject jsonobject = player.ShuangXiuData["JingYuan"];
//                int num = jsonobject["Count"].I;
//                ShuangXiuMiShu shuangXiuMiShu = ShuangXiuMiShu.DataDict[jsonobject["Skill"].I];
//                if (shuangXiuMiShu.ningliantype != 5) return true;
//                int i = jsonobject["PinJie"].I;
//                int i2 = jsonobject["Reward"].I;
//                int npcid = 0;
//                bool flag = false;
//                if (jsonobject.HasField("DaoLvID"))
//                {
//                    npcid = jsonobject["DaoLvID"].I;
//                    flag = !NPCEx.IsDeath(npcid);
//                }
//                num -= biGuanTime * ShuangXiuLianHuaSuDu.DataDict[i].speed;
//                if (num <= 0)
//                {
//                    if (shuangXiuMiShu.ningliantype == 1)
//                    {
//                        player.addEXP(i2);
//                        if (flag)
//                        {
//                            NPCEx.AddJsonInt(npcid, "exp", i2);
//                        }
//                    }
//                    else if (shuangXiuMiShu.ningliantype == 2)
//                    {
//                        player.xinjin += i2;
//                    }
//                    else if (shuangXiuMiShu.ningliantype == 3)
//                    {
//                        player.addShenShi(i2);
//                        if (flag)
//                        {
//                            NPCEx.AddJsonInt(npcid, "shengShi", i2);
//                        }
//                    }
//                    else if (shuangXiuMiShu.ningliantype == 4)
//                    {
//                        player._HP_Max += i2;
//                        if (flag)
//                        {
//                            NPCEx.AddJsonInt(npcid, "HP", i2);
//                        }
//                    }
//                    else if (shuangXiuMiShu.ningliantype == 5)
//                    {
//                        player._dunSu += i2;
//                        if (flag)
//                        {
//                            NPCEx.AddJsonInt(npcid, "dunSu", i2);
//                        }
//                    }
//                    player.ShuangXiuData.RemoveField("JingYuan");
//                    return false;
//                }
//                player.ShuangXiuData["JingYuan"].SetField("Count", num);
//                return false;
//            }
//            return false;
//        }

//        public static string[] ningliantypes = new string[]
//        {
//                "修为",
//                "心境",
//                "神识",
//                "血量上限",
//                "遁速",
//                "悟性",
//                "资质",
//        };

//        //private static FieldInfo BiGuanTime = AccessTools.Field(typeof(UIBiGuanXiuLianPanel), "BiGuanTime");
//        // 获取 BiGuanTime 属性的 set 方法
//        private static MethodInfo setBiGuanTimeMethod = AccessTools.PropertySetter(typeof(UIBiGuanXiuLianPanel), "BiGuanTime");
//        private static readonly MethodInfo getZiZhiStrMethod = AccessTools.Method(
//            typeof(UIBiGuanXiuLianPanel),
//            "GetZiZhiStr",
//            new[] { typeof(int) } // 参数类型列表
//        );
//        [HarmonyPatch("RefreshSpeedUI")]
//        [HarmonyPrefix]
//        public static bool RefreshSpeedUI(UIBiGuanXiuLianPanel __instance)
//        {
//            KBEngine.Avatar player = Tools.instance.getPlayer();
//            int staticID = player.getStaticID();
//            JSONObject jsonobject = jsonData.instance.StaticSkillJsonData[staticID.ToString()];//功法速度
//            if (staticID == 0)
//            {
//                __instance.GongFaName.text = "无";
//                __instance.GongFaSpeed.text = "0/月";
//            }
//            else
//            {
//                __instance.GongFaName.text = "《" + Tools.instance.getStaticSkillName(staticID, false) + "》";
//                __instance.GongFaSpeed.text = string.Format("{0}", jsonobject["Skill_Speed"].I);
//            }
//            if (SceneEx.NowSceneName == "S101") //地脉一类的洞府修炼速度
//            {
//                DongFuData dongFuData = new DongFuData(DongFuManager.NowDongFuID);
//                dongFuData.Load();
//                int xiuliansudu = DFLingYanLevel.DataDict[dongFuData.LingYanLevel].xiuliansudu;
//                int xiuliansudu2 = DFZhenYanLevel.DataDict[dongFuData.JuLingZhenLevel].xiuliansudu;
//                __instance.DiMaiName.text = dongFuData.DongFuName;
//                __instance.DiMaiSpeed.text = string.Format("{0}%", xiuliansudu + xiuliansudu2);
//            }
//            else
//            {
//                __instance.DiMaiName.text = jsonData.instance.BiguanJsonData[UIBiGuanPanel.Inst.BiGuanType.ToString()]["Text"].Str;
//                __instance.DiMaiSpeed.text = string.Format("{0}%", jsonData.instance.BiguanJsonData[UIBiGuanPanel.Inst.BiGuanType.ToString()]["speed"].I);
//            }//心境
//            __instance.XinJingName.text = jsonData.instance.XinJinJsonData[player.GetXinJingLevel().ToString()]["Text"].Str;
//            int num = jsonData.instance.XinJinGuanLianJsonData[player.getXinJinGuanlianType().ToString()]["speed"].I - 100;
//            if (num >= 0)
//            {
//                __instance.XinJingChange.text = "提升";
//                __instance.XinJingSpeed.text = string.Format("{0}%", num);
//            }
//            else
//            {
//                __instance.XinJingChange.text = "降低";
//                __instance.XinJingSpeed.text = string.Format("{0}%", -num);
//            }//资质相关的
//            //__instance.ZiZhiName.text = getZiZhiStrMethod(player.ZiZhi);
//            string ziZhiStr = (string)getZiZhiStrMethod.Invoke(null, new object[] { player.ZiZhi });
//            __instance.ZiZhiName.text = ziZhiStr;
//            __instance.ZiZhiSpeed.text = string.Format("{0}", (int)player.AddZiZhiSpeed((float)jsonobject["Skill_Speed"].I));
//            float timeExpSpeed = player.getTimeExpSpeed();
//            Debug.Log(string.Format("基础修炼速度:{0}", timeExpSpeed));
//            float biguanSpeed = UIBiGuanXiuLianPanel.GetBiguanSpeed(true, UIBiGuanPanel.Inst.BiGuanType, "");
//            int num2 = (int)(timeExpSpeed + biguanSpeed);
//            Debug.Log(string.Format("总速度:{0}", num2));
//            __instance.TotalSpeed.text = num2.ToString();
//            __instance.TimeSlider.maxValue = (float)__instance.GetBiGuanMaxTime();
//            if (__instance.TimeSlider.maxValue == 0f)
//            {
//                __instance.TimeSlider.minValue = 0f;
//                __instance.TimeSlider.value = 0f;
//                //BiGuanTime.SetValue(__instance, 0);
//                setBiGuanTimeMethod.Invoke(__instance, new object[] { 0 });
//            }
//            else
//            {
//                __instance.TimeSlider.minValue = 1f;
//                __instance.TimeSlider.value = 1f;
//                //BiGuanTime.SetValue(__instance, 1);
//                setBiGuanTimeMethod.Invoke(__instance, new object[] { 1 });
//            }
//            __instance.TimeSlider.onValueChanged.RemoveAllListeners();
//            __instance.TimeSlider.onValueChanged.AddListener(new UnityAction<float>(__instance.OnTimeSliderValueChanged));
//            if (player.ShuangXiuData.HasField("JingYuan"))
//            {
//                JSONObject jsonobject2 = player.ShuangXiuData["JingYuan"];
//                ShuangXiuMiShu shuangXiuMiShu = ShuangXiuMiShu.DataDict[jsonobject2["Skill"].I];
//                int num3 = jsonobject2["Count"].I / ShuangXiuLianHuaSuDu.DataDict[jsonobject2["PinJie"].I].speed;
//                if (jsonobject2["Count"].I % ShuangXiuLianHuaSuDu.DataDict[jsonobject2["PinJie"].I].speed != 0)
//                {
//                    num3++;
//                }
//                __instance.JingYuanTimeText.text = string.Format("闭关{0}年{1}月后", num3 / 12, num3 % 12);
//                __instance.JingYuanDescText.text = string.Format("可将精元凝练为{0}{1}", jsonobject2["Reward"].I, ningliantypes[shuangXiuMiShu.ningliantype - 1]);
//                return false;
//            }
//            __instance.JingYuanTimeText.text = "空";
//            __instance.JingYuanDescText.text = "";
//            return false;
//        }
//    }


//    [HarmonyPatch(typeof(UINPCShuangXiuSelect))]
//    public class UINPCShuangXiuSelectPatch
//    {
//        [HarmonyPatch("AddSkillItem")]
//        [HarmonyPrefix]
//        public static bool AddSkillItem(ShuangXiuMiShu skill, UINPCShuangXiuSelect __instance)
//        {
//            if (skill.id != 7) return true;
//            UnityEngine.GameObject gameObject = UnityEngine.Object.Instantiate<UnityEngine.GameObject>(__instance.ShuangXiuSkillPrefab, __instance.ContentRT);
//            gameObject.GetComponentInChildren<Text>().text = skill.name;
//            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = IsMoDaoKuoZhanMain.I.UIManagerHandle.spriteBank["ShuangXiuMiShuIcon7.png"];
//            gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
//            {
//                // 获取私有字段
//                FieldInfo field = typeof(UINPCShuangXiuSelect).GetField("selectedSkillID",
//                    BindingFlags.NonPublic | BindingFlags.Instance);
//                field?.SetValue(__instance, skill.id); // 安全设置值
//                __instance.MiShuDescText.text = skill.desc;
//            });
//            return false;
//        }
//    }

//    [HarmonyPatch(typeof(UINPCShuangXiuAnim))]
//    public class UINPCShuangXiuAnimPatch
//    {
//        private static FieldInfo npcField = AccessTools.Field(typeof(UINPCShuangXiuAnim), "npc");
//        private static FieldInfo needPlayField = AccessTools.Field(typeof(UINPCShuangXiuAnim), "needPlay");
//        [HarmonyPatch("RefreshUI")]
//        [HarmonyPrefix]
//        public static bool RefreshUI(UINPCShuangXiuAnim __instance)
//        {
//            IsMoDaoKuoZhanMain.LogInfo("显示双修秘法");
//            // 获取当前交互的 NPC
//            var nowJiaoHuNPC = UINPCJiaoHu.Inst.NowJiaoHuNPC;
//            npcField.SetValue(__instance, nowJiaoHuNPC);
//            nowJiaoHuNPC.RefreshData();
//            __instance.VideoImage.FallbackSprites.Clear();
//            __instance.VideoImage.FallbackSprites.Add(__instance.FallbackAnim);
//            __instance.VideoImage.GroupName = "ShuangXiu";
//            __instance.VideoImage.OnPlayFinshed.RemoveAllListeners();
//            string resultTip;
//            if (PlayerEx.Player.ShuangXiuData.HasField("JingYuan"))
//            {
//                JSONObject jsonobject = PlayerEx.Player.ShuangXiuData["JingYuan"];
//                ShuangXiuMiShu shuangXiuMiShu = ShuangXiuMiShu.DataDict[jsonobject["Skill"].I];
//                __instance.VideoImage.TargetFileName = shuangXiuMiShu.name;
//                int num = jsonobject["Count"].I / ShuangXiuLianHuaSuDu.DataDict[jsonobject["PinJie"].I].speed;
//                if (jsonobject["Count"].I % ShuangXiuLianHuaSuDu.DataDict[jsonobject["PinJie"].I].speed != 0)
//                {
//                    num++;
//                }
//                resultTip = string.Format("获得精元{0}\n闭关{1}年{2}月后，可将精元凝练为{3}{4}", new object[]
//                {
//                jsonobject["Count"].I,
//                num / 12,
//                num % 12,
//                jsonobject["Reward"].I,
//                UIBiGuanXiuLianPanelPatch.ningliantypes[shuangXiuMiShu.ningliantype - 1]
//                });
//            }
//            else
//            {
//                __instance.VideoImage.TargetFileName = ShuangXiuMiShu.DataDict[1].name;
//                resultTip = "获得精元0";
//            }
//            __instance.VideoImage.OnPlayFinshed.AddListener(delegate ()
//            {
//                UINPCJiaoHu.Inst.HideNPCShuangXiuAnim();
//                ResManager.inst.LoadPrefab("UIShuangXiuResultPanel").Inst(null).GetComponent<UIShuangXiuResultPanel>().Show(resultTip, null);
//            });
//            needPlayField.SetValue(__instance, true);
//            return false;
//        }
//    }

//}
