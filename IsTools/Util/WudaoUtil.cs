using DebuggingEssentials;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class WudaoUtil
    {
        public static bool HasWuDaoSkill(int SeidID)
        {
            List<SkillItem> allWuDaoSkills = PlayerEx.Player.wuDaoMag.GetAllWuDaoSkills();

            foreach (SkillItem skillItem in allWuDaoSkills)
            {
                if (skillItem.itemId == SeidID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
