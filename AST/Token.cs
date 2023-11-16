namespace Hulk;

class Token : Nodo
{
    public override Tipo_De_Token Tipo { get; }
    public int Posicion { get; }
    public string Texto { get; }
    public object Valor { get; }

    public Token(Tipo_De_Token tipo, int posicion, string texto, object valor)
    {
        Tipo = tipo;
        Posicion = posicion;
        Texto = texto;
        Valor = valor;
    }
}