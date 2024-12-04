using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using suzabkktgame;

public class Modding
{
    public static async Task<object> exec(string code)
    {
        try
        {
            var options = ScriptOptions.Default
                .WithReferences(AppDomain.CurrentDomain.GetAssemblies())
                .WithImports("System", "System.Collections.Generic","Raylib_cs","Newtonsoft.Json","rlImGui_cs","ImGuiNET","System.Numerics","suzabkktgame");

            return await CSharpScript.EvaluateAsync<object>(code, options);
        }
        catch (Exception ex)
        {
            Logger.sendmessage($"Error: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
}
