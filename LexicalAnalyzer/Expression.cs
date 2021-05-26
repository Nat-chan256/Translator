using System.Collections.Generic;

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

        public new string ToString()
        {
            string str = "";
            foreach (string part in componentsList)
            {
                str += part + " ";
            }
            return str;
        }
    }
}
