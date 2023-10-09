namespace Hulk;
internal  static class Precedence
{
     public static int Precedencia_Operadores_Unarios(this Tipo_De_Token tipo)
    {
        switch (tipo)
        {
            case Tipo_De_Token.Suma:
            case Tipo_De_Token.Resta:
                return 3;

            default :
                return 0;    
        }
    }
    public static int Precedencia_Operadores_Binarios(this Tipo_De_Token tipo)
    {
        switch (tipo)
        {
            case Tipo_De_Token.Producto:
            case Tipo_De_Token.Division:
                return 2;
            case Tipo_De_Token.Suma:
            case Tipo_De_Token.Resta:
                return 1;

            default :
                return 0;    
        }
    }
}