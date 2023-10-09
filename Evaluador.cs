namespace Hulk;
class Evaluador
{ 
    private readonly Expresion _rama;
    public Evaluador(Expresion rama)
    {
       this._rama = rama;
    }
    public int Evaluar()
    {
        return Evaluar_Expresion(_rama);
    }
    private int Evaluar_Expresion(Expresion nodo)
    {
        //binaria
        //numero
        if(nodo is Literal n) return (int) n._Literal.Valor;

        if(nodo is Expresion_Unaria u)
        {
            var right = Evaluar_Expresion(u.Right);
            if(u.Operador.Tipo == Tipo_De_Token.Suma) return right;

            else if(u.Operador.Tipo == Tipo_De_Token.Resta) return -right;

            else throw new Exception($"Expresion unaria inesperada:{u.Operador.Tipo}");
            
        }
        if(nodo is Expresion_Binaria b)
        {
            var left = Evaluar_Expresion(b.Left);
            var right = Evaluar_Expresion(b.Right);
            if(b.Operador.Tipo == Tipo_De_Token.Suma) return left + right;
            else if(b.Operador.Tipo == Tipo_De_Token.Resta) return left - right;
            else if(b.Operador.Tipo == Tipo_De_Token.Producto) return left * right;
            else if(b.Operador.Tipo == Tipo_De_Token.Division) return left / right;
            else throw new Exception($"Operador inesperado: {b.Operador.Tipo}");
        }

        if(nodo is Parentesis p) return Evaluar_Expresion(p.Expresion);
        
        throw new Exception ($"Nodo inesperado: {nodo.Tipo}");
    }
}