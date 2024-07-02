namespace Dechiffre.Models;

public class ExpressionCalculator
{
    private static readonly char[] operations = { '+', '-', '*', '/' };
public static AIResponse[] FindClosestExpressions(double target, int[] numbers, int numExpressions)
{
    var combinations = GenerateCombinations(numbers);
    List<AIResponse> closestExpressions = new List<AIResponse>();

    foreach (var combo in combinations)
    {
        var expressions = GenerateExpressions(combo);
        foreach (var expr in expressions)
        {
            var value = EvaluateExpression(expr);
            double diff = Math.Abs(target - value);
            AIResponse temp = new AIResponse(expr,  value);
            closestExpressions.Add(temp);
            if (diff == 0)
                break;
        }
    }

    return closestExpressions
        .OrderBy(e => Math.Abs(target - e.Value))
        .Take(numExpressions)
        .ToArray(); // Convert the list to an array before returning
}


    private static List<string> GenerateExpressions(List<int> numbers)
    {
        if (numbers.Count == 1)
            return new List<string> { numbers[0].ToString() };

        var expressions = new List<string>();

        for (int i = 1; i < numbers.Count; i++)
        {
            var left = numbers.Take(i).ToList();
            var right = numbers.Skip(i).ToList();

            var leftExpressions = GenerateExpressions(left);
            var rightExpressions = GenerateExpressions(right);

            foreach (var l in leftExpressions)
            {
                foreach (var r in rightExpressions)
                {
                    foreach (var op in operations)
                    {
                        expressions.Add($"({l} {op} {r})");
                    }
                }
            }
        }

        return expressions;
    }
    private static double EvaluateExpression(string expr)
    {
        try
        {
            return Convert.ToDouble(new System.Data.DataTable().Compute(expr, null));
        }
        catch (Exception)
        {
            return double.NaN;
        }
    }

    private static List<List<int>> GenerateCombinations(int[] numbers)
    {
        var result = new List<List<int>>();

        void Helper(int start, List<int> combo)
        {
            for (int i = start; i < numbers.Length; i++)
            {
                combo.Add(numbers[i]);
                result.Add(new List<int>(combo));
                Helper(i + 1, combo);
                combo.RemoveAt(combo.Count - 1);
            }
        }

        Helper(0, new List<int>());
        return result;
    }



}