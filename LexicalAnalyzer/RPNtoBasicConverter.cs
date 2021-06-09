using _1lab;
using System.Collections.Generic;

namespace LexicalAnalyzer
{
    class RPNtoBasicConverter
    {
        // Очередь операторов присваивания, которые надо вставить после текущего оператора описания переменных
        private List<List<string>> assignmentsQueue;

        private Stack<string> constantsAndIdentifiersStack;

        private Dictionary<string, List<string>> temporaryVariables;

        public RPNtoBasicConverter()
        {
            assignmentsQueue = new List<List<string>>();
            constantsAndIdentifiersStack = new Stack<string>();
            temporaryVariables = new Dictionary<string, List<string>>();
        }

        public List<List<string>> ConvertToBasic(List<List<string>> _rpnByLines)
        {
            VariablesManager.Reset();
            List<string> rpn = new List<string>();

            foreach (List<string> line in _rpnByLines)
            {
                foreach (string word in line)
                {
                    rpn.Add(word);
                }
            }

            List<List<string>> result = new List<List<string>>();

            // Индекс элемента в rpn
            int i = -1;
            for (int j = 0; j < _rpnByLines.Count; ++j)
            {
                for (int k = 0; k < _rpnByLines[j].Count; ++k)
                {
                    i++;
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
                        result = ProcessVariableDeclaration(j, _rpnByLines, result);
                    }
                    else if (element == "УПЛ")
                    {
                        result = ProcessIfStatement(j, k, _rpnByLines, result);
                    }
                    else if (element == "БП")
                    {
                        result = ProcessUnconditionalJump(i, rpn, result);
                    }
                    else if (element == "НЦ")
                    {
                        result = ProcessCycleOperator(result);
                    }
                    else if (IsBinaryOperator(element))
                    {
                        result = ProcessBinaryOperator(result, element);
                    }
                    else if (element == "return")
                    {
                        result = ProccessReturnOperator(i, rpn, result);
                    }
                    else if (element[element.Length - 1] == ':')
                    {
                        result.Add(new List<string> { element });
                    }
                    else if (element == "=")
                    {
                        result = ProcessAssignmentOperator(j, _rpnByLines, result);
                    }
                    else if (element == "[]")
                    {
                        result = ProcessAnonymousArrayOperator(j, _rpnByLines, result);
                        k++;
                        i++;
                    }
                }
            }

            result = ReplaceTemporaryVariablesWithTheirValues(result);

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

        private List<List<string>> ProcessAnonymousArrayOperator(int _lineIndex, List<List<string>> _rpnByLines, List<List<string>> _currentCode)
        {
            if (_rpnByLines[_lineIndex].Contains("КО"))
            {
                int arrayElemsNumber = int.Parse(constantsAndIdentifiersStack.Pop());
                List<string> arrayElems = new List<string>();

                // Извлекаем элементы массива
                for (int i = 0; i < arrayElemsNumber; ++i)
                {
                    arrayElems.Add(constantsAndIdentifiersStack.Pop());
                }
                arrayElems.Reverse();
                // Добавляем элементы массива в очередь присваиваний
                string arrayName = constantsAndIdentifiersStack.Pop();
                for (int i = 0; i < arrayElems.Count; ++i)
                {
                    assignmentsQueue.Add(new List<string> { arrayName + "(" + i + ")", "=", arrayElems[i] });
                }

                // Добавляем в стек имя массива, но уже с указанием его размера
                constantsAndIdentifiersStack.Push(arrayName + "(" + (arrayElemsNumber-1) + ")");
            }

            return _currentCode;
        }

        private List<List<string>> ProcessAssignmentOperator(int _lineNumber, List<List<string>> _rpnByLines, List<List<string>> _currentCode)
        {
            string operand2 = constantsAndIdentifiersStack.Pop();
            string operand1 = constantsAndIdentifiersStack.Pop();

            // Контейнер, в который будем добавлять оператор присваивания
            // Может быть текущим кодом либо очередью присваиваний
            List<List<string>> container = (_rpnByLines[_lineNumber].Contains("КО")) ? assignmentsQueue : _currentCode;

            if (!temporaryVariables.ContainsKey(operand2)) // Если второй операнд - не временная переменная
            {
                container.Add(new List<string> { operand1, "=", operand2 });
            }
            else
            {
                string operand2FullExpression = "";
                foreach (string expressionPart in temporaryVariables[operand2])
                {
                    operand2FullExpression += " " + expressionPart;
                }
                container.Add(new List<string> { operand1, "=", operand2FullExpression });
            }

            if (_rpnByLines[_lineNumber].Contains("КО"))
            {
                constantsAndIdentifiersStack.Push(operand1);
            }

            return _currentCode;
        }

