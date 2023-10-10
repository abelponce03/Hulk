namespace Hulk;

sealed class Literal : Expresion
{
    public Literal(Token literal, object valor)
    {
        _Literal = literal;
        Valor = valor;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.Literal;
    public Token _Literal { get; }
    public object Valor { get; }

  
}