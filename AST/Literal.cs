namespace Hulk;

sealed class Literal : Expresion
{
    //el tipo de token que buscara sera numero
    public override Tipo_De_Token Tipo => Tipo_De_Token.Literal;
    public Token _Literal { get; }
    //se lo paso a este nodo;
    public Literal(Token literal)
    {
        _Literal = literal;
    }
}