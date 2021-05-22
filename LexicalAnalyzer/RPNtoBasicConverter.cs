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
            List<List<string>> result = new List<List<string>>();
            foreach (string element in rpn)
            {
                if (IsIdentifier(element) || IsConstant(element))
                {
                    constantsAndIdentifiersStack.Push(element);
                }

                if (element == "НФ")
                {
                    result = processFunctionBeginning(result);
                }
                else if (element == "КФ")
                {
                    result = processFunctionEnd(result);
                }
                else if (element == "КО")
                {
                    result = processVariableDeclaration(result);
                }
                else if (element == "УПЛ")
                {
                    result = processIfStatement(result);
                }
                else if (element == "БП")
                {
                    result = processUnconditionalJump(result);
                }
                else if (IsBinaryOperation(element))
                {
                    result = processBinaryOperation(result, element);
                }
                else if (element == "=")
                {
                    result = processAssignmentOperator(result);
                }
            }
            return result;
        }
    }
}
