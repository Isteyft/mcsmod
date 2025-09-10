using Google.Protobuf.WellKnownTypes;
using HarmonyLib;
using JSONClass;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Patch
{
    [HarmonyPatch(typeof(Spell))]
    public class AddBuffPatch
    {
        [HarmonyPatch("addBuff")]
        [HarmonyPrefix]
        public static bool AddBuffPrefix(int buffid, ref int num, Spell __instance)
        {
            // 在方法开始处添加（记录耗时）：
            //Stopwatch stopwatch = Stopwatch.StartNew();
            if (!_BuffJsonData.DataDict.ContainsKey(buffid))
            {
                IsToolsMain.Error(string.Format("无法添加id为{0}的buff，buff不存在，请检查配表", buffid));
                return false;
            }
            var avatar = (Avatar)__instance.entity;
            BuffMag buffMag = avatar.buffmag;
            if (avatar.buffmag.HasBuffSeid(366))
            {
                //获取玩家buff里面含有366seid的buff
                foreach (List<int> list2 in avatar.buffmag.getBuffBySeid(366))
                {
                    //执行完后释放资源
                    using (List<JSONObject>.Enumerator enumerator2 = jsonData.instance.BuffSeidJsonData[366][string.Concat(list2[2])]["value1"].list.GetEnumerator())
                        while (enumerator2.MoveNext())
                        {
                            if ((int)enumerator2.Current.n == buffid)
                            {
                                int currentBuffId = (int)jsonData.instance.BuffSeidJsonData[366][string.Concat(list2[2])]["id"].n;
                                int currentBuffsum = buffMag.GetBuffSum(currentBuffId);
                                currentBuffsum -= num;
                                if (currentBuffsum > 0)
                                {
                                    foreach (List<int> list in avatar.buffmag.getBuffByID(currentBuffId))
                                    {

                                        foreach (int nums in list)
                                        {
                                            IsToolsMain.Log(nums.ToString());
                                        }
                                        list[1] = list[1] - num;
                                    }
                                    Event.fireOut("UpdataBuff", Array.Empty<object>());
                                    //IsToolsMain.Log(currentBuffsum.ToString());
                                    //buffMag.RemoveBuff(currentBuffId);
                                    //avatar.spell.addDBuff(currentBuffId, currentBuffsum);
                                    // 在方法返回前添加（如果是多个return点，每个return前都要添加）：
                                    //stopwatch.Stop();
                                    //IsToolsMain.Log($"添加BUFF耗时: {stopwatch.ElapsedMilliseconds}毫秒");
                                    return false;
                                }
                                else if (currentBuffsum == 0)
                                {
                                    buffMag.RemoveBuff(currentBuffId);
                                    ////IsToolsMain.Log(currentBuffsum.ToString());
                                    // 在方法返回前添加（如果是多个return点，每个return前都要添加）：
                                    //stopwatch.Stop();
                                    //IsToolsMain.Log($"添加BUFF耗时: {stopwatch.ElapsedMilliseconds}毫秒");
                                    return false;
                                }
                                else
                                {
                                    num = Math.Abs(currentBuffsum);
                                    buffMag.RemoveBuff(currentBuffId);
                                    //IsToolsMain.Log(currentBuffsum.ToString());
                                }

                            }
                        }
                }
            }
            //IsToolsMain.Log("正常添加buff");
            return true;
        }
    }
    [HarmonyPatch(typeof(Spell), nameof(Spell.addDBuff), new System.Type[] { typeof(int) })]
    public class AddDBuffPatch
    {
        [HarmonyPrefix]
        public static bool AddDBuffPrefix(int buffid, Spell __instance)
        {
            //// 在方法开始处添加（记录耗时）：
            //Stopwatch stopwatch = Stopwatch.StartNew();
            if (!_BuffJsonData.DataDict.ContainsKey(buffid))
            {
                IsToolsMain.Error(string.Format("无法添加id为{0}的buff，buff不存在，请检查配表", buffid));
                return false;
            }
            var avatar = (Avatar)__instance.entity;
            BuffMag buffMag = avatar.buffmag;
            if (avatar.buffmag.HasBuffSeid(366))
            {
                //获取玩家buff里面含有366seid的buff
                foreach (List<int> list2 in avatar.buffmag.getBuffBySeid(366))
                {
                    //执行完后释放资源
                    using (List<JSONObject>.Enumerator enumerator2 = jsonData.instance.BuffSeidJsonData[366][string.Concat(list2[2])]["value1"].list.GetEnumerator())
                        while (enumerator2.MoveNext())
                        {
                            if ((int)enumerator2.Current.n == buffid)
                            {
                                int currentBuffId = (int)jsonData.instance.BuffSeidJsonData[366][string.Concat(list2[2])]["id"].n;
                                //int currentBuffsum = buffMag.GetBuffSum(currentBuffId);
                                //buffMag.RemoveBuff(currentBuffId);
                                //avatar.spell.addDBuff(currentBuffId, currentBuffsum - 1);
                                foreach (List<int> list in avatar.buffmag.getBuffByID(currentBuffId))
                                {

                                    foreach (int nums in list)
                                    {
                                        IsToolsMain.Log(nums.ToString());
                                    }
                                    list[1] = list[1] - 1;
                                }
                                Event.fireOut("UpdataBuff", Array.Empty<object>());
                                // 在方法返回前添加（如果是多个return点，每个return前都要添加）：
                                //stopwatch.Stop();
                                //IsToolsMain.Log($"添加BUFF耗时: {stopwatch.ElapsedMilliseconds}毫秒");
                                return false;
                            }
                        }
                }
            }
            //IsToolsMain.Log("正常添加buff");
            return true;
        }
    }




}
//foreach (List<int> list in avatar.buffmag.getBuffByID((int)this.getSeidJson(seid)["value2"].n))
//{
//    list[1] = list[1] - (int)this.getSeidJson(seid)["value1"].n * buffInfo[1];
//}