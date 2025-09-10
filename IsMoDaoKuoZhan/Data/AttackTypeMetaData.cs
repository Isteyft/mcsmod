using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;
using Newtonsoft.Json.Linq;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Data
{
    internal class AttackTypeMetaData
    {
        public static AttackTypeMetaData Inst
        {
            get { return _inst ?? (_inst = new AttackTypeMetaData()); }
        }

        private static AttackTypeMetaData _inst;
        private bool _isInit;
        private readonly List<ModAttackType> _attackTypes = new List<ModAttackType>();

        private AttackTypeMetaData()
        {
            LoadFromJson();
        }

        public void Load()
        {
            if (_isInit) return;

            foreach (var attackType in _attackTypes)
            {
                if (!ModEditorManager.I.AttackTypes.Contains(attackType))
                {
                    ModEditorManager.I.AttackTypes.Add(attackType);
                }
                else
                {
                    IsMoDaoKuoZhanMain.Warning("已存在冲突问题");
                    IsMoDaoKuoZhanMain.Warning($"AttackType [{attackType.Id}] 发生冲突，请检查 Mod 列表");
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
                    "Meta", "AttackType.json");

                if (!File.Exists(jsonPath))
                {
                    IsMoDaoKuoZhanMain.Error($"AttackType.json 文件未找到: {jsonPath}");
                    return;
                }

                var json = JArray.Parse(File.ReadAllText(jsonPath));
                _attackTypes.AddRange(json.ToObject<List<ModAttackType>>());
            }
            catch (Exception ex)
            {
                IsMoDaoKuoZhanMain.Error($"加载 AttackType.json 失败: {ex.Message}");
            }
        }

        public void AddAttackType(ModAttackType attackType)
        {
            if (_attackTypes.Any(x => x.Id == attackType.Id))
            {
                IsMoDaoKuoZhanMain.Warning("已存在问题");
                IsMoDaoKuoZhanMain.Warning($"AttackType [{attackType.Id}] 已存在，将被覆盖");
                _attackTypes.RemoveAll(x => x.Id == attackType.Id);
            }
            _attackTypes.Add(attackType);
        }
    }
}
