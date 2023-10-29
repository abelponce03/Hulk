namespace Hulk;
class Declaracion_Funcion: Expresion
{
    public Declaracion_Funcion(string nombre, List<string> parametros, Expresion cuerpo)
    {
        Nombre = nombre;
        Parametros = parametros;
        Cuerpo = cuerpo;
    }
    public override Tipo_De_Token Tipo => Tipo_De_Token.Declaracion_Funcion;
    public string Nombre { get; }
    public List<string> Parametros { get; }
    public Expresion Cuerpo { get; set;}
    
}