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
        switch (nodo)
        {
            case Literal a: return a.Valor;

            case Expresion_Binaria b: return Evaluar_Expresion_Binaria(b);

            case Expresion_Unaria c: return Evaluar_Expresion_Unaria(c);

            case LLamada_Funcion d: return Evaluar_Llamada_Funcion(d);

            case Sen e: return Evaluar_Sen(e);

            case Cos f: return Evaluar_Cos(f);

            case Let_in g: return Evaluar_Let_in(g);

            case Let h: return Evaluar_Let(h);

            case In i: return Evaluar_Expresion(i._expresion);

            case Print j: return Evaluar_Expresion(j._expresion);

            case Variable k: return Evaluar_Variable(k);

            case Logaritmo l: return Evaluar_Logaritmo(l);

            case IF m: return Evaluar_If(m);

            case Else n: return Evaluar_Expresion(n._expresion);

            case Parentesis l: return Evaluar_Expresion(l.Expresion);

            default: throw new Exception($"! SYNTAX ERROR : Unexpected node <{nodo}>");
        }

    }
    private static void Verificar_tipos(Expresion_Binaria b, object left, object right)
    {
        if (left.GetType() != right.GetType())
        {
            throw new Exception($"! SEMANTIC ERROR : Invalid expression: Can't operate <{left.GetType().Name}> with <{right.GetType().Name}> using <{b.Operador.Texto}>");
        }
    }

    object Evaluar_Llamada_Funcion(LLamada_Funcion f)
    {
        if (!Biblioteca.Functions.ContainsKey(f.Nombre))
        {
            throw new Exception($"! FUNCTION ERROR : Function <{f.Nombre}> is not defined");
        }
        var Declaracion_Funcion = Biblioteca.Functions[f.Nombre];
        if (Declaracion_Funcion.Parametros.Count != f.Parametros.Count)
        {
            throw new Exception($"! FUNCTION ERROR : Function <{f.Nombre}> does not have <{f.Parametros.Count}> parameters but has <{Biblioteca.Functions[f.Nombre].Parametros.Count}> parameters");
        }
        var parameters = f.Parametros;
        var arguments = Declaracion_Funcion.Parametros;

        for (int i = 0; i < parameters.Count; i++)
        {

            if (!Biblioteca.Variables.ContainsKey(arguments[i]))
            {
                Biblioteca.Variables.Add(arguments[i], Evaluar_Expresion(parameters[i]));
            }
            else
            {
                Biblioteca.Variables[arguments[i]] = Evaluar_Expresion(parameters[i]);
            }
        }

        return Evaluar_Expresion(Declaracion_Funcion.Cuerpo);
    }

    object Evaluar_Sen(Sen x)
    {
        var expresion = Evaluar_Expresion(x._expresion);
        var valor = Math.Sin((double)expresion);
        return valor;
    }

    object Evaluar_Cos(Cos x)
    {
        var expresion = Evaluar_Expresion(x._expresion);
        var valor = Math.Cos((double)expresion);
        return valor;
    }
    object Evaluar_Let_in(Let_in m)
    {
        var let = Evaluar_Expresion(m._Let);
        var _in = Evaluar_Expresion(m._IN);
        return _in;
    }

    object Evaluar_Let(Let l)
    {
        if (!Biblioteca.Variables.ContainsKey(l.Identificador.Texto))
        {
            var valor = Evaluar_Expresion(l.Asignacion);
            Biblioteca.Variables.Add(l.Identificador.Texto, valor);
            if (l._Let_expresion is null)
            {
                return valor;
            }
            return Evaluar_Expresion(l._Let_expresion);
        }
        else
        {
            var valor = Evaluar_Expresion(l.Asignacion);
            Biblioteca.Variables.Remove(l.Identificador.Texto);
            Biblioteca.Variables.Add(l.Identificador.Texto, valor);
            if (l._Let_expresion is null)
            {
                return valor;
            }
            return Evaluar_Expresion(l._Let_expresion);
        }
    }
    object Evaluar_Variable(Variable v)
    {
        if (!Biblioteca.Variables.ContainsKey(v.Identificador.Texto)) throw new Exception($"! SEMANTIC ERROR : Undefine variable <{v.Identificador.Texto}>");
        var valor = Biblioteca.Variables[v.Identificador.Texto];
        return valor;
    }
    object Evaluar_Logaritmo(Logaritmo r)
    {
        var expresion = Evaluar_Expresion(r._expresion);
        var valor = Math.Log((double)expresion);
        return valor;
    }
    object Evaluar_If(IF i)
    {
        var condicion = Evaluar_Expresion(i.Condicion);
        if (condicion.GetType() != typeof(bool)) throw new Exception("! SEMANTIC ERROR : If-ELSE expressions must have a boolean condition");

        var valor = Evaluar_Expresion(i._expresion);
        if ((bool)condicion) return valor;
        else
        {
            var _else = Evaluar_Expresion(i._Else);
            return _else;
        }
    }
    object Evaluar_Expresion_Unaria(Expresion_Unaria u)
    {
        var right = Evaluar_Expresion(u.Right);

        switch (u.Operador.Tipo)
        {
            case Tipo_De_Token.Suma: return (double)right;

            case Tipo_De_Token.Resta: return -(double)right;

            case Tipo_De_Token.Bang: return !(bool)right;

            default: throw new Exception($"! SEMANTIC ERROR : Invalid unary operator <{u.Operador.Tipo}>");
        }
    }
    object Evaluar_Expresion_Binaria(Expresion_Binaria b)
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
}