using System.Drawing;
using System.IO.Pipes;
using System.Security.Principal;

namespace Hulk
{
    class Program
    {
        static void Main(string[] args)
        {
            //Diccionario para guardar variables
            var variables = new Dictionary<string, Expresion>();
            while (true)
            {
                Console.Write("> ");
                string Entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(Entrada)) return;

                var Parser = new Parser(Entrada);
                var Arbol = Parser.Parse();

                if (!Arbol.Errores.Any())
                {
                    var e = new Evaluador(Arbol.Rama);
                    var resultado = e.Evaluar(variables);
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

