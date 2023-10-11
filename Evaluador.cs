using System.Runtime.InteropServices;

namespace Hulk;
class Evaluador
{
    private readonly Expresion _rama;
    public Evaluador(Expresion rama)
    {
        this._rama = rama;
    }
    public object Evaluar(Dictionary<string, Expresion> Variables)
    {
        return Evaluar_Expresion(_rama, Variables);
    }
    private object Evaluar_Expresion(Expresion nodo, Dictionary<string, Expresion> Variables)
    {
        if (nodo is Literal n) return n.Valor;

        if (nodo is Let l)
        {
            var asignar = Evaluar_Expresion(l.Asignar, Variables);
            var IN = Evaluar_Expresion(l._IN, Variables);
            return IN;
        }

        if (nodo is In h)
        {
            var valor = Evaluar_Expresion(h._expresion, Variables);
            return valor;
        }

        if (nodo is Print t)
        {
            var valor = Evaluar_Expresion(t._expresion, Variables);
            return valor;
        }

        if (nodo is Variable v)
        {
            var key = Variables[v.Identificador.Texto];
            var valor = Evaluar_Expresion(key, Variables);
            return valor;
        }

        if (nodo is Asignacion a)
        {
            if (!Variables.ContainsKey(a.Identificador.Texto))
            {
                var valor = Evaluar_Expresion(a._expresion, Variables);
                Variables.Add(a.Identificador.Texto, a._expresion);
                return valor;
            }
            else
            {
                var valor = Evaluar_Expresion(a._expresion, Variables);
                Variables[a.Identificador.Texto] = a._expresion;
                return valor;
            }

        }
        if (nodo is IF i)
        {
            var condicion = Evaluar_Expresion(i.Condicion, Variables);
            var valor = Evaluar_Expresion(i._expresion, Variables);
            var _else = Evaluar_Expresion(i._Else, Variables);
            if ((bool)condicion) return valor;
            else return _else;
        }
        if (nodo is Else e)
        {
            var valor = Evaluar_Expresion(e._expresion, Variables);
            return valor;
        }

        if (nodo is Expresion_Unaria u)
        {
            var right = Evaluar_Expresion(u.Right, Variables);

            switch (u.Operador.Tipo)
            {
                case Tipo_De_Token.Suma: return (double)right;

                case Tipo_De_Token.Resta: return -(double)right;

                case Tipo_De_Token.Bang: return !(bool)right;

                default: throw new Exception($"Expresion unaria inesperada:{u.Operador.Tipo}");
            }
        }
        if (nodo is Expresion_Binaria b)
        {
            var left = Evaluar_Expresion(b.Left, Variables);
            var right = Evaluar_Expresion(b.Right, Variables);

            switch (b.Operador.Tipo)
            {
                case Tipo_De_Token.Suma: return (double)left + (double)right;

                case Tipo_De_Token.Resta: return (double)left - (double)right;

                case Tipo_De_Token.Producto: return (double)left * (double)right;

                case Tipo_De_Token.Division: return (double)left / (double)right;

                case Tipo_De_Token.Potenciacion: return Math.Pow((double)left, (double)right);

                case Tipo_De_Token.AmpersandAmpersand: return (bool)left && (bool)right;

                case Tipo_De_Token.PipePipe: return (bool)left || (bool)right;

                case Tipo_De_Token.Menor_que: return (double)left < (double)right;

                case Tipo_De_Token.Mayor_que: return (double)left > (double)right;

                case Tipo_De_Token.IgualIgual: return Equals(left, right);

                case Tipo_De_Token.Bang_Igual: return !Equals(left, right);

                default: throw new Exception($"Operador inesperado: {b.Operador.Tipo}");
            }
        }

        if (nodo is Parentesis p) return Evaluar_Expresion(p.Expresion, Variables);

        throw new Exception($"Nodo inesperado: {nodo.Tipo}");
    }
}