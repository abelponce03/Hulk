namespace Hulk;

class Let : Expresion
{
    public Let(Token identificador, Expresion asignacion) : this(identificador, asignacion, null!)
    {

    }
    public Let(Token identificador, Expresion asignacion, Let let_expresion)
    {
        Identificador = identificador;
        Asignacion = asignacion;
        _Let_expresion = let_expresion;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.let_Expresion;
    public Token Identificador { get; }
    public Expresion Asignacion { get; }
    public Let _Let_expresion {get;}
}