namespace Hulk;

sealed class Let_in : Expresion
{
    public Let_in (Expresion let, Expresion IN)
    {
        _Let = let;
        _IN = IN;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.let_in_Expresion;
    public Expresion _Let {get;}
    public Expresion _IN {get;}
}