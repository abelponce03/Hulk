namespace Hulk;

// Esta es la clase que le va a dar las propiedades al token
enum Tipo_De_Token
    {
        Numero,
        Suma,
        Resta,
        Producto,
        Division,
        Parentesis_Abierto,
        Parentesis_Cerrado,
        Espacio,
        Malo,
        Final,
        Identificador,

        //Keywords
        True_Keyword,
        False_Keyword,
        
        //Expresiones
        Parentesis,
        Expresion_Unaria,
        Expresion_Binaria,
        Literal,
        
    }