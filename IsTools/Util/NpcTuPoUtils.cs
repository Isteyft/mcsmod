using HarmonyLib;
using script.EventMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Util
{
    public static class NpcTuPoUtils
    {
        //NPCTuPo
        public static Dictionary<int, int> itemTuPoLvDictionary = new Dictionary<int, int>();
        public static Dictionary<int, int> itemTuPoFenShuDictionary = new Dictionary<int, int>();
        public static Dictionary<int, int> itemTuPoUseNumDictionary = new Dictionary<int, int>();
        //突破筑基
        public static void TuPoZhuJi(int npcId, int npcBigTuPoFenShu = 100)
        {
            npcId = NPCEx.NPCIDToNew(npcId);
            if (NPCEx.IsDeath(npcId))
            {
                IsToolsMain.Log($"{npcId}已经死亡");
            }
            IsToolsMain.Log("进入结算逻辑");
            JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
            _ = npcData["Level"].I;
            NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, npcBigTuPoFenShu);
            npcData.SetField("exp", 0);
            UpDateNpcData(npcId, 4);
            NpcJieSuanManager.inst.npcNoteBook.NoteZhuJiSuccess(npcId);
        }

        //突破金丹
        public static void TuPoJinDan(int npcId, int jdlevel, int npcBigTuPoFenShu = 10)
        {
            npcId = NPCEx.NPCIDToNew(npcId);
            if (NPCEx.IsDeath(npcId))
            {
                IsToolsMain.Log($"{npcId}已经死亡");
            }
            IsToolsMain.Log("进入结算逻辑");
            // 获取NPC数据
            JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
            //IsMoXinXiangJuanMain.Log("进入结算更新");
            int nineGradeId = GetJinDanId(npcId, jdlevel);
            JSONObject nineGradeData = jsonData.instance.JieDanBiao[nineGradeId.ToString()];

            JSONObject jinDanData = new JSONObject();
            jinDanData.SetField("JinDanId", nineGradeId);
            jinDanData.SetField("JinDanLv", jdlevel);
            jinDanData.SetField("JinDanAddSpeed", nineGradeData["EXP"].I);
            jinDanData.SetField("JinDanAddHp", nineGradeData["HP"].I);

            // 更新NPC数据
            npcData.SetField("JinDanData", jinDanData);
            NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, nineGradeData["HP"].I);
            npcData.SetField("exp", 0);
            UpDateNpcData(npcId, 7);

            // 记录突破成功事件
            NpcJieSuanManager.inst.npcNoteBook.NoteJinDanSuccess(npcId, jdlevel);
            EventMag.Inst.SaveEvent(npcId, 1);
        }
        public static void TuPoYuanYing(int npcId, int npcBigTuPoFenShu = 200)
        {
            npcId = NPCEx.NPCIDToNew(npcId);
            if (NPCEx.IsDeath(npcId))
            {
                IsToolsMain.Log($"{npcId}已经死亡");
            }
            IsToolsMain.Log($"{npcId}进入结算逻辑");
            JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
            _ = npcData["Level"].I;

            if (npcData.HasField("AddToPoLv"))
            {
                npcData.RemoveField("AddToPoLv");
            }

            if (npcData.HasField("JinDanData"))
            {
                npcData["JinDanData"].SetField("JinDanAddSpeed", npcData["JinDanData"]["JinDanAddSpeed"].I * 2);
                NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, npcData["JinDanData"]["JinDanAddHp"].I);
            }
            NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, npcBigTuPoFenShu * 10);
            NpcJieSuanManager.inst.npcNoteBook.NoteYuanYingSuccess(npcId);
            npcData.SetField("exp", 0);
            UpDateNpcData(npcId, 10);
            EventMag.Inst.SaveEvent(npcId, 2);
        }

        public static void TuPoHuaShen(int npcId, int npcBigTuPoFenShu = 25)
        {
            npcId = NPCEx.NPCIDToNew(npcId);
            if (NPCEx.IsDeath(npcId))
            {
                IsToolsMain.Log($"{npcId}已经死亡");
            }
            IsToolsMain.Log("进入结算逻辑");
            JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
            _ = npcData["Level"].I;

            if (npcData.HasField("AddToPoLv"))
            {
                npcData.RemoveField("AddToPoLv");
            }

            NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, npcBigTuPoFenShu * 1200);
            NpcJieSuanManager.inst.npcSetField.AddNpcShenShi(npcId, npcBigTuPoFenShu * 6);
            NpcJieSuanManager.inst.npcSetField.AddNpcDunSu(npcId, npcBigTuPoFenShu * 3);
            NpcJieSuanManager.inst.npcSetField.AddNpcZhiZi(npcId, npcBigTuPoFenShu * 3);
            NpcJieSuanManager.inst.npcSetField.AddNpcWuXing(npcId, npcBigTuPoFenShu * 3);
            NpcJieSuanManager.inst.npcNoteBook.NoteHuaShenSuccess(npcId);
            npcData.SetField("FlyTime", DateTime.Parse(NpcJieSuanManager.inst.JieSuanTime).AddYears(1000).ToString());
            npcData.SetField("exp", 0);
            UpDateNpcData(npcId, 13);
            EventMag.Inst.SaveEvent(npcId, 3);
        }

        #region npc通用函数
        //更新npc数据
        public static void UpDateNpcData(int npcId, int level)
        {
            JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
            JSONObject jSONObject = jsonData.instance.LevelUpDataJsonData[npcData["Level"].I.ToString()];
            NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, jSONObject["AddHp"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcShenShi(npcId, jSONObject["AddShenShi"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcDunSu(npcId, jSONObject["AddDunSu"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcShouYuan(npcId, jSONObject["AddShouYuan"].I);
            //NpcJieSuanManager.inst.npcSetField.AddNpcLevel(npcId, 1);
            //int val = NpcJieSuanManager.inst.GetNpcData(npcId)["Level"].I + addNum;
            NpcJieSuanManager.inst.GetNpcData(npcId).SetField("Level", level);
            JSONObject jSONObject2 = new JSONObject();
            for (int i = 0; i < jsonData.instance.NPCLeiXingDate.Count; i++)
            {
                if (jsonData.instance.NPCLeiXingDate[i]["Type"].I == npcData["Type"].I && jsonData.instance.NPCLeiXingDate[i]["LiuPai"].I == npcData["LiuPai"].I && jsonData.instance.NPCLeiXingDate[i]["Level"].I == npcData["Level"].I)
                {
                    jSONObject2 = new JSONObject(jsonData.instance.NPCLeiXingDate[i].ToString());
                    break;
                }
            }

            npcData.SetField("equipWeaponPianHao", jSONObject2["equipWeapon"]);
            npcData.SetField("equipWeapon2PianHao", jSONObject2["equipWeapon"]);
            npcData.SetField("equipClothingPianHao", jSONObject2["equipClothing"]);
            npcData.SetField("equipRingPianHao", jSONObject2["equipRing"]);
            if (jSONObject2.keys.Count > 0)
            {
                npcData.SetField("skills", jSONObject2["skills"]);
                npcData.SetField("staticSkills", jSONObject2["staticSkills"]);
                npcData.SetField("xiuLianSpeed", FactoryManager.inst.npcFactory.getXiuLianSpeed(npcData["staticSkills"], npcData["ziZhi"].I));
                npcData.SetField("yuanying", jSONObject2["yuanying"]);
                npcData.SetField("XinQuType", jSONObject2["XinQuType"].Copy());
                jsonData.instance.MonstarCreatInterstingType(npcId);
                npcData.SetField("HuaShenLingYu", jSONObject2["HuaShenLingYu"]);
            }

            NpcJieSuanManager.inst.UpdateNpcWuDao(npcId);
            int targetId = 0;
            if (NpcJieSuanManager.inst.npcChengHao.IsCanUpToChengHao(npcId, ref targetId))
            {
                NpcJieSuanManager.inst.npcChengHao.UpDateChengHao(npcId, targetId);
            }

            NpcJieSuanManager.inst.npcStatus.SetNpcStatus(npcId, 10);
            AuToWuDaoSkill(npcId);
        }

        //自动悟道技能
        public static void AuToWuDaoSkill(int npcId)
        {
            JSONObject jSONObject = jsonData.instance.AvatarJsonData[npcId.ToString()];
            List<int> list = jSONObject["wuDaoSkillList"].ToList();
            JSONObject jSONObject2 = jSONObject["wuDaoJson"];
            int num = jSONObject["EWWuDaoDian"].I;
            int num2 = 0;
            //int num3 = 0;
            if (num < 1)
            {
                return;
            }

            foreach (JSONObject item in jsonData.instance.NpcWuDaoChiData.list)
            {
                foreach (JSONObject item2 in item["wudaochi"].list)
                {
                    if (list.Contains(item2.I) || num <= item["xiaohao"].I)
                    {
                        continue;
                    }

                    int i = jSONObject2[jsonData.instance.WuDaoJson[item2.ToString()]["Type"][0].I.ToString()]["level"].I;
                    num2 = jsonData.instance.WuDaoJson[item2.ToString()]["Lv"].I;
                    if (i > num2)
                    {
                        jSONObject["wuDaoSkillList"].Add(item2.I);
                        num -= item["xiaohao"].I;
                        jSONObject.SetField("EWWuDaoDian", num);
                        if (num < 1)
                        {
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        //获取金丹的等级
        public static int GetJinDanId(int npcId, int jinDanQuality)
        {
            JSONObject jSONObject = NpcJieSuanManager.inst.GetNpcData(npcId)["JinDanType"];
            foreach (JSONObject item in jsonData.instance.JieDanBiao.list)
            {
                if (jSONObject.Count == 2)
                {
                    if (item["JinDanQuality"].I == jinDanQuality && item["JinDanType"].Count == 2 && item["JinDanType"][0].I == jSONObject[0].I && item["JinDanType"][1].I == jSONObject[1].I)
                    {
                        return item["id"].I;
                    }
                }
                else if (item["JinDanQuality"].I == jinDanQuality && item["JinDanType"].Count == 1 && item["JinDanType"][0].I == jSONObject[0].I)
                {
                    return item["id"].I;
                }
            }

            return 0;
        }

    }
}
