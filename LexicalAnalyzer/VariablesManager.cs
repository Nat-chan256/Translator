using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1lab;

namespace LexicalAnalyzer
{
    class VariablesManager
    {
        private static int lastVariableNumber = 0;
        // Возвращает переменную, ещё не использованную в тексте программы
        public static string GetNewVariable()
        {
            lastVariableNumber++;
            string varName = "R" + lastVariableNumber.ToString();
            while (ServiceTablesContainer.GetInstance().GetIdentifiersTable().Keys.Contains(varName))
            {
                varName = "R" + ++lastVariableNumber;
            }
            return varName;
        }

        public static void Reset()
        {
            lastVariableNumber = 0;
        }
    }
}
