using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class OperatorWithCounter : Operator
    {
        // Счетчик операндов
        private int counter;
        public OperatorWithCounter(string _lexeme) : base(_lexeme)
        {
            SetCounter(_lexeme);
        }

        public OperatorWithCounter(string _lexemeInnerRepresentation, string _lexemeOuterRepresentation) 
            : base(_lexemeInnerRepresentation, _lexemeOuterRepresentation)
        {
            SetCounter(_lexemeInnerRepresentation);
        }

        public int GetCounter()
        {
            return counter;
        }

        public new string GetLexeme()
        {
            return $"{this.GetCounter()} {this.GetOuterRepresentation()}";
        }

        private void SetCounter(string _lexeme)
        {
            switch (_lexeme)
            {
                case "АЭМ":
                    counter = 2;
                    break;
                case "var":
                case "let":
                case "function":
                case "Ф":
                    counter = 1;
                    break;
                default:
                    counter = 0;
                    break;
            }
        }

        public void IncreaseCounter()
        {
            counter++;
        }
    }
}
