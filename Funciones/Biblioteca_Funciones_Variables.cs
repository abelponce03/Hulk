namespace Hulk;
class Biblioteca
{
    //diccionario donde se guardan las funciones
    public static Dictionary<string, Declaracion_Funcion> Functions = new Dictionary<string, Declaracion_Funcion>();
    //diccionario donde se guardan las variables
    public static Dictionary<string, object> Variables = new Dictionary<string, object>();
    //Pila para recursividad de funciones
    public static Stack<Dictionary<string, object>> Pila = new Stack<Dictionary<string, object>>();

}