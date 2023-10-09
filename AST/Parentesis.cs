namespace Hulk;
sealed class Parentesis : Expresion
{
    public Parentesis (Token parentesis_abierto, Expresion expresion , Token parentesis_cerrado)
    {
        Parentesis_Abierto = parentesis_abierto;
        Expresion = expresion;
        Parentesis_Cerrado = parentesis_cerrado;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.Parentesis; 
    public Token Parentesis_Abierto {get;}
    public Expresion Expresion {get;}
    public Token Parentesis_Cerrado {get;}
}