// using Fungus;
// using HarmonyLib;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using top.Isteyft.MCS.IsTools.Data;
// using UnityEngine;
// using UnityEngine.UI;
// using YSGame.Fight;

// namespace top.Isteyft.MCS.IsTools.Patch
// {
//     [HarmonyPatch(typeof(RoundManager), "initAvatarInfo")]
//     public class AddDaoJuPatch
//     {
//         private static readonly KeyCode[] HomeRowKeys = {
//             KeyCode.None, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
//             KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
//             KeyCode.Z, KeyCode.X, KeyCode.C
//         };

//         private static GameObject _daoJuPanel; // 缓存道具栏面板，防止重复创建
//         [HarmonyPostfix]
//         public static void PostfixMethod(KBEngine.Avatar avatar)
//         {
//             if (!avatar.isPlayer()) return;

//             //var fightType = Tools.instance?.monstarMag?.FightType;
//             //if (fightType != StartFight.FightEnumType.无装备无丹药擂台 &&
//             //    fightType != StartFight.FightEnumType.天劫秘术领悟)
//             //{
//             //    CleanUp(); // 可选：不在这些模式下时隐藏/销毁
//             //    return;
//             //}

//             // 收集需要显示的道具（玩家已拥有的道具）
//             var ownedDaoJus = new List<DaoJuData>();
//             foreach (var daoJu in DaoJuData.data)
//             {
//                 if (avatar.hasItem(daoJu.DaoJuItem))
//                 {
//                     IsToolsMain.LogInfo($"拥有{daoJu.DaoJuItem},加载技能{daoJu.DaoJuSkill}");
//                     ownedDaoJus.Add(daoJu);
//                 }
//             }

//             if (ownedDaoJus.Count == 0)
//             {
//                 return;
//             }

//             var uiFightPanel = UIFightPanel.Inst;
//             if (uiFightPanel == null) return;

//             // 获取正确的UI父节点：UIFightCanvas/UIFight/Scale/
//             Transform parentTransform = null;
            
//             // 尝试找到UIFightCanvas
//             GameObject uiFightCanvas = GameObject.Find("UIFightCanvas");
//             if (uiFightCanvas != null)
//             {
//                 // 找到UIFightCanvas下的UIFight
//                 Transform uiFight = uiFightCanvas.transform.Find("UIFight");
//                 if (uiFight != null)
//                 {
//                     Transform uiFightScale = uiFight.Find("Scale");
//                     // 找到UIFight下的Scale
//                     if (uiFightScale != null)
//                     {
//                         parentTransform = uiFightScale.Find("Main");
//                     }
//                 }
//             }
//             if (parentTransform == null)
//             {
//                 IsToolsMain.Error("未找到战斗UI，错误");
//             }
//             // 创建新的道具栏容器
//             _daoJuPanel = new GameObject("DaoJuSkillPanel");
//             _daoJuPanel.transform.SetParent(parentTransform, false);
//             _daoJuPanel.AddComponent<RectTransform>();

            

//             var panelRect = _daoJuPanel.GetComponent<RectTransform>();
//             // 设置锚点和pivot为左下角，确保位置计算正确
//             panelRect.anchorMin = new Vector2(0, 0);
//             panelRect.anchorMax = new Vector2(0, 0);
//             panelRect.pivot = new Vector2(0, 0);
            
//             // 计算面板大小，支持两排显示
//             int maxCols = 6; // 每行最多6个
//             int rows = (int)Math.Ceiling((float)ownedDaoJus.Count / maxCols); // 计算行数
//             float width = Math.Min(ownedDaoJus.Count, maxCols) * 80f; // 宽度根据列数计算
//             float height = rows * 80f; // 高度根据行数计算，每行80个单位
            
//             panelRect.sizeDelta = new Vector2(width, height); // 设置面板大小
//             panelRect.anchoredPosition = new Vector2(370, 250); // 使用anchoredPosition设置位置
//             // 使用第一个技能按钮作为模板
//             if (uiFightPanel.FightSkills == null || uiFightPanel.FightSkills.Count == 0) return;
//             GameObject template = uiFightPanel.FightSkills[0].gameObject;

