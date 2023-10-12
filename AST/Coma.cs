namespace Hulk;
sealed class Coma : Expresion
{
    public Coma(Token _Coma, Expresion expresion)
    {
        coma = _Coma;
        _expresion = expresion;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.coma_Expresion;
    public Token coma { get; }
    public Expresion _expresion { get; }
}