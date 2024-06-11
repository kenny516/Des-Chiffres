
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Dechiffre.Models
{
    public class HelperGame
    {
        public async Task<int> EvaluateStringAsync(string operation)
        {
            try
            {
                var result = await CSharpScript.EvaluateAsync<int>(operation, ScriptOptions.Default.WithReferences(AppDomain.CurrentDomain.GetAssemblies()));
                return result;
            }
            catch (CompilationErrorException ex)
            {
                Console.WriteLine($"Erreur de compilation: {string.Join(Environment.NewLine, ex.Diagnostics)}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur d'exécution: {ex.Message}");
                throw;
            }
        }
    }
}