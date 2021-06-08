using System.Collections.Generic;
using _1lab;

namespace LexicalAnalyzer
{
    class Expression
    {
        // Составные части выражения
        private List<string> componentsList;

        public Expression()
        {
            componentsList = new List<string>();
        }

        public void AddPart(string _part)
        {
            componentsList.Add(_part);
        }

        public void Clear()
        {
            componentsList.Clear();
        }

        private void ConvertToInterfixForm()
        {
            // Перевод выражений с одним бинарным оператором и двумя операндами
            if (componentsList.Count == 3)
            {
                if (IsOperator(componentsList[0]))
                {
                    string op = componentsList[0];
                    string operand2 = componentsList[1];
                    string operand1 = componentsList[2];
                    componentsList.Clear();
                    componentsList.Add(operand1);
                    componentsList.Add(op);
                    componentsList.Add(operand2);
                }
            }
        }

        private bool IsInInterfixForm()
        {
            return !(IsOperator(componentsList[0]) || IsOperator(componentsList[componentsList.Count - 1]));
        }

        private bool IsOperator(string _lexeme)
        {
            return ServiceTablesContainer.GetInstance().GetOperatorsTable().ContainsKey(_lexeme);
        }

        public new string ToString()
        {
            if (componentsList.Count != 0 && !IsInInterfixForm())
            {
                ConvertToInterfixForm();
            }
            string str = "";
            foreach (string part in componentsList)
            {
                str += part + " ";
            }
            return str;
        }
    }
}
