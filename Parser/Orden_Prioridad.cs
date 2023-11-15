namespace Hulk;
internal static class Prioridad
{
    public static int Prioridad_Operadores_Unarios(this Tipo_De_Token tipo)
    {
        switch (tipo)
        {
            case Tipo_De_Token.Suma:
            case Tipo_De_Token.Resta:
            case Tipo_De_Token.Bang:
                return 7;

            default:
                return 0;
        }
    }
    public static int Prioridad_Operadores_Binarios(this Tipo_De_Token tipo)
    {
        switch (tipo)
        {
            case Tipo_De_Token.Potenciacion:
                return 6;
            case Tipo_De_Token.Producto:
            case Tipo_De_Token.Division:
            case Tipo_De_Token.resto:
                return 5;

            case Tipo_De_Token.Suma:
            case Tipo_De_Token.Resta:
            case Tipo_De_Token.concatenacion:
                return 4;

            case Tipo_De_Token.Menor_que:
            case Tipo_De_Token.Menor_igual_que:
            case Tipo_De_Token.Mayor_que:
            case Tipo_De_Token.Mayor_igual_que:
            case Tipo_De_Token.Bang_Igual:
            case Tipo_De_Token.IgualIgual:
                return 3;

            case Tipo_De_Token.AmpersandAmpersand:
                return 2;

            case Tipo_De_Token.PipePipe:
                return 1;

            default:
                return 0;
        }
    }
}