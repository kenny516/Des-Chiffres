﻿using System.Data;

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

    public static int suggest(int targetNumber, List<int> numbers)
    {
        return 1;
    }
}