namespace Hulk;

class Clean : Expresion
{
    public Clean( Token keyword)
    {
       Keyword = keyword;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.clean_Expresion;
    public Token Keyword {get;}
} 