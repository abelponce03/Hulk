using System.Runtime.InteropServices;

namespace Hulk;
class Evaluador
{ 
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
        if(nodo is Literal n) return n.Valor;

        if(nodo is Expresion_Unaria u)
        {
            var right = Evaluar_Expresion(u.Right);

            switch(u.Operador.Tipo)
            {   
                case Tipo_De_Token.Suma: return (double) right;
                
                case Tipo_De_Token.Resta: return -(double) right;

                case Tipo_De_Token.Bang: return !(bool) right;

                default: throw new Exception($"Expresion unaria inesperada:{u.Operador.Tipo}");
            }            
        }
        if(nodo is Expresion_Binaria b)
        {
            var left =  Evaluar_Expresion(b.Left);
            var right = Evaluar_Expresion(b.Right);

            switch(b.Operador.Tipo)
            {
                case Tipo_De_Token.Suma: return (double) left + (double) right;

                case Tipo_De_Token.Resta: return (double) left - (double) right;

                case Tipo_De_Token.Producto: return (double) left * (double) right;

                case Tipo_De_Token.Division: return (double) left / (double) right;

                case Tipo_De_Token.Potenciacion: return Math.Pow((double) left, (double) right);

                case Tipo_De_Token.AmpersandAmpersand: return (bool) left && (bool) right;

                case Tipo_De_Token.PipePipe: return (bool) left || (bool) right;

                case Tipo_De_Token.Menor_que: return (double) left < (double) right;

                case Tipo_De_Token.Mayor_que: return (double) left > (double) right;

                case Tipo_De_Token.IgualIgual: return Equals(left,right);

                case Tipo_De_Token.Bang_Igual: return !Equals(left,right);

                default: throw new Exception($"Operador inesperado: {b.Operador.Tipo}");
            }
        }

        if(nodo is Parentesis p) return Evaluar_Expresion(p.Expresion);
        
        throw new Exception ($"Nodo inesperado: {nodo.Tipo}");
    }
}