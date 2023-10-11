namespace Hulk;

// Esta es la clase que le va a dar las propiedades al token
enum Tipo_De_Token
{
    //tokens
    Numero,
    Suma,
    Resta,
    Producto,
    Division,
    Potenciacion,
    Parentesis_Abierto,
    Parentesis_Cerrado,
    Bang,
    Igual,
    AmpersandAmpersand,
    PipePipe,
    Menor_que,
    Mayor_que,
    IgualIgual,
    Bang_Igual,
    Espacio,
    Malo,
    Final,
    Identificador,

    //Keywords
    True_Keyword,
    False_Keyword,
    if_Keyword,
    else_Keyword,
    print_Keyword,

    //Expresiones
    Parentesis,
    Expresion_Unaria,
    Expresion_Binaria,
    Literal,
    Variable,
    Asignacion,
    if_Expresion,
    else_Expresion,
    Print_Expresion,

}