        private List<List<string>> ProcessBinaryOperator(List<List<string>> _currentCode, string _operator)
        {
            string operand2 = constantsAndIdentifiersStack.Pop();
            string operand1 = constantsAndIdentifiersStack.Pop();
            string newVariableName = VariablesManager.GetNewVariable();

            temporaryVariables.Add(newVariableName, new List<string> { operand1, _operator, operand2 });

            constantsAndIdentifiersStack.Push(newVariableName);

            return _currentCode;
        }

        private List<List<string>> ProcessCycleOperator(List<List<string>> _currentCode)
        {
            int operandsCount = int.Parse(constantsAndIdentifiersStack.Pop());
            if (operandsCount == 3)
            {
                string increment = constantsAndIdentifiersStack.Pop();
                string condition = constantsAndIdentifiersStack.Pop();
                string initialization = constantsAndIdentifiersStack.Pop();
                // Обработка цикла for
                if ()
            }
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
            List<string> newLine = new List<string> { "End" };
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

        private List<List<string>> ProcessIfStatement(int _lineIndex, int _uplIndex, List<List<string>> _rpn, List<List<string>> _currentCode)
        {
            List<string> newLine = new List<string> { "If", "Not", "(", constantsAndIdentifiersStack.Pop() };
            newLine.Add(")");
            newLine.Add("Then");

            List<string> lineWithGoTo = new List<string> { "\t", "GoTo", _rpn[_lineIndex][_uplIndex - 1] };

            _currentCode.Add(newLine);
            _currentCode.Add(lineWithGoTo);

            return _currentCode;
        }

        private List<List<string>> ProccessReturnOperator(int _elemIndexInRPN, List<string> _rpn, List<List<string>> _currentCode)
        {
            string valueToReturn = constantsAndIdentifiersStack.Pop();
            if (temporaryVariables.ContainsKey(valueToReturn))
            {
                string temp = "";
                foreach (string expressionPart in temporaryVariables[valueToReturn])
                {
                    temp += expressionPart + " ";
                }
                valueToReturn = temp;
            }

            string functionName = FindFunctionName(_elemIndexInRPN, _rpn);

            _currentCode.Add(new List<string> { functionName, "=", valueToReturn });
            return _currentCode;
        }

        private List<List<string>> ProcessUnconditionalJump(int _bpIndex, List<string> _rpn, List<List<string>> _currentCode)
        {
            _currentCode.Add(new List<string> { "GoTo", _rpn[_bpIndex - 1] });
            return _currentCode;
        }

        private List<List<string>> ProcessVariableDeclaration(int _lineIndex, List<List<string>> _rpnByLines, List<List<string>> _currentCode)
        {
            int level = int.Parse(constantsAndIdentifiersStack.Pop());
            int functionNumber = int.Parse(constantsAndIdentifiersStack.Pop());
            int operandsCounter = int.Parse(constantsAndIdentifiersStack.Pop());

            List<string> lineWithVarDeclaration = new List<string> { "Dim" };
            List<string> variables = new List<string>();

            for (int i = 0; i < operandsCounter; ++i)
            {
                variables.Add(constantsAndIdentifiersStack.Pop());
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
            if (!_rpnByLines[_lineIndex].Contains("НЦ"))
            {
                while (assignmentsQueue.Count > 0)
                {
                    _currentCode.Add(assignmentsQueue[0]);
                    assignmentsQueue.RemoveAt(0);
                }
            }
            else
            {
                while (assignmentsQueue.Count > 0)
                {
                    string newVar = VariablesManager.GetNewVariable();
                    temporaryVariables.Add(newVar, assignmentsQueue[0]);
                    constantsAndIdentifiersStack.Push(newVar);
                    assignmentsQueue.RemoveAt(0);
                }
            }

            return _currentCode;
        }

        private List<List<string>> ReplaceTemporaryVariablesWithTheirValues(List<List<string>> _code)
        {
            List<List<string>> result = new List<List<string>>();
            List<string> currentLine = new List<string>();
            foreach (List<string> line in _code)
            {
                foreach (string word in line)
                {
                    if (temporaryVariables.ContainsKey(word))
                    {
                        currentLine.AddRange(temporaryVariables[word]);
                    }
                    else
                    {
                        currentLine.Add(word);
                    }
                }
                result.Add(currentLine);
                currentLine = new List<string>();
            }
            return result;
        }

    }
}
