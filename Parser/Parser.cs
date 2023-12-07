using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

namespace Hulk;
//Aqui a partir de los token qu saque en el analizador voy a
// ir viendo que es cada uno y que puedo hcer con ellos 
class Parser
{
    private readonly Token[] _tokens;// todos los tokens se guardan aqui
    private int _posicion;
    //lista donde se van a guardar errores que se vayan encontrando
    public List<string> errores = new List<string>();
    public Parser(string texto)
    {
        
        var tokens = new List<Token>();
        //se crea el objeto analizador lexico donde se va a convertir la entrada del usuario en tokens
        var Analizador = new Analizador_lexico(texto);
        Token token;
        do
        {
            token = Analizador.Proximo_Token();
            //si es un espacio, un token malo que sea considerado un error lexico
            if (token.Tipo != Tipo_De_Token.Espacio && token.Tipo != Tipo_De_Token.Malo && token.Tipo != Tipo_De_Token.Final) tokens.Add(token);
        }
        while (token.Tipo != Tipo_De_Token.Final);
        //todos los token creados se guardaran en el array de _tokens 
        _tokens = tokens.ToArray();
        //si el ultimo token no es un punto y coma devolver el error
        if (tokens.Count > 0 && tokens[_tokens.Length - 1].Tipo != Tipo_De_Token.punto_y_coma) errores.Add($"! SYNTAX ERROR : Expected in the end off line <{";"}> not <{tokens[_tokens.Length - 1].Texto}>");
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
    //verificar que el token que se analiza es el q tu necesitas
    public Token Match(Tipo_De_Token tipo)
    {
        if (Verificandose.Tipo == tipo) return Proximo_Token();
        else if (_tokens.Length == 1) errores.Add($"! SYNTAX ERROR : Not find <{tipo}> in <{_posicion}>");
        else throw new Exception($"! SYNTAX ERROR : Not find <{tipo}> after <{Tomar(-1).Texto}> in position <{_posicion}>");
        return new Token(tipo, Verificandose.Posicion, null, null);

    }
    //metodo que va a devolver el AST
    public Arbol Parse()
    {
        if(_tokens.Length == 0) return new Arbol(errores, null, null);
        var expresion = Parse_Expresion();
        var final = Match(Tipo_De_Token.punto_y_coma);
        //error de contexto ya que despues del punto y coma lo demas no se tiene en cuenta
        if (_posicion -1 < _tokens.Length - 1)
        {
            string fragmento = "";
            for(int i = _posicion; i < _tokens.Length; i++)
            {
                if(i == _tokens.Length - 1)
                {
                    fragmento += _tokens[i].Texto;
                }
                else fragmento += _tokens[i].Texto + " ";  
            }
            errores.Add($"! SYNTAX ERROR : The expression <{fragmento}> does not exist in the curret context");
        }
        return new Arbol(errores, expresion, final);
    }
    //aqui se verifica si es una llamada de funcion
    public Expresion Parse_Expresion()
    {
        if (Verificandose.Tipo is Tipo_De_Token.function_Keyword)
        {
            return Parse_Declaracion_Funcion();
        }
        return Parse_Expresion_Binaria();
    }
    //parseo de declaracion de funciones
    private Declaracion_Funcion Parse_Declaracion_Funcion()
    {
        Match(Tipo_De_Token.function_Keyword);
        var nombre = Match(Tipo_De_Token.Identificador);
        var parametros = Parseo_parametros();
        Match(Tipo_De_Token.Implicacion);
        var cuerpo = Parse_Expresion();
        var declaracion_Funcion = new Declaracion_Funcion(nombre.Texto, parametros, cuerpo);
        
        //condicion para que no puedan crear funciones con el nombre de sen cos log
        if (!Biblioteca.Functions.ContainsKey(nombre.Texto) && errores.Count == 0 && nombre.Texto != "sen(x)" && nombre.Texto != "cos(x)" && nombre.Texto != "log(x)")
        {
            Biblioteca.Functions.Add(nombre.Texto, declaracion_Funcion);
        }
        else
        {
            errores.Add($"! FUNCTION ERROR : Function <{nombre.Texto}> is already defined");
        }

        return declaracion_Funcion;
    }
    //parseo de parametros de funciones
    public List<string> Parseo_parametros()
    {
        Match(Tipo_De_Token.Parentesis_Abierto);
        var parametros = new List<string>();
        //se verifica que ya no existan mas parametros
        if (Verificandose.Tipo is Tipo_De_Token.Parentesis_Cerrado)
        {
            Proximo_Token();
            return parametros;
        }
        parametros.Add(Verificandose.Texto);
        Proximo_Token();
        while (Verificandose.Tipo == Tipo_De_Token.coma)
        {
            Proximo_Token();
            if (Verificandose.Tipo is not Tipo_De_Token.Identificador)
            {
                errores.Add($"! SEMANTIC ERROR : Parameters must be a valid identifier");
            }
            if (parametros.Contains(Verificandose.Texto))
            {
                errores.Add($"! SEMANTIC ERROR : A parameter with the name <'{Verificandose.Texto}'> already exists insert another parameter name");

            }
            parametros.Add(Verificandose.Texto);
            Proximo_Token();
        }
        Proximo_Token();
        return parametros;
    }
    //parseo de llamdas de una funcion
    private Expresion Parse_LLamada_Funcion(string identificador)
    {
        Proximo_Token();
        var parametros = new List<Expresion>();

        Match(Tipo_De_Token.Parentesis_Abierto);

       //se creaba un bucle al dejar una llamada de funcion sin cerrar parentesis
        int evitar_bucle = 0;
        while (true)
        {

            if (Verificandose.Tipo == Tipo_De_Token.Parentesis_Cerrado)
            {
                break;
            }
            var expresion = Parse_Expresion();
            parametros.Add(expresion);
            if (Verificandose.Tipo == Tipo_De_Token.coma)
            {
                Proximo_Token();
            }
            if (evitar_bucle == _posicion)
            {
                errores.Add($"! SYNTAX ERROR : Expected <{")"}> in position <{_posicion}> after the expresion");
                break;
            }
            evitar_bucle = _posicion;
        }

        Match(Tipo_De_Token.Parentesis_Cerrado);

        return new LLamada_Funcion(identificador, parametros);
    }
    //distincion de un token identificador entre variable y llamada de funcion
    private Expresion Parse_Variable_O_LLamada_Funcion()
    {
        if (Verificandose.Tipo == Tipo_De_Token.Identificador
        && Tomar(1).Tipo == Tipo_De_Token.Parentesis_Abierto)
        {
            return Parse_LLamada_Funcion(Verificandose.Texto);
        }
        else
        {
            var identificador = Proximo_Token();
            return new Literal(identificador, identificador.Texto);
        }
    }
    //parsear la expresion let in 
    public Expresion Parse_Let_in_Expresion()
    {
        var let_id = Match(Tipo_De_Token.let_Keyword);
        var let_Expresion = Parse_Let_Expresion();
        var in_id = Match(Tipo_De_Token.in_Keyword);
        var in_Expresion = Parse_Expresion();
        return new Let_in(let_Expresion, in_Expresion);
    }
    public Let Parse_Let_Expresion()
    {
        var keyword = Match(Tipo_De_Token.Identificador);
        var igual = Match(Tipo_De_Token.Igual);
        var asignar = Parse_Expresion();
         
         //esto es x si se asignan a otras variables un valor en el mismo let 
        if (Verificandose.Tipo == Tipo_De_Token.coma)
        {
            var coma = Match(Tipo_De_Token.coma);
            var otras_variables = Parse_Let_Expresion();
            return new Let(keyword, asignar, otras_variables);
        }
        else
        {
            return new Let(keyword, asignar);
        }
    }
    private Expresion Parse_Expresion_Binaria(int parentPrecedence = 0)
    {
        // este metodo verifica primero llamando al metodo orden de prioridad la prioridad que tendria el token
        //si esta es 0 pasa al siguiente tipo de parseo
        //se comienza por las expresiones unarias q tienen la mayor prioridad
        Expresion left;
        var expresion_unaria = Verificandose.Tipo.Prioridad_Operadores_Unarios();

        if (expresion_unaria != 0 && expresion_unaria >= parentPrecedence)
        {
            var operador = Proximo_Token();
            var right = Parse_Expresion_Binaria(expresion_unaria);
            left = new Expresion_Unaria(operador, right);
        }
        //esto va al parser fundamental de expresiones donde segun el tipo de cada token se crearan las expresiones
        else left = Parseo_Fundamental_Expresion();
        //proceso similar que el de las expresiones unarias 
        //parseo de expresiones binarias
        //segun el operador entre las expresiones
        //la prioridad que va a tener 
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
    //parseo segun el tipo de token 
    private Expresion Parseo_Fundamental_Expresion()
    {
        switch (Verificandose.Tipo)
        {
            //expresion que al evaluarse limpia el diccionario de funciones
            case Tipo_De_Token.clean_keyword:
                {
                    var keyword = Proximo_Token();
                    return new Clean(keyword);
                }
                //expresion parentesis
            case Tipo_De_Token.Parentesis_Abierto:
                {
                    var left = Proximo_Token();
                    var expresion = Parse_Expresion();
                    var right = Match(Tipo_De_Token.Parentesis_Cerrado);
                    return new Parentesis(left, expresion, right);
                }
                //expresiones booleanas
            case Tipo_De_Token.True_Keyword:
            case Tipo_De_Token.False_Keyword:
                {
                    var keyword = Proximo_Token();
                    var valor = keyword.Tipo == Tipo_De_Token.True_Keyword;
                    return new Literal(keyword, valor);
                }
                //identificadores
            case Tipo_De_Token.Identificador:
                {
                    return Parse_Variable_O_LLamada_Funcion();
                }
                //parseo de condicionales
            case Tipo_De_Token.if_Keyword:
                {
                    var keyword = Proximo_Token();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Abierto)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{"("}>  in  position <{_posicion}> before the expresion");
                    }
                    var op_parentesis = Proximo_Token();
                    var parentesis = Parse_Expresion();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Cerrado)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{")"}> in position <{_posicion}> after the expresion");
                    }
                    var cl_parentesis = Proximo_Token();
                    var expresion = Parse_Expresion();
                    var _else = Parse_Expresion();
                    if (_else.Tipo != Tipo_De_Token.else_Expresion)
                        errores.Add($" SEMANTIC ERROR : Invalid expresion <{_else.Tipo}> in position <{_posicion}> expected <{"else_Expresion"}>");
                    return new IF(keyword, op_parentesis, parentesis, cl_parentesis, expresion, _else);
                }
                //else expresion que complemta al if expresion
            case Tipo_De_Token.else_Keyword:
                {
                    var keyword = Proximo_Token();
                    var expresion = Parse_Expresion();
                    return new Else(keyword, expresion);
                }
                //expresion que imprime en pantalla su argumento
            case Tipo_De_Token.print_Keyword:
                {
                    var keyword = Proximo_Token();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Abierto)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{"("}>  in position <{_posicion}> before the expresion");
                    }
                    var op_parentesis = Proximo_Token();
                    var expresion = Parse_Expresion();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Cerrado)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{")"}>  in position <{_posicion}> after the expresion");
                    }
                    var cl_parentesis = Proximo_Token();
                    return new Print(keyword, op_parentesis, expresion, cl_parentesis);
                }
                //declaracion de variable
            case Tipo_De_Token.let_Keyword:
                {
                    return Parse_Let_in_Expresion();
                }
                //ambito de definicion de variables
            case Tipo_De_Token.in_Keyword:
                {
                    var keyword = Match(Tipo_De_Token.in_Keyword);
                    var expresion = Parse_Expresion();
                    return new In(expresion);
                }
                //PI de la case Math 
            case Tipo_De_Token.PI_Keyword:
                {
                    var PI = Match(Tipo_De_Token.PI_Keyword);
                    var valor = Math.PI;
                    return new Literal(PI, valor);
                }
                //evaluacion de string
            case Tipo_De_Token.String:
                {
                    var keyword = Match(Tipo_De_Token.String);
                    var valor = keyword.Texto;
                    return new Literal(keyword, valor);
                }
                //funcion sen
            case Tipo_De_Token.sen_Keyword:
                {
                    var keyword = Proximo_Token();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Abierto)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{"("}> in position <{_posicion}> before the expresion");
                    }
                    var op_parentesis = Proximo_Token();
                    var expresion = Parse_Expresion();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Cerrado)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{")"}> in position <{_posicion}> after the expresion");
                    }
                    var cl_parentesis = Proximo_Token();
                    return new Sen(keyword, op_parentesis, expresion, cl_parentesis);
                }
                //funcion coseno
            case Tipo_De_Token.cos_Keyword:
                {
                    var keyword = Proximo_Token();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Abierto)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{"("}> in position <{_posicion}> before the expresion");
                    }
                    var op_parentesis = Proximo_Token();
                    var expresion = Parse_Expresion();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Cerrado)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{")"}> in position <{_posicion}> after the expresion");
                    }
                    var cl_parentesis = Proximo_Token();
                    return new Cos(keyword, op_parentesis, expresion, cl_parentesis);
                }
                //funcion logaritmo
            case Tipo_De_Token.logaritmo_Keyword:
                {
                    var keyword = Proximo_Token();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Abierto)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{"("}> in position <{_posicion}> before the expresion");
                    }
                    var op_parentesis = Proximo_Token();
                    var expresion = Parse_Expresion();
                    if (Verificandose.Tipo != Tipo_De_Token.Parentesis_Cerrado)
                    {
                        errores.Add($"! SYNTAX ERROR : Expected <{")"}> in position <{_posicion}> after the expresion");
                    }
                    var cl_parentesis = Proximo_Token();
                    return new Logaritmo(keyword, op_parentesis, expresion, cl_parentesis);
                }
                //por defecto se considerara que el token es un numero y es una expresion literal
            default:
                {
                    var token_num = Match(Tipo_De_Token.Numero);
                    var valor = token_num.Valor;
                    return new Literal(token_num, valor);
                }
        }
    }
}