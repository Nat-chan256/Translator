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

        private List<string> ConvertToBasic(List<string> _rpnLine)
        {
            RPNtoBasicConverter littleConverter = new RPNtoBasicConverter();
            List<List<string>> multipleLinesResult = littleConverter
                .ConvertToBasic(new List<List<string>> { _rpnLine }, false);

            List<string> result = new List<string>();
            foreach (List<string> line in multipleLinesResult)
            {
                foreach (string word in line)
                {
                    result.Add(word);
                }
            }

            return result;
        }

        public List<List<string>> ConvertToBasic(List<List<string>> _rpnByLines, bool _resetTempVars = true)
        {
            if (_resetTempVars)
            {
                VariablesManager.Reset();
            }
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
                if (j == 26)
                {
                    string l = "Debug";
                }
                for (int k = 0; k < _rpnByLines[j].Count; ++k)
                {
                    i++;
                    string element = rpn[i];

                    if (element.Length == 0)
                    {
                        continue;
                    }

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
                        result = ProcessCycleOperator(j, _rpnByLines, result);
                    }
                    else if (element == "КЦ")
                    {
                        result = ProcessCycleEnd(result);
                    }
                    else if (element == "АЭМ")
                    {
                        result = ProcessArrayOperator(result);
                    }
                    else if (IsBinaryOperator(element))
                    {
                        result = ProcessBinaryOperator(result, element);
                    }
                    else if (IsUnaryOperator(element))
                    {
                        result = ProcessUnaryOperator(element, result);
                    }
                    else if (element == "return")
                    {
                        result = ProccessReturnOperator(i, rpn, result);
                    }
                    // Обработка метки
                    else if (element[element.Length - 1] == ':')
                    {
                        // Заносим метку в результат только в том случае, когда она не является частью цикла
                        if (k > 0 && _rpnByLines[j][k - 1] != "НЦ" || k == 0)
                        {
                            result.Add(new List<string> { element });
                        }
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

            while (constantsAndIdentifiersStack.Count > 0)
            {
                string token = constantsAndIdentifiersStack.Pop();
                if (token.Length > 0)
                {
                    string line = ReplaceTempVarWithItsValue(token);
                    result.Add(new List<string> { line });
                }
            }

            result = ReplaceTemporaryVariablesWithTheirValues(result);

            return result;
        }

        // Извлечение условия из цикла do...while
        private List<string> ExtractCondition(int _lineIndex, List<List<string>> _rpnByLines)
        {
            List<List<string>> cycleBody = ExtractCycleBody(_lineIndex, _rpnByLines);
            int lineWithConditionIndex = _lineIndex + cycleBody.Count + 1;
            List<string> condition = new List<string>();
            foreach (string lexeme in _rpnByLines[lineWithConditionIndex])
            {
                if (lexeme == "УПИ")
                {
                    break;
                }
                condition.Add(lexeme);
            }
            return ConvertToBasic(condition);
        }

        // Нахождение тела цикла по индексу строки, в которой лежит лексема НЦ 
        private List<List<string>> ExtractCycleBody(int _lineIndex, List<List<string>> _rpnByLines)
        {
            List<List<string>> cycleBodyRpn = new List<List<string>>();
            int nestingLevel = 0;
            for (int i = _lineIndex + 1; i < _rpnByLines.Count; ++i)
            {
                if (!_rpnByLines[i].Contains("КЦ"))
                {
                    cycleBodyRpn.Add(_rpnByLines[i]);
                }
                else if (_rpnByLines[i].Contains("КЦ"))
                {
                    if (nestingLevel == 0)
                    {
                        return ConvertToBasic(cycleBodyRpn, false);
                    }
                    else
                    {
                        nestingLevel--;
                    }

                }
                if (_rpnByLines[i].Contains("НЦ"))
                {
                    nestingLevel++;
                }
            }
            return ConvertToBasic(cycleBodyRpn, false);
        }

        private string ExtractRightOperandInCondition(string _line)
        {
            string[] tokens = _line.Split();
            for (int i = 0; i < tokens.Length; ++i)
            {
                if ((new List<string> { "==", "<=", "<", ">=", ">", "!=" }).Contains(tokens[i]))
                {
                    return tokens[i + 1];
                }
            }
            return null;
        }

        // Извлечение шага из строк вида "i += 2" (в данном примере - 2)
        private string ExtractStep(string _line)
        {
            string[] tokens = _line.Trim(' ').Split();
            for (int i = 0; i < tokens.Length; ++i)
            {
                if ((new List<string> { "+=", "-=", "+", "-" }).Contains(tokens[i]))
                {
                    if ((tokens[i] == "+" || tokens[i] == "-") && tokens[i + 1] == tokens[0])
                    {
                        return tokens[i - 1];
                    }
                    else
                    {
                        return tokens[i + 1];
                    }
                }
            }
            return null;
        }

        // Извлечение имени переменной из строки вида "i = 0"
        private string ExtractVarFromInitialization(string _line)
        {
            return _line.Trim().Split()[0];
        }

        private string FindCurrentCycleIterator(List<List<string>> _code)
        {
            int nestingLevel = 0;
            for (int i = _code.Count - 1; i >= 0; --i)
            {
                for (int j = _code[i].Count - 1; j >= 0; --j)
                {
                    if (_code[i][j] == "Next")
                    {
                        nestingLevel++;
                    }

                    if (_code[i][j] == "For")
                    {
                        if (nestingLevel == 0)
                        {
                            return ExtractVarFromInitialization(_code[i][j + 1]);
                        }
                        else
                        {
                            nestingLevel--;
                        }
                    }
                }
            }
            return null;
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

        private bool IsInsideWhileCycle(List<List<string>> _code)
        {
            int nestingLevel = 0;
            for (int i = _code.Count - 1; i >= 0; --i)
            {
                for (int j = _code[i].Count - 1; j >= 0; --j)
                {
                    if (_code[i][j] == "Loop" || _code[i][j] == "Next")
                    {
                        nestingLevel++;
                    }

                    if (_code[i][j] == "While" || _code[i][j] == "For")
                    {
                        if (nestingLevel == 0)
                        {
                            return _code[i][j] == "While";
                        }
                        else
                        {
                            nestingLevel--;
                        }
                    }
                }
            }
            return false;
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

        private bool IsUnaryOperator(string _element)
        {
            return (new List<string> { "++", "--" }).Contains(_element);
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

        private List<List<string>> ProcessArrayOperator(List<List<string>> _currentCode)
        {
            int operndsCount = int.Parse(constantsAndIdentifiersStack.Pop());
            List<string> indeces = new List<string>();
            for (int i = 0; i < operndsCount - 1; ++i)
            {
                indeces.Add(constantsAndIdentifiersStack.Pop());
            }
            indeces.Reverse();
            string arrayName = constantsAndIdentifiersStack.Pop() + "(";
            for (int i = 0; i < indeces.Count; ++i)
            {
                arrayName += ReplaceTempVarWithItsValue(indeces[i]);
                if (i < indeces.Count - 1)
                {
                    arrayName += ",";
                }
                else
                {
                    arrayName += ")";
                }
            }
            constantsAndIdentifiersStack.Push(arrayName);
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
           
            if (_operator.Length == 2 && _operator[0] != '=' && _operator[1] == '=') // Операторы вида "+="
            {
                // Переделываем выражения вида "i += 1" в "i = i + 1" 
                _currentCode.Add(new List<string> { operand1, "=", operand1, _operator[0].ToString(), operand2 });
            }
            else 
            {
                string newVariableName = VariablesManager.GetNewVariable();

                temporaryVariables.Add(newVariableName, new List<string> { operand1, _operator, operand2 });

                constantsAndIdentifiersStack.Push(newVariableName);
            }

            return _currentCode;
        }

        private List<List<string>> ProcessCycleEnd(List<List<string>> _currentCode)
        {
            if (IsInsideWhileCycle(_currentCode))
            {
                _currentCode.Add(new List<string> { "Loop" });
            }
            else
            {
                string iteratorName = FindCurrentCycleIterator(_currentCode);
                _currentCode.Add(new List<string> { "Next", iteratorName });
            }
            
            return _currentCode;
        }

        private List<List<string>> ProcessCycleOperator(int _lineIndex, List<List<string>> _rpnByLines, 
            List<List<string>> _currentCode)
        {
            int operandsCount = int.Parse(constantsAndIdentifiersStack.Pop());
            List<string> lineWithCycle = new List<string>();
            // Обработка цикла for
            if (operandsCount == 3)
            {
                string increment = ReplaceTempVarWithItsValue(constantsAndIdentifiersStack.Pop());
                string condition = ReplaceTempVarWithItsValue(constantsAndIdentifiersStack.Pop());
                string initialization = ReplaceTempVarWithItsValue(constantsAndIdentifiersStack.Pop());
                // Обработка цикла for
                if ((condition.Contains("<") || condition.Contains(">")) && assignmentsQueue.Count <= 1)
                {
                    lineWithCycle.Add("For");
                    lineWithCycle.Add(initialization);
                    lineWithCycle.Add("To");
                    lineWithCycle.Add(ExtractRightOperandInCondition(condition));
                    if (!increment.Contains("--") && !increment.Contains("++"))
                    {
                        lineWithCycle.Add("Step");
                        lineWithCycle.Add(ExtractStep(increment));
                    }
                }
            }
            // Обработка цикла while
            else if (operandsCount == 1)
            {
                string condition = ReplaceTempVarWithItsValue(constantsAndIdentifiersStack.Pop());
                lineWithCycle.Add("Do");
                lineWithCycle.Add("While");
                lineWithCycle.Add(condition);
            }
            // Обработка цикла do...while
            else if (operandsCount == 0)
            {
                // Вставляем тело цикла до его начала, чтобы оно выполнилось хотя бы один раз
                List<List<string>> cycleBody = ExtractCycleBody(_lineIndex, _rpnByLines);
                _currentCode.AddRange(cycleBody);

                // Добавляем первую строку цикла
                List<string> condition = ExtractCondition(_lineIndex, _rpnByLines);
                lineWithCycle.Add("Do");
                lineWithCycle.Add("While");
                lineWithCycle.AddRange(condition);
            }

            _currentCode.Add(lineWithCycle);

            return _currentCode;
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

        private List<List<string>> ProcessUnaryOperator(string _operator, List<List<string>> _currentCode)
        {
            string operand = constantsAndIdentifiersStack.Pop();
            string tempVarName = VariablesManager.GetNewVariable();
            temporaryVariables.Add(tempVarName, new List<string> { _operator, operand });
            constantsAndIdentifiersStack.Push(tempVarName);
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
                    currentLine.Add(ReplaceTempVarWithItsValue(word));
                }
                result.Add(currentLine);
                currentLine = new List<string>();
            }
            return result;
        }

        private string ReplaceTempVarWithItsValue(string _varName)
        {
            if (!temporaryVariables.ContainsKey(_varName))
            {
                return _varName;
            }

            List<string> valueList = temporaryVariables[_varName];
            string value = "";
            foreach (string valuePart in valueList)
            {
                value += ReplaceTempVarWithItsValue(valuePart) + " ";
            }
            return value;
        }

    }
}
