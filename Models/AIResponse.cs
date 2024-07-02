namespace Dechiffre.Models;

public class AIResponse
{
    string expression;
    double value;

    public AIResponse() { 
    }

    public AIResponse(string expression, double value)
    {
        this.Expression = expression;
        this.Value = value;
    }

    public string Expression { get => expression; set => expression = value; }

    public double Value
    {
        get => value;
        set => this.value = value;
    }

}