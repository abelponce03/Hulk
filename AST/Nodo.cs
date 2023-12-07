namespace Hulk;
//clase abstracta de la cual todos van a ir sobreescribiendo el tipo
abstract class Nodo
{
    public abstract Tipo_De_Token Tipo { get; }
}