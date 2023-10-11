namespace Hulk;
sealed class Else : Expresion
{
    public Else (Token keyword, Expresion expresion)
    {
       Keyword = keyword;
       _expresion = expresion;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.else_Expresion;
    public Token Keyword {get;}
    public Expresion _expresion {get;}
}