namespace Hulk;

sealed class Let : Expresion
{
    public Let (Token keyword, Expresion asignar, Expresion IN)
    {
        Keyword = keyword;
        Asignar = asignar;
        _IN = IN;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.let_Expresion;
    public Token Keyword {get;}
    public Expresion Asignar {get;}
    public Expresion _IN {get;}
}