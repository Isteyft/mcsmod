using UnityEngine;

namespace top.Isteyft.MCS.IsTools.Util
{
    internal class AutoSaveUtils
    {
        public static void AutoSave()
        {
            YSNewSaveSystem.SaveGame(PlayerPrefs.GetInt("NowPlayerFileAvatar"), 0, null, false);
        }
    }
}
