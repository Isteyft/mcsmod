using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.IO;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_HasMod")]
    public class IS_HasMod : IDialogEnvQuery
    {
        private static List<DirectoryInfo> _mods;

        public object Execute(DialogEnvQueryContext context)
        {
            // 保持原有的参数获取方式
            string modName = DialogEnvQueryContextExtension.GetMyArgs<string>(context, 0, "");

            if (_mods == null)
            {
                _mods = WorkshopTool.GetAllModDirectory();
            }

            foreach (var modDir in _mods)
            {
                if (modDir.FullName.Contains(modName))
                {
                    return !WorkshopTool.CheckModIsDisable(modName);
                }
            }

            return false;
        }
    }
}