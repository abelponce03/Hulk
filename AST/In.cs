namespace Hulk;

sealed class In : Expresion
{
    public In ( Token keyword, Expresion expresion)
    {
        Keyword = keyword;
        _expresion = expresion;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.in_Expresion;

    public Token Keyword {get;}
    public Expresion _expresion {get;}
}