using _1lab;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LexicalAnalyzer
{
    class RPNtoBasicConverter
    {
        private Stack<string> constantsAndIdentifiersStack;

        private Dictionary<string, List<string>> temporaryVariables;

        public RPNtoBasicConverter()
        {
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
                        result = ProcessVariableDeclaration(i, rpn, result);
                    }
                    else if (element == "УПЛ")
                    {
                        result = ProcessIfStatement(j, k, _rpnByLines, result);
                    }
                    else if (element == "БП")
                    {
                        result = ProcessUnconditionalJump(i, rpn, result);
                    }
                    else if (IsBinaryOperator(element))
                    {
                        result = ProcessBinaryOperator(result, element);
                    }
                    else if (element[element.Length - 1] == ':')
                    {
                        result.Add(new List<string> { element });
                    }
                    else if (element == "=")
                    {
                        result = ProcessAssignmentOperator(result);
                    }
                }
            }

            result = ReplaceTemporaryVariablesWithTheirValues(result);
            //result = MoveVariablesDeclarationToTheBeggining(result);
            
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

        // Перемещает операторы объявления переменных (за исключением тех, что внутри функций) в начало программы
        private List<List<string>> MoveVariablesDeclarationToTheBeggining(List<List<string>> _code)
        {
            List<List<string>> result = new List<List<string>>();
            List<List<string>> variablesDeclarationLines = new List<List<string>>();
            foreach (List<string> line in _code)
            {
                if (line.Count > 0 && line[0] == "Dim")
                {
                    variablesDeclarationLines.Add(line);
                }
                else
                {
                    result.Add(line);
                }
            }

            variablesDeclarationLines = UnionVariablesDeclarationLines(variablesDeclarationLines);
            variablesDeclarationLines.AddRange(result);
            return variablesDeclarationLines;
        }

        private List<List<string>> ProcessAssignmentOperator(List<List<string>> _currentCode)
        {
            string operand2 = constantsAndIdentifiersStack.Pop();
            string operand1 = constantsAndIdentifiersStack.Pop();
            _currentCode.Add(new List<string> { operand1, "=", operand2 });
            return _currentCode;
        }

        private List<List<string>> ProcessBinaryOperator(List<List<string>> _currentCode, string _operator)
        {
            List<string> newLine = new List<string>();
            string operand2 = constantsAndIdentifiersStack.Pop();
            string operand1 = constantsAndIdentifiersStack.Pop();
            newLine.Add("Dim");
            string newVariableName = VariablesManager.GetNewVariable();
            newLine.Add(newVariableName);
            newLine.Add("As");
            newLine.Add("Variant");

            List<string> newLine2 = new List<string> { newVariableName, "=", operand1, _operator, operand2 };

            _currentCode.Add(newLine);
            _currentCode.Add(newLine2);

            constantsAndIdentifiersStack.Push(newVariableName);

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

        private List<List<string>> ProcessUnconditionalJump(int _bpIndex, List<string> _rpn, List<List<string>> _currentCode)
        {
            _currentCode.Add(new List<string> { "GoTo", _rpn[_bpIndex - 1] });
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
                if (IsIdentifier(_rpn[i]) || IsConstant(_rpn[i]) || _rpn[i] == "true" || _rpn[i] == "false")
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
                else if (_rpn[i] == "=")
                {
                    counter--;
                    isEspressionPart = true;
                }
                else if (IsBinaryOperator(_rpn[i]))
                {
                    counter -= 2;
                }

                if (isEspressionPart && _rpn[i] != "=")
                {
                    currentExpression.AddPart(_rpn[i]);
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

        private List<List<string>> UnionVariablesDeclarationLines(List<List<string>> _variablesDeclarationLines)
        {
            // Собираем переменные
            List<string> variables = new List<string>();
            foreach (List<string> line in _variablesDeclarationLines)
            {
                foreach (string word in line)
                {
                    if (word != "Dim" && word != "As" && word != "," && word != "Variant")
                    {
                        variables.Add(word);
                    }
                }
            }

            List<List<string>> result = new List<List<string>>();
            List<string> currentLine = new List<string> { "Dim" };
            for (int i = 0; i < variables.Count; ++i)
            {
                currentLine.Add(variables[i]);
                if (i < variables.Count - 1)
                {
                    currentLine.Add(",");
                }
                // Переход на новую строку
                if ((i + 1) % 10 == 0)
                {
                    result.Add(currentLine);
                    currentLine = new List<string>();
                }
            }
            if (currentLine.Count > 0)
            {
                currentLine.Add("As");
                currentLine.Add("Variant");
                result.Add(currentLine);
            }
            else
            {
                result[result.Count - 1].Add("As");
                result[result.Count - 1].Add("Variant");
            }
            return result;
        }
    }
}
