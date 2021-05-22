using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class VariableDeclarationOperator : OperatorWithCounter
    {
        // Номер функции, в которой был вызван оператор объявления функции
        private int currentFunctionNumber;

        // Уровень вложенности, на котором находится объявление функции
        private int currentNestingLevel;

        public VariableDeclarationOperator(string _lexemeInnerRepresentation, 
            int _currentFunctionNumber, 
            int _currentNestingLevel) : base(_lexemeInnerRepresentation, "КО")
        {
            currentFunctionNumber = _currentFunctionNumber;
            currentNestingLevel = _currentNestingLevel;
        }

        public new string GetLexeme()
        {
            return $"{this.GetCounter()} {currentFunctionNumber} {currentNestingLevel} {this.GetOuterRepresentation()}";
        }
    }
}
