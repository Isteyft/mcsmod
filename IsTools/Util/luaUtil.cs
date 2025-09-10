using System;
using XLua;
using System.Collections.Generic;
using SkySwordKill.Next;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class luaUtil
    {
        private LuaEnv _luaEnv;
        public luaUtil(LuaEnv luaEnv)
        {
            _luaEnv = luaEnv;
        }
        public object[] RunFuncHasResult(string scr, string funcName, object[] args)
        {
            // 加载 Lua 模块
            object[] array = _luaEnv.DoString("return require '" + scr + "'", "chunk", null);
            // 打印所有已注册的 Lua 模块路径
            //foreach (var entry in Main.Lua.LuaCaches)
            //{
            //    IsToolsMain.LogInfo($"模块名: {entry.Key} -> 文件路径: {entry.Value.FilePath}");
            //}
            if (array.Length != 0 && array[0] is LuaTable luaTable)
            {
                // 获取函数
                LuaFunction func = luaTable.Get<LuaFunction>(funcName);
                if (func != null)
                {
                    Main.LogInfo($"找到 '{scr}' 中函数 '{funcName}'");
                    // 调用函数并返回结果（比如 [true] 或 [false]）
                    var result = func.Call(args);
                    if (result != null && result.Length > 0)
                    {
                        Main.LogInfo($"返回值: {result[0].ToString()}");
                        Main.LogInfo($"返回值长度: {result.Length}");
                    }
                    else
                    {
                        Main.LogInfo("返回值为空或长度为0");
                    }
                    return result;
                }
                else
                {
                    Main.LogError($"Lua模块 '{scr}' 中找不到函数 '{funcName}'");
                }
            }
            else
            {
                Main.LogError($"读取Lua模块 '{scr}' 失败，或返回值不是 LuaTable");
            }

            return null; // 出错时返回 null
        }
    }
}