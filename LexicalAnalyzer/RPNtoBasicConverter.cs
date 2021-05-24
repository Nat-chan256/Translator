using _1lab;
using System.Collections.Generic;

namespace LexicalAnalyzer
{
    class RPNtoBasicConverter
    {
        private Stack<string> constantsAndIdentifiersStack;

        public RPNtoBasicConverter()
        {
            constantsAndIdentifiersStack = new Stack<string>();
        }

        public List<List<string>> ConvertToBasic(List<string> rpn)
        {
            List<List<string>> result = new List<List<string>>();
            foreach (string element in rpn)
            {
                if (IsIdentifier(element) || IsConstant(element))
                {
                    constantsAndIdentifiersStack.Push(element);
                    continue;
                }

                if (element == "НФ")
                {
                    result = ProcessFunctionBeginning(result, rpn);
                }
                else if (element == "КФ")
                {
                    result = ProcessFunctionEnd(result);
                }
                else if (element == "КО")
                {
                    result = ProcessVariableDeclaration(result);
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

        private bool IsBinaryOperator(string _element)
        {
            return _element == "+" || _element == "-" || _element == "*" || _element == "/" || _element == "**" || _element == "%"
                   || _element == "&&" || _element == "||" || _element == "<" || _element == "<=" || _element == ">=" || _element == ">"
                   || _element == "==" || _element == "!=" || _element == "+=" || _element == "-=" || _element == "*=" || _element == "/="; 
        }

        private bool IsConstant(string _element)
        {
            return ServiceTablesContainer.GetInstance().GetNumConstantsTable().ContainsKey(_element)
                || ServiceTablesContainer.GetInstance().GetStringConstantsTable().ContainsKey(_element);
        }

        private bool IsIdentifier(string _element)
        {
            return ServiceTablesContainer.GetInstance().GetIdentifiersTable().ContainsKey(_element);
        }

        private bool IsPrcedure(string _functionName, List<string> _rpn)
        {
            int currentLevel = 0;
            bool isInsideFunction = false;
            for (int i = 0; i < _rpn.Count; ++i)
            {
                if (_rpn[i] == "НФ")
                {
                    if (isInsideFunction)
                    {
                        currentLevel++;
                    }
                    else
                    {
                        int operandsNumber = int.Parse(_rpn[i - 3]);
                        string functionName = _rpn[i - operandsNumber - 3];
                        if (_functionName == functionName)
                        {
                            isInsideFunction = true;
                        }
                    }
                }
                else if (_rpn[i] == "КФ")
                {
                    if (isInsideFunction && currentLevel == 0)
                    {
                        return true;
                    }
                    else if (isInsideFunction)
                    {
                        currentLevel--;
                    }
                }
                else if (_rpn[i] == "return" && isInsideFunction && currentLevel == 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Обработка начала описания функции
        // _currentCode - текущий результат перевода в Basic
        private List<List<string>> ProcessFunctionBeginning(List<List<string>> _currentCode, List<string> _rpn)
        {
            int level = int.Parse(constantsAndIdentifiersStack.Pop());
            int functionNumber = int.Parse(constantsAndIdentifiersStack.Pop());
            int operandsNumber = int.Parse(constantsAndIdentifiersStack.Pop());
            List<string> operands = new List<string>();
            for (int i = 0; i < operandsNumber; ++i)
            {
                operands.Add(constantsAndIdentifiersStack.Pop());
            }
            string functionName = operands[operands.Count - 1];

            List<string> functionSignature = new List<string>();

            bool isProcedure = IsPrcedure(functionName, _rpn);
            if (isProcedure)
            {
                functionSignature.Add("Sub");
            }
            else
            {
                functionSignature.Add("Function");
            }

            functionSignature.Add(functionName);
            functionSignature.Add("(");

            for (int i = operands.Count - 2; i >= 0; --i)
            {
                functionSignature.Add(operands[i]);
                functionSignature.Add("As");
                functionSignature.Add("Variant");
                if (i > 0)
                {
                    functionSignature.Add(",");
                }
            }
            functionSignature.Add(")");

            if (!isProcedure)
            {
                functionSignature.Add("As");
                functionSignature.Add("Variant");
            }

            _currentCode.Add(functionSignature);
            return _currentCode;
        }

        private List<List<string>> ProcessFunctionEnd(List<List<string>> _currentCode)
        {
            _currentCode.Add(new List<string>() {"End"});
            return _currentCode;
        }

        private List<List<string>> ProcessVariableDeclaration(List<List<string>> _currentCode)
        { 
            int level = int.Parse(constantsAndIdentifiersStack.Pop());
            int functionNumber = int.Parse(constantsAndIdentifiersStack.Pop());
            int operandsNumber = int.Parse(constantsAndIdentifiersStack.Pop());


        }
    }
}
