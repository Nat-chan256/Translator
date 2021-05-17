using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.Operators
{
    // Оператор, реализующий определение (не вызов!) функции
    class FunctionOperator : OperatorWithCounter
    {
        // Количество функций в обрабатываемой программе
        private static int functionsCount = 0;

        // Номер функции
        private int functionNumber;

        // Уровень вложенности функции
        private int nestingLevel;

        public FunctionOperator(int _nestingLevel): base("function", "НФ")
        {
            nestingLevel = _nestingLevel;
            functionNumber = FunctionOperator.GetNewFunctionNumber();
        }

        private static int GetNewFunctionNumber()
        {
            return ++functionsCount;
        }

        public static int GetLastFunctionNumber()
        {
            return functionsCount;
        }

        public new string GetLexeme()
        {
            return $"{this.GetCounter()} {functionNumber} {nestingLevel} {this.GetOuterRepresentation()}";
        }

        // Сбрасывает значение переменной functionsCount
        public static void Reset()
        {
            functionsCount = 0;
        }
    }
}
