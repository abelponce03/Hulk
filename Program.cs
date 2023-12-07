using System.Drawing;
using System.IO.Pipes;
using System.Reflection.Emit;
using System.Security.Principal;

namespace Hulk
{

    //Arreglar el ambito de declaracion de fuuncion 
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------HULK------");

            while (true)
            {
                //limpieza de variables en stack y diccionarios
                Biblioteca.Pila.Clear();
                Biblioteca.Variables.Clear();
                //try catch para errores que yo he definido en evaluador
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("> ");
                    string? Entrada = Console.ReadLine();
                    //si la entrada es vacia termina el proceso
                    if (string.IsNullOrWhiteSpace(Entrada)) return;

                    //creacion del objeto parser donde se crea cada expresion del arbol
                    var Parser = new Parser(Entrada);
                    //llamada al metodo que parsea cada token y devuelve expresiones
                    var Arbol = Parser.Parse();

                    if (!Arbol.Errores.Any())
                    {
                        //si el usuario introduce  clean se eliminan todas las funciones en el diccionario 
                        if (Arbol.Rama is Clean)
                        {
                            Biblioteca.Functions.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("LIBRARY IS CLEAN");
                            continue;
                        }
                        //si es una declaracion de funcion pasa a la proxima entrada 
                        if (Arbol.Rama is Declaracion_Funcion)
                        {
                            continue;
                        }
                        //creacion del objeto evaluador
                        var e = new Evaluador(Arbol.Rama);
                        //llamada al metodo evaluar donde se van a evaluar cada nodo del arbol
                        var resultado = e.Evaluar();
                        
                        //color para ver en consola de la salida
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(resultado);
                    }
                    else
                    {
                        //implementacion de colores distintivos para  cada error
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
                                if (mensaje[i] == "FUNCTION")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    break;
                                }
                            }
                            Console.WriteLine(error);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {

                    string[] mensaje = e.ToString().Split();
                    for (int i = 0; i < mensaje.Length; i++)
                    {
                        if (mensaje[i] == "OVERFLOW")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        }
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
                        if (mensaje[i] == "FUNCTION")
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        }
                    }
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

