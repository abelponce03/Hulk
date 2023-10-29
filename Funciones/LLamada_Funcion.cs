namespace Hulk;
class LLamada_Funcion : Expresion
{
    public LLamada_Funcion(string nombre, List<Expresion> parametros)
    {
        Nombre = nombre;
        Parametros = parametros;
    }

    public override Tipo_De_Token Tipo => Tipo_De_Token.LLamada_Funcion;
    public string Nombre { get; }
    public List<Expresion> Parametros { get; }
}
