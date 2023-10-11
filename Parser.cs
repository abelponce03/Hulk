using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

namespace Hulk;
//Aqui a partir de los token qu saque en el analizador voy a
// ir viendo que es cada uno y que puedo hcer con ellos 
class Parser
{

    private readonly Token[] _tokens;// todos los tokens se guardan aqui
    private int _posicion;
    private List<string> errores = new List<string>();
    public Parser(string texto)
    {
        var tokens = new List<Token>();
        var Analizador = new Analizador_lexico(texto);
        Token token;
        do
        {
            token = Analizador.Proximo_Token();
            if (token.Tipo != Tipo_De_Token.Espacio && token.Tipo != Tipo_De_Token.Malo) tokens.Add(token);
        }
        while (token.Tipo != Tipo_De_Token.Final);
        _tokens = tokens.ToArray();
        errores.AddRange(Analizador.Error);
    }


    private Token Tomar(int offset)
    {
        int index = _posicion + offset;
        if (index >= _tokens.Length) return _tokens[_tokens.Length - 1];
        else return _tokens[index];
    }
    private Token Verificandose => Tomar(0);

    public Token Proximo_Token()
    {
        var token = Verificandose;
        _posicion++;
        return token;
    }
    public Token Match(Tipo_De_Token tipo)
    {
        if (Verificandose.Tipo == tipo) return Proximo_Token();
        errores.Add($"ERROR: Token incorrecto <{Verificandose.Tipo}>, se esperaba <{tipo}>");
        return new Token(tipo, Verificandose.Posicion, null, null);

    }
    public Arbol Parse()
    {
        var expresion = Parse_Expresion();
        var final = Match(Tipo_De_Token.Final);
        return new Arbol(errores, expresion, final);
    }
    public Expresion Parse_Expresion()
    {
        return Parse_Asignacion();
    }
    public Expresion Parse_Asignacion()
    {
        if (Tomar(0).Tipo == Tipo_De_Token.Identificador && Tomar(1).Tipo == Tipo_De_Token.Igual)
        {
            var identificador = Proximo_Token();
            var operador = Proximo_Token();
            var right = Parse_Asignacion();
            return new Asignacion(identificador, operador, right);
        }
        return Parse_Expresion_Binaria();
    }
    private Expresion Parse_Expresion_Binaria(int parentPrecedence = 0)
    {
        Expresion left;
        var expresion_unaria = Verificandose.Tipo.Prioridad_Operadores_Unarios();

        if (expresion_unaria != 0 && expresion_unaria >= parentPrecedence)
        {
            var operador = Proximo_Token();
            var right = Parse_Expresion_Binaria(expresion_unaria);
            left = new Expresion_Unaria(operador, right);
        }
        else left = Parseo_Fundamental_Expresion();

        while (true)
        {
            var precedence = Verificandose.Tipo.Prioridad_Operadores_Binarios();
            if (precedence == 0 || precedence <= parentPrecedence) break;

            var operador = Proximo_Token();
            var right = Parse_Expresion_Binaria(precedence);
            left = new Expresion_Binaria(left, operador, right);
        }
        return left;
    }
    private Expresion Parseo_Fundamental_Expresion()
    {
        switch (Verificandose.Tipo)
        {
            case Tipo_De_Token.Parentesis_Abierto:
                {
                    var left = Proximo_Token();
                    var expresion = Parse_Expresion();
                    var right = Match(Tipo_De_Token.Parentesis_Cerrado);
                    return new Parentesis(left, expresion, right);
                }
            case Tipo_De_Token.True_Keyword:
            case Tipo_De_Token.False_Keyword:
                {
                    var keyword = Proximo_Token();
                    var valor = keyword.Tipo == Tipo_De_Token.True_Keyword;
                    return new Literal(keyword, valor);
                }
            case Tipo_De_Token.Identificador:
                {
                    var identificador = Proximo_Token();
                    return new Variable(identificador);
                }
            case Tipo_De_Token.if_Keyword:
                {
                    var keyword = Proximo_Token();
                    var parentesis = Parse_Expresion();
                    if(parentesis.Tipo != Tipo_De_Token.Parentesis)
                    errores.Add($"ERROR: Se esperaba expresion con parentesis {parentesis.Tipo}");
                    var expresion = Parse_Expresion();
                    var _else = Parse_Expresion();
                    if(_else.Tipo != Tipo_De_Token.else_Expresion)  
                    errores.Add($"ERROR: Token incorrecto <{_else.Tipo}>, se esperaba else_Expresion");
                    return new IF(keyword, parentesis, expresion, _else);  
                }
            case Tipo_De_Token.else_Keyword:
                {
                    var keyword = Proximo_Token();
                    var expresion = Parse_Expresion();
                    return new Else(keyword, expresion);
                }
            case Tipo_De_Token.print_Keyword:
                {
                    var keyword = Proximo_Token();
                    var expresion = Parse_Expresion();
                    if(expresion.Tipo != Tipo_De_Token.Parentesis)
                    errores.Add($"ERROR: Se esperaba expresion con parentesis {expresion.Tipo}");
                    return new Print(keyword, expresion);
                }
            case Tipo_De_Token.String:
                {
                    var keyword = Proximo_Token();
                    var valor = keyword.Texto;
                    return new Literal(keyword, valor);
                }
            case Tipo_De_Token.let_Keyword:
                {
                   var keyword = Proximo_Token();
                   var asignar = Parse_Expresion();
                   if(asignar.Tipo != Tipo_De_Token.Asignacion)
                    errores.Add($"ERROR: Se esperaba una asignacion de variable {asignar.Tipo}");
                   var IN = Parse_Expresion();
                   if(IN.Tipo != Tipo_De_Token.in_Expresion)
                    errores.Add($"ERROR: Se esperaba una declaracion de contexto {IN.Tipo}");
                   return new Let(keyword, asignar, IN);
                }
            case Tipo_De_Token.in_Keyword:
                {
                    var keyword = Proximo_Token();
                    var expresion = Parse_Expresion();
                    return new In(keyword, expresion);
                }                
            default:
                {
                    var token_num = Match(Tipo_De_Token.Numero);
                    var valor = token_num.Valor;
                    return new Literal(token_num, valor);
                }
        }
    }
}