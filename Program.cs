using System.Drawing;
using System.IO.Pipes;
using System.Security.Principal;

namespace Hulk
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string Entrada = " (-1)^2 == 1 || 2 < 1";
                if (string.IsNullOrWhiteSpace(Entrada)) return;

                var Parser = new Parser(Entrada);
                var Arbol = Parser.Parse();

                if (!Arbol.Errores.Any())
                {
                    var e = new Evaluador(Arbol.Rama);
                    var resultado = e.Evaluar();
                    Console.WriteLine(resultado);
                }
                else
                {
                    foreach (var error in Arbol.Errores)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }
    }

}

