using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class OperatorsStack
    {
        // Содержимое стека
        private List<Operator> operators;

        public OperatorsStack()
        {
            operators = new List<Operator>();
        }

        // Возвращает верхний элемент стека без удаления этого элемента
        public Operator GetElememt()
        {
            return operators.ElementAt(operators.Count - 1);
        }

        public bool ContainsLexeme(string lexeme)
        { 
            foreach (Operator op in operators)
            {
                if (op.GetLexeme() == lexeme)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsOneOfLexemes(params string[] lexemes)
        {
            foreach (Operator op in operators)
            {
                if (lexemes.Contains(op.GetLexeme()))
                {
                    return true;
                }
            }
            return false;
        }

        // Увеличивает значение счетчика последнего в стеке оператора объявления переменной
        public void IncreaseVariableDeclarationOperatorCounter()
        {
            for (int i = operators.Count-1; i >= 0; --i)
            {
                if (operators.ElementAt(i) is VariableDeclarationOperator)
                {
                    ((VariableDeclarationOperator)operators.ElementAt(i)).IncreaseCounter();
                    break;
                }
            }
        }

        // Является ли стек пустым
        public bool IsEmpty()
        {
            return operators.Count == 0;
        }

        // Возвращает верхний элемент стека, при этом удаляя его оттуда
        public Operator Pop()
        {
            if (IsEmpty())
            {
                return null;
            }
            Operator lastElem = operators.ElementAt(operators.Count - 1);
            operators.RemoveAt(operators.Count - 1);
            return lastElem;
        }

        // Добавление оператора в стек
        public void Push(Operator op)
        {
            operators.Add(op);
        }

    }
}
