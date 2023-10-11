namespace Hulk;
sealed class Print : Expresion
{
    public Print (Token keyword, Expresion expresion)
    {
       Keyword = keyword;
       _expresion = expresion;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.Print_Expresion;
    public Token Keyword {get;}
    public Expresion _expresion {get;}
}