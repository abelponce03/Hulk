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
        //este es el char que esta siendo analizado
        private char charete
        {
            get
            {
                if (_posicion >= texto.Length) return '\0';
                else return texto[_posicion];
            }
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

            if (char.IsDigit(charete))
            {
                int inicio = _posicion;
                while(char.IsDigit(charete)) Siguiente();
                int final = _posicion - inicio;
                string fragmento = texto.Substring(inicio, final);
                int.TryParse(fragmento, out var valor);
                return new Token(Tipo_De_Token.Numero, _posicion, fragmento, valor); 
            }
            if (char.IsLetter(charete))
            {
                int inicio = _posicion;
                while(char.IsLetter(charete)) Siguiente();
                int final = _posicion - inicio;
                string fragmento = texto.Substring(inicio, final);
                int.TryParse(fragmento, out var valor);
                var tipo = Keyword.Tipo(texto); 
                return new Token(tipo, _posicion, texto, valor);
            }
            if (charete == '+') return new Token(Tipo_De_Token.Suma, _posicion++, "+", null);

            else if (charete == '-') return new Token(Tipo_De_Token.Resta, _posicion++, "-", null);

            else if (charete == '*') return new Token(Tipo_De_Token.Producto, _posicion++, "*", null);

            else if (charete == '/') return new Token(Tipo_De_Token.Division, _posicion++, "/", null);

            else if (charete == '(') return new Token(Tipo_De_Token.Parentesis_Abierto, _posicion++, "(", null);

            else if (charete == ')') return new Token(Tipo_De_Token.Parentesis_Cerrado, _posicion++, ")", null);

            errores.Add($"ERROR: Ha ingresada un caracter incorrecto: '{charete}'");

            return new Token(Tipo_De_Token.Malo, _posicion++, texto.Substring(_posicion - 1, 1), null);
        }
    }