using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hulk
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Write("> ");
                string prueba = Console.ReadLine();
                Imprimir_Lista(Analizador_lexico(prueba).Item1, Analizador_lexico(prueba).Item2);
            }
        }
        //Esta funcion es para tomar todo lo que escriba el usuario y separarlo en terminos 
        //que mas tardes seran analizados uno a uno y segun las propiedades que cumplan 
        //se vera a que conjunto pertenecen
        public static Tuple<List<string>, List<string>> Analizador_lexico (string prueba)
        {
            char[] charete = new char[prueba.Length];
            for(int i  = 0; i < prueba.Length; i++)
            {
                charete[i] = prueba[i];
            }
            List<string> termino = new List<string>();
            List<string> Analizado = new List<string>(); 
            int valor = 0;
            for (int i = 0; i < charete.Length; i++)
            {
                if (charete[i] != '>' && charete[i] != '<' && charete[i] != ',' && charete[i] != ' ' && charete[i] != '(' && charete[i] != ')' && charete[i] != '+' && charete[i] != '-' && charete[i] != '*' && charete[i] != '^' && charete[i] != '/' && charete[i] != ';' && charete[i] != '=' && !int.TryParse(charete[i].ToString(), out valor))
                {
                    termino.Add(Letras(charete, i).Item1);
                    Analizado.Add(Palabra_clave(Letras(charete,i).Item1));
                    i = Letras(charete, i).Item2;
                }
                else if (charete[i] != ' ' && !(int.TryParse(charete[i].ToString(), out valor)))
                {
                    termino.Add(charete[i].ToString());
                    Analizado.Add(Simbolos(charete[i]));
                }
                else if (int.TryParse(charete[i].ToString(), out valor))
                {
                    termino.Add(Digito(charete, i).Item1);
                    Analizado.Add("numero");
                    i = Digito(charete, i).Item2;
                }
               
            }
            return new Tuple<List<string>, List<string>> (termino, Analizado);
        }
        public static void Imprimir_Lista (List<string> termino, List<string> Analizado)
        {
            for(int i = 0; i < termino.Count; i++)
            {
                Console.Write(termino[i] + " " + Analizado[i]);
                Console.WriteLine();
            }
        }
        public static string Palabra_clave (string clave)
        {
            if(clave == "let")
            {
                return "entrada";
            }
            else if (clave == "print")
            {
               return "imprimir_en_consola";
            }
            else if (clave == "function")
            {
                return "definir_funcion";
            }
            else if (clave == "if")
            {
                return "condicional";
            }
            else if (clave == "else")
            {
                return "alternativa_condicional";
            }
            else if (clave == "in")
            {
                return "existencia+_en_ambito";
            }
            return "indentificador";
        }
        public static Tuple<string, int> Letras(char[] charete, int posicion)
        {
            int valor = 0;
            string temporal = "";
            for (int j = posicion; j < charete.Length; j++)
            {
                if (charete[j] != '>' && charete[j] != '<' && charete[j] != ',' && charete[j] != ' ' && charete[j] != '(' && charete[j] != ')' && charete[j] != '+' && charete[j] != '-' && charete[j] != '*' && charete[j] != '^' && charete[j] != '/' && charete[j] != ';' && charete[j] != '=' && !int.TryParse(charete[j].ToString(), out valor))
                {
                    temporal += charete[j].ToString();
                }
                else
                {
                    posicion = j - 1;
                    break;
                }
                if (j == charete.Length - 1)
                {
                    posicion = j;
                    break;
                }
            }
            return new Tuple<string, int>(temporal, posicion);
        }
        public static Tuple<string,int> Digito (char[] charete, int posicion)
        {
            int valor = 0;
            string temporal = "";
            for (int j = posicion; j < charete.Length; j++)
            {
                if (int.TryParse(charete[j].ToString(), out valor))
                {
                    temporal += charete[j].ToString();
                }
                else
                {
                    posicion = j - 1;
                    break;
                }
                if (j == charete.Length - 1)
                {
                    posicion = j;
                    break;
                }
            }
            return new Tuple<string, int>(temporal, posicion);
        }
        public static string Simbolos (char charete)
        {
            string termino = "";
            if (charete == '+')
            {
                termino = "adicion";
            }
            else if (charete ==  '-')
            {
                termino = "sustraccion";
            }
            else if (charete == '*')
            {
                termino = "multiplicacion";
            }
            else if (charete == '/')
            {
                termino = "division";
            }
            else if (charete == '^')
            {
                termino = "potenciacion";
            }
            else if (charete == ';')
            {
                termino = "cierre";
            }
            else if (charete == '=')
            {
                termino = "igual";
            }
            else if (charete == '(')
            {
                termino = "parentesis_inicial";
            }
            else if (charete == ')')
            {
                termino = "parentesis_final";
            }
            else if (charete == ',')
            {
                termino = "coma";
            }
            else if (charete == '>')
            {
                termino = "mayor_que";
            }
            else if (charete == '<')
            {
                termino = "menor_que";
            }
            return termino;
        }
        public static void Imprimir_Diccionario (Dictionary<int,string> Analizado)
        {
            foreach(int posicion in Analizado.Keys)
            {
                Console.WriteLine(Analizado[posicion]);
            }
        }
    }
}
