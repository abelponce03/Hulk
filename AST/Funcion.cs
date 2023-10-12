using System.Data.Common;

namespace Hulk;

sealed class Function : Expresion
{
    public Function(Token keyword, Token identificador ,Token parentesis_abierto, Expresion expresion , Token parentesis_cerrado, Token implicacion, Expresion funcion)
    {
        Keyword = keyword;
        Identificador = identificador;
        _parentesis_abierto = parentesis_abierto;
        _expresion = expresion;
        _parentesis_cerrado = parentesis_cerrado;
        Implicacion = implicacion;
        Funcion = funcion;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.function_Expresion;
    public Token Keyword { get; }
    public Token Identificador { get; }
    public Token _parentesis_abierto {get;}
    public Expresion _expresion {get;}
    public Token _parentesis_cerrado {get;}
    public Token Implicacion { get; }
    public Expresion Funcion { get; }
}
  
    
