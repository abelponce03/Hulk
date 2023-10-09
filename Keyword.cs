namespace Hulk;

class Keyword
{
    public static Tipo_De_Token Tipo(string texto)
    {
        switch (texto)
        {
            case "true":
                return Tipo_De_Token.True_Keyword;
            case "false":
                return Tipo_De_Token.False_Keyword;
            default:
                return Tipo_De_Token.Identificador;
        }
    }
}