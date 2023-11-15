namespace Hulk;

sealed class In : Expresion
{
    public In (  Expresion expresion)
    {
        _expresion = expresion;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.in_Expresion;

    public Expresion _expresion {get;}
}