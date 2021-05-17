using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Operator
    {
        // Лексема оператора
        private string lexeme;

        // Внешнее представление оператора, которое будет записываться в выходной строке
        private string outerRepresentation;

        // Приоритет оператора в таблице приоритетов
        private int? priority;

        // Таблица приоритетов операций
        private static Dictionary<string, int> priorityTable;


        static Operator()
        {
            FillPriorityTable();
        }

        public Operator(string _lexeme)
        {
            lexeme = _lexeme;
            outerRepresentation = _lexeme;
            try
            {
                priority = priorityTable[lexeme];
            }
            catch (KeyNotFoundException ex)
            {
                priority = null;
            }
        }

        public Operator(string _innerRepresentation, string _outerRepresentation) : this(_innerRepresentation)
        {
            outerRepresentation = _outerRepresentation;
        }

        public string GetLexeme()
        {
            return lexeme;
        }

        protected string GetOuterRepresentation()
        {
            return outerRepresentation;
        }

        public int? GetPriority()
        {
            return priority;
        }

        // Заполнение таблицы приоритетов
        private static void FillPriorityTable()
        {
            priorityTable = new Dictionary<string, int>();
            priorityTable.Add("if", 0);
            priorityTable.Add("(", 0);
            priorityTable.Add("[", 0);
            priorityTable.Add("АЭМ", 0);
            priorityTable.Add("Ф", 0);
            priorityTable.Add("for", 0);
            priorityTable.Add("while", 0);
            priorityTable.Add("do", 0);
            priorityTable.Add(")", 1);
            priorityTable.Add("]", 1);
            priorityTable.Add("[]", 1);
            priorityTable.Add(",", 1);
            priorityTable.Add(";", 1);
            priorityTable.Add("else", 1);
            priorityTable.Add("var", 1);
            priorityTable.Add("let", 1);
            priorityTable.Add("=", 2);
            priorityTable.Add("+=", 2);
            priorityTable.Add("-=", 2);
            priorityTable.Add("break", 2);
            priorityTable.Add("continue", 2);
            priorityTable.Add("||", 3);
            priorityTable.Add("&&", 4);
            priorityTable.Add("!", 5);
            priorityTable.Add("<", 6);
            priorityTable.Add(">", 6);
            priorityTable.Add("<=", 6);
            priorityTable.Add(">=", 6);
            priorityTable.Add("==", 6);
            priorityTable.Add("+", 7);
            priorityTable.Add("-", 7);
            priorityTable.Add("*", 8);
            priorityTable.Add("/", 8);
            priorityTable.Add("%", 8);
            priorityTable.Add("**", 9);
            priorityTable.Add("++", 10);
            priorityTable.Add(":", 11);
            priorityTable.Add("function", 11);
            priorityTable.Add("}", 11);
            priorityTable.Add("return", 12);
        }
    }
}
