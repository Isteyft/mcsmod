using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IsMoXinXiangJuan;
using Newtonsoft.Json.Linq;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Data
{
    internal class StaticSkillTypeMetaData
    {
        public static StaticSkillTypeMetaData Inst
        {
            get { return _inst ?? (_inst = new StaticSkillTypeMetaData()); }
        }

        private static StaticSkillTypeMetaData _inst;
        private bool _isInit;
        private readonly List<ModStaticSkillType> _staticSkillTypes = new List<ModStaticSkillType>();

        private StaticSkillTypeMetaData()
        {
            LoadFromJson();
        }

        public void Load()
        {
            if (_isInit) return;

            foreach (var skillType in _staticSkillTypes)
            {
                if (!ModEditorManager.I.StaticSkillTypes.Any(x => x.Id == skillType.Id))
                {
                    ModEditorManager.I.StaticSkillTypes.Add(skillType);
                }
                else
                {
                    IsMoXinXiangJuanMain.Warning("已存在问题");
                    IsMoXinXiangJuanMain.Warning($"功法类型 [{skillType.Id}] 已存在，请检查 Mod 列表");
                }
            }

            _isInit = true;
        }

        private void LoadFromJson()
        {
            try
            {
                string jsonPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Meta", "StaticSkillType.json");

                if (!File.Exists(jsonPath))
                {
                    IsMoXinXiangJuanMain.Error($"未找到 StaticSkillType.json 文件: {jsonPath}");
                    return;
                }

                var json = JArray.Parse(File.ReadAllText(jsonPath));
                _staticSkillTypes.AddRange(json.ToObject<List<ModStaticSkillType>>());
            }
            catch (Exception ex)
            {
                IsMoXinXiangJuanMain.Error($"加载 StaticSkillType.json 失败: {ex.Message}");
            }
        }

        public void AddStaticSkillType(ModStaticSkillType skillType)
        {
            if (_staticSkillTypes.Any(x => x.Id == skillType.Id))
            {
                IsMoXinXiangJuanMain.Warning("已存在问题");
                IsMoXinXiangJuanMain.Warning($"功法类型 [{skillType.Id}] 已存在，将被覆盖");
                _staticSkillTypes.RemoveAll(x => x.Id == skillType.Id);
            }
            _staticSkillTypes.Add(skillType);
        }
    }
}