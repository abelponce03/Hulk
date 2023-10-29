namespace Hulk;
sealed class Variable : Expresion
{
    public Variable (Token identificador)
    {
       Identificador = identificador;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.Variable;
    public Token Identificador {get;}
}