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
sealed class Asignacion : Expresion
{
    public Asignacion (Token identificador, Token igual, Expresion expresion)
    {
       Identificador = identificador;
       Igual = igual;
       _expresion = expresion;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.Asignacion;
    public Token Identificador {get;}
    public Token Igual{get;}
    public Expresion _expresion {get;}
    
}