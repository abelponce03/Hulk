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
        int index = _posicion;
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
    private Expresion Parse_Expresion(int parentPrecedence = 0)
    {
        Expresion left;
        var expresion_unaria = Verificandose.Tipo.Prioridad_Operadores_Unarios();

        if (expresion_unaria != 0 && expresion_unaria >= parentPrecedence)
        {
            var operador = Proximo_Token();
            var right = Parse_Expresion(expresion_unaria);
            left = new Expresion_Unaria(operador, right);
        }
        else left = Parseo_Fundamental_Expresion();

        while (true)
        {
            var precedence = Verificandose.Tipo.Prioridad_Operadores_Binarios();
            if (precedence == 0 || precedence <= parentPrecedence) break;

            var operador = Proximo_Token();
            var right = Parse_Expresion(precedence);
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
            default:
                {
                    var token_num = Match(Tipo_De_Token.Numero);
                    var valor = token_num.Valor;
                    return new Literal(token_num, valor);
                }
        }
    }
}