using System.Data;

namespace Dechiffre.Models;

public class Tools
{
    public static int EvaluateExpression(string expression)
    {
        // Supprimer les espaces de la chaîne d'expression
        expression = expression.Replace(" ", "");

        // Vérifier la validité de l'expression
        if (!IsValidExpression(expression))
        {
            throw new ArgumentException("Expression invalide.");
        }

        // Utiliser la classe DataTable pour évaluer l'expression
        DataTable table = new DataTable();
        DataColumn column = new DataColumn("Eval", typeof(int), expression);
        table.Columns.Add(column);
        table.Rows.Add(0);

        // Récupérer le résultat et le convertir en entier
        int result = Convert.ToInt32(table.Rows[0]["Eval"]);

        return result;
    }

    // Vérifier si l'expression est valide en utilisant la classe DataTable
    private static bool IsValidExpression(string expression)
    {
        DataTable table = new DataTable();
        DataColumn column = new DataColumn("Eval", typeof(int), expression);
        table.Columns.Add(column);
        try
        {
            table.Rows.Add(0);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }


    // Méthode pour générer des nombres aléatoires
    public static List<int> GenerateNumbers(int count, int min, int max)
    {
        Random _random = new Random();
        var numbers = new List<int>();
        for (var i = 0; i < count; i++)
        {
            numbers.Add(_random.Next(min, max));
        }

        return numbers;
    }

    public static (int, string) FindClosestNumber(int[] numbers, int target)
    {
        string[] operations = numbers.Select(n => n.ToString()).ToArray();
        var results = new List<(int result, string ops)>();
        GenerateResults(numbers, operations, results);

        return results.OrderBy(r => Math.Abs(target - r.result)).First();
    }

    private static void GenerateResults(int[] numbers, string[] operations, List<(int, string)> results,
        string currentOps = "")
    {
        if (numbers.Length == 1)
        {
            results.Add((numbers[0], currentOps));
            return;
        }

        for (int i = 0; i < numbers.Length; i++)
        {
            for (int j = 0; j < numbers.Length; j++)
            {
                if (i != j)
                {
                    var newNumbers = numbers.Where((_, idx) => idx != i && idx != j).ToList();
                    var newOps = operations.Where((_, idx) => idx != i && idx != j).ToList();

                    foreach (var (result, op) in ApplyOperations(numbers[i], operations[i], numbers[j], operations[j]))
                    {
                        newNumbers.Add(result);
                        newOps.Add(op);
                        GenerateResults(newNumbers.ToArray(), newOps.ToArray(), results, op);
                        newNumbers.RemoveAt(newNumbers.Count - 1);
                        newOps.RemoveAt(newOps.Count - 1);
                    }
                }
            }
        }
    }

    private static IEnumerable<(int, string)> ApplyOperations(int a, string aOp, int b, string bOp)
    {
        var results = new List<(int, string)>
        {
            (a + b, $"({aOp}) + ({bOp})"),
            (a - b, $"({aOp}) - ({bOp})"),
            (a * b, $"({aOp}) * ({bOp})")
        };

        if (b != 0)
            results.Add((a / b, $"({aOp}) / ({bOp})"));

        if (a != 0)
            results.Add((b / a, $"({bOp}) / ({aOp})"));

        return results;
    }
}