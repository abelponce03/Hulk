using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hulk;
class Evaluador
{
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
    private object Evaluar_Expresion(Expresion nodo)
    {
        Contador++;
        if (Contador > 1000)
        {
            throw new Exception("! OVERFLOW ERROR : Hulk Stack overflow");
        }

        if (nodo is Literal n) return n.Valor;

        if (nodo is LLamada_Funcion f)
        {
            if (!Biblioteca.Functions.ContainsKey(f.Nombre))
            {
                throw new Exception($"! FUNCTION ERROR : Function {f.Nombre} is not defined");
            }
            var Declaracion_Funcion = Biblioteca.Functions[f.Nombre];
            if (Declaracion_Funcion.Parametros.Count != f.Parametros.Count)
            {
                throw new Exception($"! FUNCTION ERROR : Function {f.Nombre} does not have {f.Parametros.Count} parameters but have {Biblioteca.Functions[f.Nombre].Parametros.Count} parameters");
            }
            var parameters = f.Parametros;
            var arguments = Declaracion_Funcion.Parametros;

            foreach (var (arg, param) in arguments.Zip(parameters))
            {
                var evaluatedParameter = Evaluar_Expresion(param);
            }
            return Evaluar_Expresion(Declaracion_Funcion.Cuerpo);
        }

        if (nodo is Sen x)
        {
            var expresion = Evaluar_Expresion(x._expresion);
            var valor = Math.Sin((double)expresion);
            return valor;
        }
        if (nodo is Cos y)
        {
            var expresion = Evaluar_Expresion(y._expresion);
            var valor = Math.Cos((double)expresion);
            return valor;
        }

        if (nodo is Let_in m)
        {
            var let = Evaluar_Expresion(m._Let);
            var _in = Evaluar_Expresion(m._IN);
            return _in;
        }

        if (nodo is Let l)
        {
            if (!Biblioteca.Variables.ContainsKey(l.Identificador.Texto))
            {
                var valor = Evaluar_Expresion(l.Asignacion);
                Biblioteca.Variables.Add(l.Identificador.Texto, l.Asignacion);
                if (l._Let_expresion is null)
                {
                    return valor;
                }
                return Evaluar_Expresion(l._Let_expresion);
            }
            else
            {
                var valor = Evaluar_Expresion(l.Asignacion);
                Biblioteca.Variables[l.Identificador.Texto] = l.Asignacion;
                if (l._Let_expresion is null)
                {
                    return valor;
                }
                return Evaluar_Expresion(l._Let_expresion);
            }

        }

        if (nodo is In h)
        {
            var valor = Evaluar_Expresion(h._expresion);
            return valor;
        }

        if (nodo is Print t)
        {
            var valor = Evaluar_Expresion(t._expresion);
            Console.WriteLine(valor);
            return valor;
        }

        if (nodo is Variable v)
        {
            if (!Biblioteca.Variables.ContainsKey(v.Identificador.Texto)) throw new Exception($"! SEMANTIC ERROR : Undefine variable {v.Identificador.Texto}");
            var key = Biblioteca.Variables[v.Identificador.Texto];
            var valor = Evaluar_Expresion((Expresion)key);
            return valor;
        }

        if (nodo is Logaritmo r)
        {
            var expresion = Evaluar_Expresion(r._expresion);
            var valor = Math.Log((double)expresion);
            return valor;
        }

        if (nodo is IF i)
        {
            var condicion = Evaluar_Expresion(i.Condicion);
            var valor = Evaluar_Expresion(i._expresion);
            var _else = Evaluar_Expresion(i._Else);
            if (condicion.GetType() != typeof(bool)) throw new Exception("! SEMANTIC ERROR : If-ELSE expressions must have a boolean condition");
            else if ((bool)condicion) return valor;
            else return _else;
        }
        if (nodo is Else e)
        {
            var valor = Evaluar_Expresion(e._expresion);
            return valor;
        }

        if (nodo is Expresion_Unaria u)
        {
            var right = Evaluar_Expresion(u.Right);

            switch (u.Operador.Tipo)
            {
                case Tipo_De_Token.Suma: return (double)right;

                case Tipo_De_Token.Resta: return -(double)right;

                case Tipo_De_Token.Bang: return !(bool)right;

                default: throw new Exception($"! SEMANTIC ERROR : Invalid unary operator {u.Operador.Tipo}");
            }
        }
        if (nodo is Expresion_Binaria b)
        {
            var left = Evaluar_Expresion(b.Left);
            var right = Evaluar_Expresion(b.Right);

            switch (b.Operador.Tipo)
            {
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
                        if ((double)right == 0) throw new Exception("! SEMANTIC ERROR : Cannot divide by zero");
                        else return (double)left / (double)right;
                    }

                case Tipo_De_Token.Potenciacion:
                    {
                        Verificar_tipos(b, left, right);
                        if ((double)left == 0 & (double)right == 0) throw new Exception($"! SEMANTIC ERROR : {left} pow to {right} is not defined");
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
                default: throw new Exception($"! SEMANTIC ERROR : Unexpected binary operator {b.Operador.Tipo}");
            }
        }

        if (nodo is Parentesis p) return Evaluar_Expresion(p.Expresion);

        throw new Exception($"! SYNTAX ERROR : Unexpected node {nodo}");
    }
    private static void Verificar_tipos(Expresion_Binaria b, object left, object right)
    {
        if (left.GetType() != right.GetType())
        {
            throw new Exception($"! SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType().Name} with {right.GetType().Name} using {b.Operador.Texto}");
        }
    }
}