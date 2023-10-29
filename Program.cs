using System.Drawing;
using System.IO.Pipes;
using System.Security.Principal;

namespace Hulk
{
    class Program
    {
        static void Main(string[] args)
        {
            var funciones = new Dictionary<string, Expresion>();
            while (true)
            {
                Console.Write("> ");
                string Entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(Entrada)) return;

                var Parser = new Parser(Entrada);
                var Arbol = Parser.Parse();


                if (!Arbol.Errores.Any())
                {
                    try
                    {
                        if (Arbol.Rama is Declaracion_Funcion)
                        {
                            continue;
                        }
                        var e = new Evaluador(Arbol.Rama);
                        var resultado = e.Evaluar();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                else
                {
                    foreach (var error in Arbol.Errores)
                    {
                        Console.WriteLine(error);
                        break;
                    }
                }
            }
        }
    }

}

