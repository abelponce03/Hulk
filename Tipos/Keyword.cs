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
            case "if":
                return Tipo_De_Token.if_Keyword;
            case "else":
                return Tipo_De_Token.else_Keyword;   
            case "print":
                return Tipo_De_Token.print_Keyword;
            case "let":
                return Tipo_De_Token.let_Keyword;
            case "in":
                return Tipo_De_Token.in_Keyword;
            case "sen":
                return Tipo_De_Token.sen_Keyword;
            case "cos":
                return Tipo_De_Token.cos_Keyword;
            case "PI":
                return Tipo_De_Token.PI_Keyword; 
            case "function":
                return Tipo_De_Token.function_Keyword;
            case "log":
                return Tipo_De_Token.logaritmo_Keyword;                          
            default:
                return Tipo_De_Token.Identificador;
        }
    }
}