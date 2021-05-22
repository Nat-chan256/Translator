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
            priorityTable = new Dictionary<string, int>
            {
                { "if", 0 },
                { "(", 0 },
                { "[", 0 },
                { "АЭМ", 0 },
                { "Ф", 0 },
                { "for", 0 },
                { "while", 0 },
                { "do", 0 },
                { ")", 1 },
                { "]", 1 },
                { "[]", 1 },
                { ",", 1 },
                { ";", 1 },
                { "else", 1 },
                { "var", 1 },
                { "let", 1 },
                { "=", 2 },
                { "+=", 2 },
                { "-=", 2 },
                { "break", 2 },
                { "continue", 2 },
                { "||", 3 },
                { "&&", 4 },
                { "!", 5 },
                { "<", 6 },
                { ">", 6 },
                { "<=", 6 },
                { ">=", 6 },
                { "==", 6 },
                { "+", 7 },
                { "-", 7 },
                { "*", 8 },
                { "/", 8 },
                { "%", 8 },
                { "**", 9 },
                { "++", 10 },
                { ":", 11 },
                { "function", 11 },
                { "}", 11 },
                { "return", 12 }
            };
        }
    }
}
