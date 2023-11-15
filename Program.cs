using System.Drawing;
using System.IO.Pipes;
using System.Security.Principal;

namespace Hulk
{

    //Arreglar el ambito de declaracion de fuuncion 
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"--------HULK--------");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
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
                            Biblioteca.Variables.Clear();
                            continue;
                        }
                        var e = new Evaluador(Arbol.Rama);
                        var resultado = e.Evaluar();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(resultado);
                        Biblioteca.Variables.Clear();
                    }
                    catch (Exception e)
                    {
                        string[] mensaje = e.ToString().Split();
                        for (int i = 0; i < mensaje.Length; i++)
                        {
                            if (mensaje[i] == "SINTAX")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            }
                            if (mensaje[i] == "SEMANTIC")
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            }
                            if (mensaje[i] == "LEXICAL")
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            }
                        }
                        Console.WriteLine(e.Message);
                    }

                }
                else
                {
                    foreach (var error in Arbol.Errores)
                    {
                        string[] mensaje = error.Split();
                        for (int i = 0; i < mensaje.Length; i++)
                        {
                            if (mensaje[i] == "SYNTAX")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            }
                            if (mensaje[i] == "SEMANTIC")
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            }
                            if (mensaje[i] == "LEXICAL")
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            }
                        }
                        Console.WriteLine(error);
                        break;
                    }
                }
            }
        }
    }

}

