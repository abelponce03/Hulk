namespace Hulk;
sealed class IF : Expresion
{
    public IF(Token keyword, Expresion condicion, Expresion expresion, Expresion _else)
    {
       Keyword = keyword;
       Condicion = condicion;
       _expresion = expresion;
       _Else = _else;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.if_Expresion;
    public Token Keyword {get;}
    public Expresion Condicion {get;}
    public Expresion _expresion {get;}
    public Expresion _Else {get;}
}