using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hulk;
class Evaluador
{
    //contador para el stackoverflow
    private int Contador;
    private readonly Expresion _rama;
    public Evaluador(Expresion rama)
    {
        this._rama = rama;
    }
    public object Evaluar()
    {
        return Evaluar_Expresion(_rama);
    }
    object Evaluar_Expresion(Expresion nodo)
    {
        Contador++;
        if (Contador > 1000)
        {
            throw new Exception("! OVERFLOW ERROR : Hulk Stack overflow");
        }
        // si es un indentificador y busca en su diccionario correspondiente, si es un numero retorna su valor
        if (nodo is Literal a)
        {
            if (a._Literal.Tipo == Tipo_De_Token.Identificador)
            {
                if (!Biblioteca.Variables.ContainsKey(a._Literal.Texto) && Biblioteca.Pila.Count == 0) throw new Exception($"! SEMANTIC ERROR : Variable <{a._Literal.Texto}> is not defined");
                if (Biblioteca.Pila.Count == 0) return Biblioteca.Variables[a._Literal.Texto];
                return Biblioteca.Pila.Peek()[a._Literal.Texto];
            }
            return a.Valor;
        }
        // evaluar izquierda y derecha y ver el operador de la expresion binaria 
        if (nodo is Expresion_Binaria b)
        {
            var left = Evaluar_Expresion(b.Left);
            var right = Evaluar_Expresion(b.Right);

            switch (b.Operador.Tipo)
            {
                case Tipo_De_Token.resto:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left % (double)right;
                    }
                case Tipo_De_Token.concatenacion:
                    {
                        Verificar_tipos(b, left, right);
                        return (string)left + (string)right;
                    }
                case Tipo_De_Token.Suma:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left + (double)right;
                    }

                case Tipo_De_Token.Resta:
                    {

                        Verificar_tipos(b, left, right);
                        return (double)left - (double)right;
                    }

                case Tipo_De_Token.Producto:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left * (double)right;
                    }

                case Tipo_De_Token.Division:
                    {
                        Verificar_tipos(b, left, right);
                        if ((double)right == 0) throw new Exception($"! SEMANTIC ERROR : Cannot divide <{left}> by <{right}>");
                        else return (double)left / (double)right;
                    }

                case Tipo_De_Token.Potenciacion:
                    {
                        Verificar_tipos(b, left, right);
                        if ((double)left == 0 && (double)right == 0) throw new Exception($"! SEMANTIC ERROR : <{left}> pow to <{right}> is not defined");
                        else return Math.Pow((double)left, (double)right);
                    }

                case Tipo_De_Token.AmpersandAmpersand:
                    {
                        Verificar_tipos(b, left, right);
                        return (bool)left && (bool)right;
                    }

                case Tipo_De_Token.PipePipe:
                    {
                        Verificar_tipos(b, left, right);
                        return (bool)left || (bool)right;
                    }

