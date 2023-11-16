using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace Hulk;

sealed class Arbol
{
    public Arbol( List<string> error, Expresion rama, Token final)
    {
        Rama = rama;
        Final = final;
        Errores = error;
    }
    public  List<string> Errores {get;}
    public Expresion Rama {get;}
    public Token Final {get;}
   
}