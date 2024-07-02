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
// suggest 

    public (int, string) FindClosestNumber(List<int> numbers, int target)
    {
        if (numbers.Contains(target))
        {
            return (target, target + "");
        }

        List<List<int>> r = nearestNumber(numbers, target, 0, new List<List<int>>());
        if (r.Count == 0)
        {
            int min = Int32.MinValue;
            int nombre = 0;
            foreach (var nbr in numbers)
            {
                if (min > Math.Abs(nbr - target))
                {
                    min = Math.Abs(nbr - target);
                    nombre = nbr;
                }
            }

            return (nombre, nombre + "");
        }

        int closest = r[r.Count - 1][0];
        string operation = getOp(r);
        return (closest, operation);
    }

    int nearestNumberVal(List<int> numbers,int target)
    {
        int min = Int32.MaxValue;
        int nombre = 0;
        foreach (var nbr in numbers)
        {
            if (min > Math.Abs(nbr - target))
            {
                min = Math.Abs(nbr - target);
                nombre = nbr;
            }
        }
        return nombre;
    }
     public static List<List<int>> getSolution(List<int> nbTools, int nbGuess)
    {
        string list = string.Join(", ", nbTools);
        Console.WriteLine($"Nb Guess: {nbGuess} | Nb Tools: {list}");
        return nearestNumber(nbTools, nbGuess, 0, new List<List<int>>());
    }

    static int getNearest(int t, List<int> listNbs)
    {
        int minDecal = int.Abs(t - listNbs[0]),
            minNb = listNbs[0];
        for (int i = 1; i < listNbs.Count(); i++)
        {
            int decal = int.Abs(t - listNbs[i]);
            if (decal < minDecal)
            {
                minDecal = decal;
                minNb = listNbs[i];
            }
        }

        return minNb;
    }

    static List<List<int>> nearestNumber(List<int> listNbs, int t, int N, List<List<int>> listOperations)
    {
        int idOperator = (N > t) ? 2 : 0;
        int nearestToT = getNearest(t, listNbs);
        int decalMinOfList = int.Abs(t - nearestToT);
        if (N == 0)
        {
            if (nearestToT == t) return [[t]];

            if (t < nearestToT) idOperator = 2;
        }

        if (N == t || listNbs.Count == 1) return listOperations;

        // Utiliser des operations de reduction quand sumResults > t

        // Sort listNbs descending
        listNbs = listNbs.OrderByDescending(n => n).ToList();

        // Obtenir le resultat d'operation le plus proche de t a partir de N et des elements de listNbs
        List<int> nearestForOperator = getNearestForOperator(t, idOperator, listNbs, N),
            nearestForNextOperator = getNearestForOperator(t, idOperator + 1, listNbs, N);

        int decalOp = int.Abs(t - nearestForOperator[0]),
            decalNextOp = int.Abs(t - nearestForNextOperator[0]);

        List<int> operation = [..decalOp < decalNextOp ? nearestForOperator : nearestForNextOperator];
        if (decalOp < decalNextOp) operation.Add(idOperator);
        else operation.Add(idOperator + 1);

        List<int> bestOp = [operation[0], listNbs[operation[1]], listNbs[operation[2]], operation[3]];

        if (N == 0 && int.Abs(t - bestOp[0]) > decalMinOfList)
        {
            return [[nearestToT]];
        }

        int decalN = int.Abs(t - N),
            decalBestOp = int.Abs(t - bestOp[0]);
        if (decalN <= decalBestOp) return listOperations; // Si N est deja proche de t le plus possible

        listOperations.Add(bestOp);

        // Supprimer les deux nombres nb1 et nb2 de listNbs
        listNbs.Add(operation[0]);
        if (operation[1] > operation[2])
        {
            listNbs.RemoveAt(operation[1]);
            listNbs.RemoveAt(operation[2]);
        }
        else
        {
            listNbs.RemoveAt(operation[2]);
            listNbs.RemoveAt(operation[1]);
        }

        // Sort listNbs descending
        listNbs = listNbs.OrderByDescending(n => n).ToList();

        return nearestNumber(listNbs, t, operation[0], listOperations);
    }

    static int exeOperation(int idOperator, int nb1, int nb2)
    {
        return idOperator switch
        {
            0 => nb1 * nb2,
            1 => nb1 + nb2,
            2 => nb1 - nb2,
            3 => nb1 / nb2,
            _ => throw new ArgumentOutOfRangeException(nameof(idOperator), idOperator, "Unknown operation!")
        };
    }

    static List<int> getNearestForOperator(int t, int idOperator, List<int> listNbs, int N)
    {
        List<int> operationInf = [], // [result, index_nb1, index_nb2]
            operationSup = []; // [result, index_nb1, index_nb2]

        bool lookFor = true;
        int infToT = int.MinValue, supToT = int.MaxValue;
        for (int i = 0; i < listNbs.Count - 1 && lookFor; i++)
        {
            for (int j = i + 1; j < listNbs.Count && lookFor; j++)
            {
                int result = exeOperation(idOperator, listNbs[i], listNbs[j]);

                if (result == t) return [t, i, j];

                if (N == 0)
                {

                }

                if (result < t)
                // if (result < t && (idOperator == 0 || idOperator == 1))
                {
                    infToT = result;
                    operationInf = [infToT, i, j];
                    lookFor = false;
                    break;
                }

                supToT = result;
                operationSup = [supToT, i, j];
            }
        }

        if (infToT == int.MinValue) return operationSup; // There is no result < t
        if (supToT == int.MaxValue) return operationInf; // There is no result > t

        // Look for the result nearest to t
        return int.Abs(t - infToT) < int.Abs(t - supToT) ? operationInf : operationSup;
    }

    string getOperation(int idOperator)
    {
        return idOperator switch
        {
            0 => "*",
            1 => "+ ",
            2 => "-",
            3 => "/",
            _ => throw new ArgumentOutOfRangeException(nameof(idOperator), idOperator, "Unknown operation!")
        };
    }

    string getOp(List<List<int>> r)
    {
        string result = "";
        Console.WriteLine("counttt" + r.Count);

        result += (r[0][1] + getOperation(r[0][r[0].Count - 1]) + r[0][2]);
        for (int i = 1; i < r.Count; i++)
        {
            List<int> number = r[i];
            result += (getOperation(number[number.Count - 1]) + number[2]);
        }

        return result;
    }
}