                case Tipo_De_Token.Menor_que:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left < (double)right;
                    }
                case Tipo_De_Token.Menor_igual_que:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left <= (double)right;
                    }
                case Tipo_De_Token.Mayor_que:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left > (double)right;
                    }
                case Tipo_De_Token.Mayor_igual_que:
                    {
                        Verificar_tipos(b, left, right);
                        return (double)left >= (double)right;
                    }
                case Tipo_De_Token.IgualIgual:
                    {
                        Verificar_tipos(b, left, right);
                        return Equals(left, right);
                    }
                case Tipo_De_Token.Bang_Igual:
                    {
                        Verificar_tipos(b, left, right);
                        return !Equals(left, right);
                    }
                default: throw new Exception($"! SEMANTIC ERROR : Unexpected binary operator <{b.Operador.Tipo}>");
            }
        }
        //evaluar expresion a la derecha y ver el operador de la expresion
        if (nodo is Expresion_Unaria c)
        {
            var right = Evaluar_Expresion(c.Right);

            switch (c.Operador.Tipo)
            {
                case Tipo_De_Token.Suma: return (double)right;

                case Tipo_De_Token.Resta: return -(double)right;

                case Tipo_De_Token.Bang: return !(bool)right;

                default: throw new Exception($"! SEMANTIC ERROR : Invalid unary operator <{c.Operador.Tipo}>");
            }
        }
        
        if (nodo is LLamada_Funcion d)
        {
            //verificar que se encuentre declarada esa funcion
            if (!Biblioteca.Functions.ContainsKey(d.Nombre))
            {
                throw new Exception($"! FUNCTION ERROR : Function <{d.Nombre}> is not defined");
            }
            // tomar la declaracion de funcion segun el identificador
            var Declaracion_Funcion = Biblioteca.Functions[d.Nombre];
            //ver si concuerda el numero de parametros
            if (Declaracion_Funcion.Parametros.Count != d.Parametros.Count)
            {
                throw new Exception($"! FUNCTION ERROR : Function <{d.Nombre}> does not have <{d.Parametros.Count}> parameters but has <{Biblioteca.Functions[d.Nombre].Parametros.Count}> parameters");
            }
            //crear diccionario temporal para asi crear un ambito local de variable mas adelante
            var temp = new Dictionary<string, object>();

            var parametros = d.Parametros;
            var argumentos = Declaracion_Funcion.Parametros;

            for (int i = 0; i < parametros.Count; i++)
            {
                var id = argumentos[i];
                var expresion = Evaluar_Expresion(parametros[i]);
                temp.Add(id, expresion);
            }
            //vamos a ir anadiendo a un stack 
            Biblioteca.Pila.Push(temp);
            
            //se evaluan con los valores de la variable temporal
            var valor = Evaluar_Expresion(Declaracion_Funcion.Cuerpo);
            
            //luego se van sacando estos diccionarios hasta quedarse con el valor original de la variable
            Biblioteca.Pila.Pop();

            return valor;
        }
        //evaluacion de expresion seno
        if (nodo is Sen e)
        {
            var expresion = Evaluar_Expresion(e._expresion);
            var valor = Math.Sin((double)expresion);
            return valor;
        }
        //evaluacion de expresion cos
        if (nodo is Cos f)
        {
            var expresion = Evaluar_Expresion(f._expresion);
            var valor = Math.Cos((double)expresion);
            return valor;
        }
        //evaluacion de expresion let in
        if (nodo is Let_in g)
        {
            var let = Evaluar_Expresion(g._Let);
            var _in = Evaluar_Expresion(g._IN);
            return _in;
        }
        if (nodo is Let h)
        {
            var valor = Evaluar_Expresion(h.Asignacion);
            //asignar un valor a la variable y guardarlo en el diccionario
            Biblioteca.Variables[h.Identificador.Texto] = valor;
            //si la expresion let no tiene otra asignacion dentro de ella
            if (h._Let_expresion is null)
            {
                return valor;
            }
            return Evaluar_Expresion(h._Let_expresion);
        }
        //evaluacion del logaritmo
        if (nodo is Logaritmo x)
        {
            var expresion = Evaluar_Expresion(x._expresion);
            var valor = Math.Log((double)expresion);
            return valor;
        }
        
        if (nodo is IF j)
        {

            var condicion = Evaluar_Expresion(j.Condicion);
            //verificacion de tipos en condicion de if expresion
            if (condicion.GetType() != typeof(bool)) throw new Exception("! SEMANTIC ERROR : If-ELSE expressions must have a boolean condition");
            
            //si es true retorna el objeto de la expresion correspondite a la condicion
            if ((bool)condicion) return Evaluar_Expresion(j._expresion);

            else
            {
                //retornar el objeto de la expresion else
                return Evaluar_Expresion(j._Else);
            }
        }
       
        if (nodo is In k) return Evaluar_Expresion(k._expresion);

        if (nodo is Print l) return Evaluar_Expresion(l._expresion);

        if (nodo is Else m) return Evaluar_Expresion(m._expresion);

        if (nodo is Parentesis n) return Evaluar_Expresion(n.Expresion);

        throw new Exception($"! SYNTAX ERROR : Unexpected node <{nodo}>");

    }
    //verificacion de tipos para cada evaluacion de expresiones
    private static void Verificar_tipos(Expresion_Binaria b, object left, object right)
    {
        if (left.GetType() != right.GetType())
        {
            throw new Exception($"! SEMANTIC ERROR : Invalid expression: Can't operate <{left.GetType().Name}> with <{right.GetType().Name}> using <{b.Operador.Texto}>");
        }
    }
}