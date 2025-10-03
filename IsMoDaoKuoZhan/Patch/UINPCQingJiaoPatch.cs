using HarmonyLib;
using JSONClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    [HarmonyPatch(typeof(UINPCQingJiao))]
    public class UINPCQingJiaoPatch
    {
            private static Dictionary<int, int> FavorDict = new Dictionary<int, int>()
            {
                { 1, 5 },
                { 2, 6 },
                { 3, 8 }
            };
            [HarmonyPostfix]
            [HarmonyPatch("GongFaSlotAction")]
            public static void PostfixGongFaSlotAction(bool isShiFu, int qingJiaoType, int pinJie, JSONObject skill)
            {
                _ItemJsonData QingJiaoBook;
                UINPCData npc = UINPCJiaoHu.Inst.NowJiaoHuNPC;
                IsMoDaoKuoZhanMain.Log("请教功法");
                IsMoDaoKuoZhanMain.Log(skill.ToString());
                //确保是请教魔道结束的
                if (qingJiaoType != 6 || isShiFu)
                    return;
                UINPCJiaoHu.Inst.IsQingJiaoShiBaiSW = false;
                if (npc.FavorLevel < FavorDict[pinJie])
                {
                    Tools.Say("我们的感情还没这么好吧？", npc.ID);
                    return;
                }
                int qingFenCost = NPCEx.GetQingFenCost(skill, isGongFa: true);
                if (npc.QingFen < qingFenCost)
                {
                    Tools.Say("我像是这么慷慨的人吗？", npc.ID);
                    //UINPCJiaoHu.Inst.IsQingJiaoShiBaiQF = true;
                    return;
                }

                if (npc.IsNingZhouNPC && PlayerEx.GetNingZhouShengWangLevel() > 1)
                {
                    Tools.Say("魔道功法可不会教给宁州的好人。", npc.ID);
                    return;
                }
                if (!npc.IsNingZhouNPC && PlayerEx.GetSeaShengWangLevel() > 1)
                {
                    Tools.Say("魔道功法可不会教给无尽之海的好人。", npc.ID);
                    return;
                }

                UINPCJiaoHu.Inst.HideNPCQingJiaoPanel();
                UINPCJiaoHu.Inst.HideJiaoHuPop();

                //记录请教的物品
                int SkillID = skill["Skill_ID"].I;
                QingJiaoBook = _ItemJsonData.DataList.FirstOrDefault(data => data.type == 4 && (int)float.Parse(data.desc) == SkillID);

                if (QingJiaoBook == null)
                {
                    Tools.Say("{punch=10,1}这个功法我也不太懂，还是算了吧。", npc.ID);
                    return;
                }
                NPCEx.AddQingFen(npc.ID, -qingFenCost);
                PlayerEx.Player.addItem(QingJiaoBook.id, 1, null, ShowText: true);
                Tools.Say($"既然如此，便教你《{QingJiaoBook.name}》", npc.ID);
        }

            [HarmonyPostfix]
            [HarmonyPatch("ShenTongSlotAction")]
            public static void ShenTongSlotAction_Postfix(bool isShiFu, int qingJiaoType, int pinJie, JSONObject skill)
            {
                _ItemJsonData QingJiaoBook;
                UINPCData npc = UINPCJiaoHu.Inst.NowJiaoHuNPC;
                IsMoDaoKuoZhanMain.Log("请教神通");
                IsMoDaoKuoZhanMain.Log(skill.ToString());
                //确保是请教魔道结束的
                if (qingJiaoType != 6 || isShiFu)
                    return;
                UINPCJiaoHu.Inst.IsQingJiaoShiBaiSW = false;
                if (npc.FavorLevel < FavorDict[pinJie])
                {
                    Tools.Say("我们的感情还没这么好吧？", npc.ID);
                    return;
                }
                int qingFenCost = NPCEx.GetQingFenCost(skill, isGongFa: false);
                if (npc.QingFen < qingFenCost)
                {
                    Tools.Say("我像是这么慷慨的人吗？", npc.ID);
                    //UINPCJiaoHu.Inst.IsQingJiaoShiBaiQF = true;
                    return;
                }

                if (npc.IsNingZhouNPC && PlayerEx.GetNingZhouShengWangLevel() > 1)
                {
                    Tools.Say("魔道神通可不会教给好人。", npc.ID);
                    return;
                }
                if (!npc.IsNingZhouNPC && PlayerEx.GetSeaShengWangLevel() > 1)
                {
                    Tools.Say("魔道神通可不会教给好人。", npc.ID);
                    return;
                }
                UINPCJiaoHu.Inst.HideNPCQingJiaoPanel();
                UINPCJiaoHu.Inst.HideJiaoHuPop();

                //记录请教的物品
                int SkillID = skill["Skill_ID"].I;
                QingJiaoBook = _ItemJsonData.DataList.FirstOrDefault(data => data.type == 3 && (int)float.Parse(data.desc) == SkillID);

                if (QingJiaoBook == null)
                {
                    Tools.Say("{punch=10,1}这个神通我也不太懂，还是算了吧。", npc.ID);
                    return;
                }
                NPCEx.AddQingFen(npc.ID, -qingFenCost);
                IsMoDaoKuoZhanMain.Log(QingJiaoBook.id + QingJiaoBook.name + QingJiaoBook.desc);
                Tools.Say($"既然如此，便教你《{QingJiaoBook.name}》", npc.ID);
                PlayerEx.Player.addItem(QingJiaoBook.id, 1, null, ShowText: true);
        }
        }
}
