namespace Hulk;
sealed class IF : Expresion
{
    public IF(Token keyword, Token parentesis_abierto, Expresion condicion, Token parentesis_cerrado, Expresion expresion, Expresion _else)
    {
       Keyword = keyword;
       _parentesis_abierto = parentesis_abierto;
       Condicion = condicion;
       _parentesis_cerrado = parentesis_cerrado;
       _expresion = expresion;
       _Else = _else;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.if_Expresion;
    public Token Keyword {get;}
    public Token _parentesis_abierto {get;}
    public Expresion Condicion {get;}
    public Token _parentesis_cerrado {get;}
    public Expresion _expresion {get;}
    public Expresion _Else {get;}
}