//             for (int i = 0; i < ownedDaoJus.Count; i++)
//             {
//                 DaoJuData daoJu = ownedDaoJus[i];
                
//                 // 实例化按钮
//                 GameObject btn = UnityEngine.Object.Instantiate(template, _daoJuPanel.transform, false);
//                 var item = btn.GetComponent<UIFightSkillItem>();
//                 item.Clear();

//                 // 设置技能
//                 var skill = new GUIPackage.Skill(daoJu.DaoJuSkill, 0, 10);
//                 avatar.skill.Add(skill);
//                 item.SetSkill(skill); 
//                 //UIFightPanel.Inst.FightSkills[i + 6].SetSkill(skill);

//                 // 设置热键
//                 KeyCode hotKey = (i + 1 <= 12) ? HomeRowKeys[i + 1] : KeyCode.None;
//                 item.HotKey = hotKey;

//                 // 获取hotKeyText组件
//                 var hotKeyText = btn.transform.Find("Slot/LeftUpMask/HotKey")?.GetComponent<Text>();
//                 if (hotKeyText != null)
//                 {
//                     // 设置热键文本
//                     hotKeyText.text = hotKey != KeyCode.None ? hotKey.ToString() : "";
                    
//                     // 复制hotKeyText的样式到新的数量显示组件
//                     Transform slotTransform = btn.transform.Find("Slot");
//                     if (slotTransform != null)
//                     {
//                         // 创建新的数量显示组件
//                         GameObject countTextObj = new GameObject("CountText");
//                         countTextObj.transform.SetParent(slotTransform, false);
//                         Text countText = countTextObj.AddComponent<Text>();
                        
//                         // 复制hotKeyText的所有样式属性
//                         countText.font = hotKeyText.font;
//                         countText.fontSize = hotKeyText.fontSize;
//                         countText.color = new Color(0, 0, 0, 1);
//                         countText.alignment = hotKeyText.alignment;
//                         countText.fontStyle = hotKeyText.fontStyle;
//                         countText.lineSpacing = hotKeyText.lineSpacing;
//                         countText.horizontalOverflow = hotKeyText.horizontalOverflow;
//                         countText.verticalOverflow = hotKeyText.verticalOverflow;
                        
                        
//                         // 设置文本内容为道具数量
//                         countText.text = $"{avatar.getItemNum(daoJu.DaoJuItem)}";
                        
//                         // 设置位置
//                         RectTransform countRect = countTextObj.GetComponent<RectTransform>();
//                         if (countRect != null)
//                         {
//                             countRect.sizeDelta = hotKeyText.GetComponent<RectTransform>().sizeDelta;
//                             countRect.localPosition = new Vector3(-45, -50, 0);
//                         }
//                     }
//                 }

//                 // 设置按钮名称为FightSkillItem_按键格式
//                 btn.name = "FightSkillItem_" + (hotKey != KeyCode.None ? hotKey.ToString() : (i + 1).ToString());

//                 // 布局：两排显示，超过6个时往上排
//                 var btnRect = btn.GetComponent<RectTransform>();
//                 int row = i / 6; // 计算行数，每6个一行
//                 int col = i % 6; // 计算列数
//                 float xPos = col * 80f;
//                 float yPos = row * 80f; // 每行往上移动80个单位（因为面板pivot在左下角）
//                 btnRect.anchoredPosition = new Vector2(xPos, yPos);
//             }

//             // 调整Main组件下的SkillTip位置，根据道具行数每行增加85高度
//             if (rows > 0) // 只有当有道具行时才调整
//             {
//                 Transform mainTransform = parentTransform; // parentTransform就是Main组件
//                 Transform skillTipTransform = mainTransform.Find("SkillTip");
//                 if (skillTipTransform != null)
//                 {
//                     Vector3 localPoint = skillTipTransform.localPosition;
//                     float newY = localPoint.y + (rows * 85f); // 每行增加85高度
//                     skillTipTransform.localPosition = new Vector3(localPoint.x, newY, localPoint.z);
//                 }
//             }
//         }

//     }
// }
