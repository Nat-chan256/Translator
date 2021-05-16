using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.Operators
{
    class CycleOperator : OperatorWithCounter
    {
        // Метка для цикла do...while
        private string label;

        public CycleOperator(string _lexeme) : base(_lexeme, "НЦ")
        {
            if (_lexeme == "do")
            {
                label = LabelsManager.GetNewLabel();
            }
        }

        public string GetInnerRepresentation()
        {
            return ((Operator)this).GetLexeme();
        }

        public string GetLabel()
        {
            return label;
        }

        public new string GetLexeme()
        {
            if (((Operator)this).GetLexeme() == "do")
            {
                return $"НЦ {label}:";
            }
            else
            {
                return $"{this.GetCounter()} {this.GetOuterRepresentation()}";
            }
        }
    }
}
