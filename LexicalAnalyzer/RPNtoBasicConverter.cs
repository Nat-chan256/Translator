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

        private List<string> FindArgumentsTypes(string _functionName, List<string> _rpn, int _argumentsNumber)
        {
            List<string> argumentsTypes = new List<string>();
            for (int i = 0; i < _rpn.Count; ++i)
            {
                if (_rpn[i] == "Ф")
                {
                    // Находим вызов указанной функции в тексте программы
                    if (_functionName == FindFunctionName(i, _rpn))
                    {
                        int functionNameIndex = FindFunctionNameIndex(i, _rpn);
                        Stack<string> constsIdentifiers = new Stack<string>();
                        List<Expression> arguments = new List<Expression>();
                        // Обрабатываем аргументы функции по той же логике, что и всю программу
                        for (int j = functionNameIndex + 1; j < i; ++j)
                        {
                            if (IsIdentifier(_rpn[j]) || IsConstant(_rpn[j]))
                            {
                                constsIdentifiers.Push(_rpn[j]);
                            }


                            if (IsBinaryOperation(_rpn[j]))
                            {
                                string secondOperand = constsIdentifiers.Pop();
                                string firstOperand = constsIdentifiers.Pop();
                                arguments.Add(new Expression(firstOperand, _rpn[j], secondOperand));
                            }
                            else if (_rpn[j] == "АЭМ")
                            {
                                int counter = int.Parse(constsIdentifiers.Pop());
                                List<string> arrayElemComponents = new List<string>();
                                for (int k = 0; k < counter; ++k)
                                {
                                    arrayElemComponents.Add(constsIdentifiers.Pop());
                                }
                                arguments.Add(new ArrayExpression(arrayElemComponents));
                            }
                        }

                        for (int j = 0; j < arguments.Count; ++j)
                        {
                            argumentsTypes.Add(GetTypeOf(arguments[j]));
                        }

                        return argumentsTypes;
                    }
                }
            }

            // Если не удалось определеить типы параметров
            // Делаем их по умолчанию Variant
            for (int i = 0; i < _argumentsNumber; ++i)
            {
                argumentsTypes.Add("Variant");
            }
            return argumentsTypes;
        }

        // Нахождение имени функции
        // _fOperatorPosition - позиция оператора Ф, для которого нужно найти имя функции
        private string FindFunctionName(int _fOperatorPosition, List<string> _rpn)
        {
            return _rpn[FindFunctionNameIndex(_fOperatorPosition, _rpn)];
        }

        private int FindFunctionNameIndex(int _fOperatorPosition, List<string> _rpn)
        {
            int operandsNumber = int.Parse(_rpn[_fOperatorPosition - 1]);
            if (operandsNumber == 1)
            {
                return _fOperatorPosition - 2;
            }
            string currentWord = "";
            int operandsCounter = 0;

            int i;
            for (i = _fOperatorPosition - 2; i >= 0 && operandsCounter < operandsNumber - 1; --i)
            {
                currentWord = _rpn[i];
                if (currentWord == "АЭМ" || currentWord == "[]")
                {
                    int arrayCounter = int.Parse(_rpn[i - 1]);
                    // Уменьшаем счетчик операндов, чтобы операнды АЭМ не учитывались при подсчете аргументов функции
                    operandsCounter -= arrayCounter + 1;
                    i--;
                }
                else if (IsBinaryOperator(currentWord))
                {
                    operandsCounter -= 1;
                }
                else if (IsIdentifier(currentWord) || IsConstant(currentWord))
                {
                    operandsCounter++;
                }
            }
            return i;
        }

        private string FindFunctionType(string _functionName, List<string> _rpn)
        {
            for (int i = 0; i < _rpn.Count; ++i)
            {
                if (_rpn[i] == "Ф" && _functionName == FindFunctionName(i, _rpn))
                {
                    int functionInvokeIndex = FindFunctionNameIndex(i, _rpn);
                    int operandsCounter = 0;
                    int j;
                    List<string> expressionComponents = new List<string>();
                    for (j = functionInvokeIndex - 1; j >= 0 && operandsCounter < 1; --j)
                    {
                        if (_rpn[j] == "АЭМ")
                        {
                            int arrayOperandsCounter = int.Parse(_rpn[j - 1]);
                            operandsCounter -= arrayOperandsCounter + 1;
                            j--;
                        }
                        else 
                        {
                            expressionComponents.Add(_rpn[j]);
                            operandsCounter++;
                        }
                    }

                    return GetTypeOf(new Expression(expressionComponents), _rpn);
                }
            }

            // Если не удалось найти тип функции
            return "Variant";
        }

        private string GetTypeOf(Expression _expression, List<string> _rpn)
        {
            for (int i = 0; i < _rpn.Count; ++i)
            { 
                if (_rpn)
            }
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
            // Добавляем аргументы функции
            List<string> argumentsTypes = FindArgumentsTypes(functionName, _rpn, operandsNumber - 1);

            for (int i = operands.Count - 2; i >= 0; --i)
            {
                functionSignature.Add(operands[i]);
                functionSignature.Add("As");
                functionSignature.Add(argumentsTypes[argumentsTypes.Count - i - 1]);
                if (i > 0)
                {
                    functionSignature.Add(",");
                }
            }
            functionSignature.Add(")");

            if (!isProcedure)
            {
                functionSignature.Add("As");
                functionSignature.Add(FindFunctionType(functionName, _rpn));
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
