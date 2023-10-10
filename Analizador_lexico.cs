using System.ComponentModel.Design;

namespace Hulk;
// Aqui voy analizando la entrada del usuario char por char
class Analizador_lexico
{
    private readonly string texto;
    private int _posicion;
    private List<string> errores = new List<string>();
    public IEnumerable<String> Error => errores;
    //constructor de la clase
    public Analizador_lexico(string text)
    {
        texto = text;
    }
    private char charete => Tomar(0);
    private char Siguiente_char => Tomar(1);
    private char Tomar(int offset)
    {
        int index = _posicion + offset;
        if (index >= texto.Length) return '\0';
        else return texto[index];
    }
    public void Siguiente()
    {
        _posicion++;
    }
    //Aqui es donde veo las caracteristicas de cada token y los voy creando y asignando propiedades segun sus caracteristicas
    public Token Proximo_Token()
    {
        if (_posicion >= texto.Length) return new Token(Tipo_De_Token.Final, _posicion, "\0", null);

        if (charete == ' ') return new Token(Tipo_De_Token.Espacio, _posicion++, " ", null);

        int inicio = _posicion;

        if (char.IsDigit(charete))
        {
            while (char.IsDigit(charete)) Siguiente();
            int final = _posicion - inicio;
            string fragmento = texto.Substring(inicio, final);
            double.TryParse(fragmento, out var valor);
            return new Token(Tipo_De_Token.Numero, _posicion, fragmento, valor);
        }
        if (char.IsLetter(charete))
        {
            while (char.IsLetter(charete)) Siguiente();
            int final = _posicion - inicio;
            string fragmento = texto.Substring(inicio, final);
            var tipo = Keyword.Tipo(fragmento);
            return new Token(tipo, _posicion, fragmento, null);
        }
        switch (charete)
        {
            case '+': return new Token(Tipo_De_Token.Suma, _posicion++, "+", null);

            case '-': return new Token(Tipo_De_Token.Resta, _posicion++, "-", null);

            case '*': return new Token(Tipo_De_Token.Producto, _posicion++, "*", null);

            case '/': return new Token(Tipo_De_Token.Division, _posicion++, "/", null);

            case '^': return new Token(Tipo_De_Token.Potenciacion, _posicion++, "^", null);

            case '(': return new Token(Tipo_De_Token.Parentesis_Abierto, _posicion++, "(", null);

            case ')': return new Token(Tipo_De_Token.Parentesis_Cerrado, _posicion++, ")", null);

            case '<': return new Token(Tipo_De_Token.Menor_que, _posicion += 2, "<", null);

            case '>': return new Token(Tipo_De_Token.Mayor_que, _posicion += 2, ">", null);
                  
            case '=':
                {
                    if (Siguiente_char == '=') 
                    {
                        _posicion += 2;
                        return new Token(Tipo_De_Token.IgualIgual, inicio , "==", null);
                    }
                    else return new Token(Tipo_De_Token.Igual, _posicion++, "=", null);
                }

           case '!':
                {
                    if (Siguiente_char == '=') 
                    {
                        _posicion += 2;
                        return new Token(Tipo_De_Token.Bang_Igual, inicio , "!=", null);
                    }
                    else return new Token(Tipo_De_Token.Bang, _posicion++, "!", null);
                }

            case '&':
                {
                    if (Siguiente_char == '&') 
                    {
                        _posicion += 2;
                        return new Token(Tipo_De_Token.AmpersandAmpersand,inicio , "&&", null);
                    }
                    else break;
                }
            case '|':
                {
                    if (Siguiente_char == '|') 
                    {
                        _posicion += 2;
                        return new Token(Tipo_De_Token.PipePipe, inicio , "||", null);
                    }
                    else break;
                }
        }
        errores.Add($"ERROR: Ha ingresada un caracter incorrecto: '{charete}'");

        return new Token(Tipo_De_Token.Malo, _posicion++, texto.Substring(_posicion - 1, 1), null);
    }
}