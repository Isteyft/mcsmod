using HarmonyLib;
using IsMoXinXiangJuan;
using Newtonsoft.Json.Linq;
using script.EventMsg;
using SkySwordKill.Next;
using SkySwordKill.NextModEditor.Mod;
using System;
using System.Collections.Generic;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    [HarmonyPatch(typeof(NPCTuPo))]
    public static class NPCTuPoPatch
    {
        [HarmonyPatch("NpcTuPoJinDan")]
        [HarmonyPrefix]
        private static bool NpcTuPoJinDan_Prefix(int npcId, bool isKuaiSu, NPCTuPo __instance)
        {
            //IsMoXinXiangJuanMain.Log($"{npcId}");
            int oldNpcId = NPCEx.NPCIDToOld(npcId);
            if (NPCEx.IsDeath(npcId))
            {
                //IsToolsMain.Warning($"npc已死亡");
                return true;
            }
            //IsMoXinXiangJuanMain.Log($"{oldNpcId}");
            // 检查是否是目标NPC
            if (oldNpcId == 10305)
            {
                IsMoXinXiangJuanMain.Log("进入结算逻辑");
                // 获取NPC数据
                JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
                IsMoXinXiangJuanMain.Log("进入结算逻辑判断");
                // 检查是否有预设的金丹突破时间
                if (npcData.HasField("JinDanTime"))
                {
                    DateTime t = DateTime.Parse(npcData["JinDanTime"].str);
                    // 如果当前时间未到突破时间，直接返回
                    if (!(NpcJieSuanManager.inst.GetNowTime() >= t))
                    {
                        //UIPopTip.Inst.Pop("时间没到");
                        //IsMoXinXiangJuanMain.Log("时间没到");
                        return false;
                    }
                }
                //IsMoXinXiangJuanMain.Log("进入结算更新");
                int nineGradeId = __instance.GetJinDanId(npcId, 9);
                JSONObject nineGradeData = jsonData.instance.JieDanBiao[nineGradeId.ToString()];

                JSONObject jinDanData = new JSONObject();
                jinDanData.SetField("JinDanId", nineGradeId);
                jinDanData.SetField("JinDanLv", 9);
                jinDanData.SetField("JinDanAddSpeed", nineGradeData["EXP"].I);
                jinDanData.SetField("JinDanAddHp", nineGradeData["HP"].I);

                // 更新NPC数据
                npcData.SetField("JinDanData", jinDanData);
                NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, nineGradeData["HP"].I);
                npcData.SetField("exp", 0);
                UpDateNpcData(npcId);

                // 记录突破成功事件
                NpcJieSuanManager.inst.npcNoteBook.NoteJinDanSuccess(npcId, 9);
                EventMag.Inst.SaveEvent(npcId, 1);

                // 返回false表示跳过原方法执行
                return false;
            }
            // 非目标NPC，正常执行原方法
            return true;
        }



        private static void UpDateNpcData(int npcId)
        {
            JSONObject npcData = NpcJieSuanManager.inst.GetNpcData(npcId);
            JSONObject jSONObject = jsonData.instance.LevelUpDataJsonData[npcData["Level"].I.ToString()];
            NpcJieSuanManager.inst.npcSetField.AddNpcHp(npcId, jSONObject["AddHp"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcShenShi(npcId, jSONObject["AddShenShi"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcDunSu(npcId, jSONObject["AddDunSu"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcShouYuan(npcId, jSONObject["AddShouYuan"].I);
            NpcJieSuanManager.inst.npcSetField.AddNpcLevel(npcId, 1);
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


    }
}