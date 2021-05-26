using _1lab;
using System.Collections.Generic;
using System.Collections.Specialized;

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
            for (int i = 0; i < rpn.Count; ++i)
            {
                string element = rpn[i];
                if (IsIdentifier(element) || IsConstant(element) || IsNumber(element))
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
                    result = ProcessFunctionEnd(i, rpn, result);
                }
                else if (element == "КО")
                {
                    result = ProcessVariableDeclaration(i, rpn, result);
                }
                /*else if (element == "УПЛ")
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
                }*/
            }

            return result;
        }

        // Нахождение имени функции по индексу её конца
        private string FindFunctionName(int _functionEndPosition, List<string> _rpn)
        {
            // Уровень вложенности относительно искомой функции
            int currentLevel = 0;
            for (int i = _functionEndPosition - 1; i >= 0; --i)
            {
                if (_rpn[i] == "КФ")
                {
                    currentLevel++;
                }
                else if (_rpn[i] == "НФ" && currentLevel == 0)
                {
                    int counter = int.Parse(_rpn[i - 3]);
                    return _rpn[i - counter - 3];
                }
                else if (_rpn[i] == "НФ")
                {
                    currentLevel--;
                }
            }
            return "";
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

        private bool IsNumber(string _element)
        {
            foreach (char ch in _element)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }
            return true;
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

        private List<List<string>> ProcessFunctionEnd(int _functionEndIndex, List<string> _rpn, List<List<string>> _currentCode)
        {
            string functionName = FindFunctionName(_functionEndIndex, _rpn);
            List<string> newLine = new List<string> { "End"};
            if (IsPrcedure(functionName, _rpn))
            {
                newLine.Add("Sub");
            }
            else
            {
                newLine.Add("Function");
            }

            _currentCode.Add(newLine);
            return _currentCode;
        }

        private List<List<string>> ProcessVariableDeclaration(int _declarationEndIndex, List<string> _rpn, List<List<string>> _currentCode)
        { 
            int level = int.Parse(constantsAndIdentifiersStack.Pop());
            int functionNumber = int.Parse(constantsAndIdentifiersStack.Pop());
            int operandsCounter = int.Parse(constantsAndIdentifiersStack.Pop());

            List<string> lineWithVarDeclaration = new List<string> { "Dim" };
            // Вспомогательная переменная для того, чтобы отличать объявленные переменные 
            // от операндов выражений
            int counter = 0;
            List<string> variables = new List<string>();
            OrderedDictionary variablesToSet = new OrderedDictionary();
            bool isEspressionPart = false;
            Expression currentExpression = new Expression();
            for (int i = _declarationEndIndex - 4; i >= 0 && variables.Count < operandsCounter; --i)
            {
                if (IsIdentifier(_rpn[i]))
                {
                    if (counter < 0)
                    {
                        counter++;
                    }
                    if (counter == 0)
                    {
                        variables.Add(_rpn[i]);
                        if (isEspressionPart)
                        {
                            isEspressionPart = false;
                            variablesToSet.Add(_rpn[i], currentExpression);
                            currentExpression = new Expression();
                        }
                    }
                }
                else if (_rpn[i] == "=" || IsBinaryOperator(_rpn[i]))
                {
                    counter--;
                    isEspressionPart = true;
                }

                if (isEspressionPart && _rpn[i] != "=")
                {
                    currentExpression.AddPart(_rpn[i]);
                    counter++;
                }
            }

            // Добавляем объявление переменных
            variables.Reverse();
            for (int i = 0; i < variables.Count; ++i)
            {
                lineWithVarDeclaration.Add(variables[i]);
                if (i < variables.Count - 1)
                {
                    lineWithVarDeclaration.Add(",");
                }
            }
            lineWithVarDeclaration.Add("As");
            lineWithVarDeclaration.Add("Variant");
            _currentCode.Add(lineWithVarDeclaration);

            // Добавляем инициализацию переменных
            string[] keys = new string[variablesToSet.Count];
            Expression[] values = new Expression[variablesToSet.Count];
            variablesToSet.Keys.CopyTo(keys, 0);
            variablesToSet.Values.CopyTo(values, 0);

            for(int i = variablesToSet.Count-1; i >= 0; --i)
            {
                _currentCode.Add(new List<string> { keys[i], "=", values[i].ToString() });
            }

            return _currentCode;
        }
    }
}
