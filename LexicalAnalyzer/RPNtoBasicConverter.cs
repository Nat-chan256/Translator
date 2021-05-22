using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class RPNtoBasicConverter
    {

        public List<List<string>> ConvertToBasic(List<string> rpn)
        {
            foreach (string element in rpn)
            {
                if (IsIdentifier(element) || IsConstant(element))
                {
                    constantsAndIdentifiersStack.Push(element);
                }


            }
        }
    }
}
