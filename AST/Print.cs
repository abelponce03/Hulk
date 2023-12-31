namespace Hulk;
sealed class Print : Expresion
{
    public Print (Token keyword,Token parentesis_abierto, Expresion expresion, Token parentesis_cerrado)
    {
       Keyword = keyword;
       _parentesis_abierto = parentesis_abierto;
       _expresion = expresion;
       _parentesis_cerrado = parentesis_cerrado;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.Print_Expresion;
    public Token Keyword {get;}
    public Token _parentesis_abierto {get;}
    public Expresion _expresion {get;}
    public Token _parentesis_cerrado {get;}
}