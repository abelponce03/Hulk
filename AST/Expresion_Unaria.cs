namespace Hulk;
sealed class Expresion_Unaria : Expresion
{
    public override Tipo_De_Token Tipo => Tipo_De_Token.Expresion_Unaria;
    public Token Operador { get; }
    public Expresion Right { get; }
    public Expresion_Unaria(Token operador, Expresion right)
    {
        Operador = operador;
        Right = right;
    }
}