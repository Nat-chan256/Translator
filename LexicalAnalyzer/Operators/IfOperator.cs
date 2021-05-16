using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class IfOperator : Operator
    {
        private string labelUPL;

        public IfOperator() : base("if")
        {
            labelUPL = LabelsManager.GetNewLabel();
        }

        public string GetLabelUPI()
        { 
            return label
        }

        // Возвращает метку УПЛ
        public string GetLabelUPL()
        { 
            return labelUPL;
        }
    }
}
