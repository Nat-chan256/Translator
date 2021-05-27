using System;
using System.Collections.Generic;
using _1lab;
using LexicalAnalyzer.Operators;

namespace LexicalAnalyzer
{
    class RPNConverter
    {
        // Номер текущей функции в обрабатываемой программе
        private int currentFunctionNumber;

        private int CurrentFunctionNumber {
            set {
                if (!(functionsDictionary.ContainsKey(value)))
                {
                    functionsDictionary[value] = CurrentFunctionNumber;
                }
                currentFunctionNumber = value;
            }
            get { return currentFunctionNumber; }
        }

        // Текущий уровень вложенности обрабатываемой программы
        private int currentNestingLevel;

        // Словарь, в котором ключи - номера функций, а значения - соответствующие им номера внешних функций, в которые они непосредственно вложены
        private readonly Dictionary<int, int> functionsDictionary;

        // Стек операторов
        private OperatorsStack operatorsStack;

        // Контейнер служебных таблиц
        private ServiceTablesContainer serviceTablesContainer;

        public RPNConverter()
        {
            functionsDictionary = new Dictionary<int, int>();
            operatorsStack = new OperatorsStack();
            serviceTablesContainer = ServiceTablesContainer.GetInstance();
        }

        // Перевод списка кодов лексем 
        // lexemesCodesByLines - список строк, где каждая строка может содержать несколько кодов лексем
        public List<List<string>> ConvertToRPN(List<string> lexemesCodesByLines)
        {
            CurrentFunctionNumber = 0;
            currentNestingLevel = 1;
            FunctionOperator.Reset();
            LabelsManager.Reset();
            operatorsStack.Clear();

            List<List<string>> result = new List<List<string>>();

            for (int j = 0; j < lexemesCodesByLines.Count; ++j)
            {
                string line = lexemesCodesByLines[j];
                if (line.Length == 0)
                {
                    continue;
                }
                List<string> resultLine = new List<string>(); // Результат перевода текущей строки в ОПЗ
                string[] lexemesCodesInCurLine = line.Split(); // Преобразовываем строку в массив кодов лексем
                for (int i = 0; i < lexemesCodesInCurLine.Length; ++i)
                {
                    if (lexemesCodesInCurLine[i].Length == 0)
                    {
                        continue;
                    }
                    String lexeme = serviceTablesContainer.GetLexemeByCode(lexemesCodesInCurLine[i]);

                    // Работа с операндом
                    if (IsOperand(lexemesCodesInCurLine[i]))
                    {
                        resultLine.Add(lexeme);
                        continue;
                    }

                    // Работа с оператором
                    if (operatorsStack.IsEmpty())
                    {
                        if (lexeme == "[")
                        {
                            // a[i]
                            if (i > 0 && IsOperand(lexemesCodesInCurLine[i - 1]))
                            {
                                operatorsStack.Push(new OperatorWithCounter("АЭМ"));
                            }
                            // [1,2,3]
                            else
                            {
                                operatorsStack.Push(new ArrayOperator());
                            }
                        }
                        else if (lexeme == "(" && i > 0 && IsFunction(i - 1, lexemesCodesInCurLine))
                        {
                            operatorsStack.Push(new OperatorWithCounter("Ф"));
                        }
                        else if (lexeme == "if")
                        {
                            operatorsStack.Push(new IfOperator());
                        }
                        else if (lexeme == "function")
                        {
                            operatorsStack.Push(new FunctionOperator(currentNestingLevel++));
                            CurrentFunctionNumber = FunctionOperator.GetLastFunctionNumber();
                        }
                        else if (lexeme == "var" || lexeme == "let")
                        {
                            operatorsStack.Push(new VariableDeclarationOperator(
                                lexeme,
                                CurrentFunctionNumber,
                                currentNestingLevel
                                ));
                        }
                        else if (lexeme == "for" || lexeme == "while" || lexeme == "do")
                        {
                            operatorsStack.Push(new CycleOperator(lexeme));
                            if (lexeme == "do")
                            {
                                resultLine.Add(((CycleOperator)operatorsStack.GetElememt()).GetLexeme());
                            }
                        }
                        else if (lexeme == ";")
                        {
                            continue;
                        }
                        else
                        {
                            operatorsStack.Push(new Operator(lexeme));
                        }
                    }
                    else if (lexeme == "(")
                    {
                        if (operatorsStack.GetElememt() is FunctionOperator
                            || operatorsStack.GetElememt() is IfOperator
                            || operatorsStack.GetElememt() is CycleOperator)
                        {
                            continue;
                        }
                        // Обработка вызова функций
                        else if (i > 0 && IsFunction(i - 1, lexemesCodesInCurLine))
                        {
                            operatorsStack.Push(new OperatorWithCounter("Ф"));
                        }
                        else
                        {
                            operatorsStack.Push(new Operator(lexeme));
                        }
                    }
                    else if (lexeme == ")")
                    {
                        if (!operatorsStack.IsEmpty()
                            && operatorsStack.GetElememt() is CycleOperator
                            && ((CycleOperator)operatorsStack.GetElememt()).GetInnerRepresentation() == "for")
                        {
                            resultLine.Add("ПЧЦ");
                        }

                        while (!operatorsStack.IsEmpty()
                        && operatorsStack.GetElememt().GetLexeme() != "("
                        && operatorsStack.GetElememt().GetLexeme() != "Ф"
                        && !(operatorsStack.GetElememt() is IfOperator)
                        && !(operatorsStack.GetElememt() is FunctionOperator)
                        && !(operatorsStack.GetElememt() is CycleOperator)
                        )
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }

                        if (operatorsStack.IsEmpty())
                        {
                            continue;
                        }
                        Operator op = operatorsStack.GetElememt();


                        if (op is CycleOperator && ((CycleOperator)op).GetInnerRepresentation() == "do")
                        {
                            resultLine.Add($"УПИ {((CycleOperator)op).GetLabel()} КЦ");
                            operatorsStack.Pop();
                            continue;
                        }

                        if (op.GetLexeme() == "Ф") // Добавление оператора Ф в выходную строку
                        {
                            if (!IsFunctionWithoutParameters(i, lexemesCodesInCurLine)) // Предотвращаем увеличение счетчика в случае, когда функция без параметров
                            {
                                ((OperatorWithCounter)op).IncreaseCounter();
                            }
                            resultLine.Add($"{((OperatorWithCounter)op).GetCounter()} {op.GetLexeme()}");
                            operatorsStack.Pop(); // Извелкаем Ф из стэка
                        }
                        else if (op is IfOperator)
                        {
                            resultLine.Add($"{((IfOperator)op).GetLabelUPL()} УПЛ");
                        }
                        else if (op is FunctionOperator)
                        {
                            if (!IsFunctionWithoutParameters(i, lexemesCodesInCurLine)) // Предотвращаем увеличение счетчика в случае, когда функция без параметров
                            {
                                ((OperatorWithCounter)op).IncreaseCounter();
                            }
                            resultLine.Add(((FunctionOperator)op).GetLexeme());
                        }
                        else if (op.GetLexeme() == "(")
                        {
                            operatorsStack.Pop();
                        }
                        else if (op is CycleOperator)
                        {
                            ((OperatorWithCounter)op).IncreaseCounter();
                            resultLine.Add(((CycleOperator)op).GetLexeme());
                        }
                    }
                    else if (lexeme == "[")
                    {
                        // a[i]
                        if (i > 0 && IsOperand(lexemesCodesInCurLine[i - 1]))
                        {
                            operatorsStack.Push(new OperatorWithCounter("АЭМ"));
                        }
                        // [1,2,3]
                        else
                        {
                            operatorsStack.Push(new ArrayOperator());
                        }
                    }
                    // Обработка элементов многомерных массивов
                    else if (lexeme == "]"
                        && i < lexemesCodesInCurLine.Length - 1
                        && lexemesCodesInCurLine[i + 1].Length > 0
                        && serviceTablesContainer.GetLexemeByCode(lexemesCodesInCurLine[i + 1]) == "[")
                    {
                        // Обработка многомерного массива
                        while (!(operatorsStack.GetElememt().GetLexeme() == "АЭМ"))
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                        ((OperatorWithCounter)operatorsStack.GetElememt()).IncreaseCounter();
                        i += 1; // Для корректной обработки второго индекса пропускаем итерацию
                    }
                    else if (lexeme == "]")
                    {
                        while (!operatorsStack.IsEmpty()
                            && !(operatorsStack.GetElememt().GetLexeme() == "АЭМ")
                            && !(operatorsStack.GetElememt() is ArrayOperator))
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                        OperatorWithCounter arrayOp = (OperatorWithCounter)operatorsStack.Pop();
                        if (arrayOp is ArrayOperator)
                        {
                            arrayOp.IncreaseCounter();
                        }
                        resultLine.Add(arrayOp.GetLexeme());
                    }
                    else if (lexeme == ",")
                    {
                        // Обработка массива, записанного в виде [1, 2, 3, ...]
                        if (operatorsStack.GetElememt().GetLexeme() == "АЭМ" || operatorsStack.GetElememt() is ArrayOperator)
                        {
                            ((OperatorWithCounter)operatorsStack.GetElememt()).IncreaseCounter();
                        }
                        // Обработка объявления переменных
                        else if (operatorsStack.ContainsLexeme("let") || operatorsStack.ContainsLexeme("var"))
                        {
                            operatorsStack.IncreaseVariableDeclarationOperatorCounter();
                            while (!operatorsStack.IsEmpty() && !(operatorsStack.GetElememt() is VariableDeclarationOperator))
                            {
                                resultLine.Add(operatorsStack.Pop().GetLexeme());
                            }
                        }
                        else
                        {
                            // Выталкиваем все операторы из стека до последнего оператора Ф или function
                            // Увеличиваем значение счетчика на 1
                            while (
                                !operatorsStack.IsEmpty()
                                && !((operatorsStack.GetElememt().GetLexeme() == "Ф") || operatorsStack.GetElememt() is FunctionOperator)
                                )
                            {
                                resultLine.Add(operatorsStack.Pop().GetLexeme());
                            }
                            if (!operatorsStack.IsEmpty())
                            {
                                OperatorWithCounter funcOp = (OperatorWithCounter)operatorsStack.GetElememt();
                                funcOp.IncreaseCounter();
                            }
                        }
                    }
                    else if (lexeme == "if")
                    {
                        operatorsStack.Push(new IfOperator());
                    }
                    else if (lexeme == "else")
                    {
                        while (!operatorsStack.IsEmpty() && !(operatorsStack.GetElememt() is IfOperator))
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                        resultLine.Add($"{((IfOperator)operatorsStack.GetElememt()).GetLabelUPI()} " +
                            $"БП {((IfOperator)operatorsStack.GetElememt()).GetLabelUPL()}:");
                    }
                    else if (lexeme == "}")
                    {
                        if (findNextLexeme(j, i, lexemesCodesByLines) == "else")
                        {
                            ((IfOperator)operatorsStack.GetElememt()).SetHasElseBranch(true);
                            continue;
                        }
                        while (!operatorsStack.IsEmpty()
                            && !(operatorsStack.GetElememt() is IfOperator)
                            && !(operatorsStack.GetElememt() is FunctionOperator)
                            && !(operatorsStack.GetElememt() is CycleOperator)
                            )
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                        if (!operatorsStack.IsEmpty())
                        {
                            Operator op = operatorsStack.GetElememt();
                            if (op is IfOperator)
                            {
                                if (((IfOperator)op).HasElseBrach())
                                {
                                    resultLine.Add($"{((IfOperator)op).GetLabelUPI()}:");
                                }
                                else
                                {
                                    resultLine.Add($"{((IfOperator)op).GetLabelUPL()}:");
                                }
                                operatorsStack.Pop();
                            }
                            else if (op is FunctionOperator)
                            {
                                currentNestingLevel--;
                                currentFunctionNumber = functionsDictionary[currentFunctionNumber];
                                resultLine.Add("КФ");
                                operatorsStack.Pop();
                            }
                            else if (op is CycleOperator && ((CycleOperator)op).GetInnerRepresentation() != "do")
                            {
                                resultLine.Add("КЦ");
                                operatorsStack.Pop();
                            }
                        }
                    }
                    else if (lexeme == "function")
                    {
                        operatorsStack.Push(new FunctionOperator(currentNestingLevel++));
                        CurrentFunctionNumber = FunctionOperator.GetLastFunctionNumber();

                    }
                    else if (lexeme == "var" || lexeme == "let")
                    {
                        operatorsStack.Push(new VariableDeclarationOperator(
                            lexeme,
                            CurrentFunctionNumber,
                            currentNestingLevel
                            ));
                    }
                    else if (lexeme == ";")
                    {
                        // Обработка пустой части цикла for
                        if (operatorsStack.GetElememt() is CycleOperator && ((CycleOperator)operatorsStack.GetElememt()).GetInnerRepresentation() == "for")
                        {
                            resultLine.Add("ПЧЦ");
                            ((OperatorWithCounter)operatorsStack.GetElememt()).IncreaseCounter();
                            continue;
                        }
                        while (!operatorsStack.IsEmpty()
                            && !(operatorsStack.GetElememt() is VariableDeclarationOperator)
                            && !(operatorsStack.GetElememt() is IfOperator)
                            && !(operatorsStack.GetElememt() is FunctionOperator)
                            && !(operatorsStack.GetElememt() is CycleOperator)
                            && !(operatorsStack.GetElememt().GetLexeme() == "return")
                            )
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                        if (operatorsStack.IsEmpty())
                        {
                            continue;
                        }
                        if (operatorsStack.GetElememt() is VariableDeclarationOperator)
                        {
                            // Извлекаем КО оператор
                            resultLine.Add(((VariableDeclarationOperator)operatorsStack.Pop()).GetLexeme());
                            continue;
                        }
                        if (!operatorsStack.IsEmpty() && operatorsStack.GetElememt() is CycleOperator)
                        {
                            ((CycleOperator)operatorsStack.GetElememt()).IncreaseCounter();
                        }
                        if (operatorsStack.GetElememt().GetLexeme() == "return")
                        {
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                    }
                    else if (lexeme == "for" || lexeme == "while" || lexeme == "do")
                    {
                        if (!(
                            (operatorsStack.GetElememt() is CycleOperator)
                            && ((CycleOperator)operatorsStack.GetElememt()).GetInnerRepresentation() == "do"
                            ))
                        {
                            operatorsStack.Push(new CycleOperator(lexeme));
                        }
                    }
                    else if (lexeme == "return")
                    {
                        operatorsStack.Push(new Operator(lexeme));
                    }
                    else if (new Operator(lexeme).GetPriority() == null)
                    {
                        continue;
                    }
                    else if (operatorsStack.GetElememt().GetPriority() < new Operator(lexeme).GetPriority()) // Если в стеке верхний элемент имеет более высокий приоритет, чем текущая операция
                    {
                        operatorsStack.Push(new Operator(lexeme));
                    }
                    else // Если в стеке верхний элемент (операция) имеет более низкий или равный приоритет
                    {
                        while (!operatorsStack.IsEmpty() && operatorsStack.GetElememt().GetPriority() >= new Operator(lexeme).GetPriority()) // пока не встретится операция с приоритетом выше, чем у рассматриваемой операции
                        {
                            // выталкиваем из стека во входную строку все операции
                            resultLine.Add(operatorsStack.Pop().GetLexeme());
                        }
                        // Операция из входной строки проталкивается в стек
                        operatorsStack.Push(new Operator(lexeme));
                    }
                }

                if (j == lexemesCodesByLines.Count - 1) // Если строка последняя
                {
                    // Добавляем все оставшиеся в стеке операции в выходную строку
                    while (!operatorsStack.IsEmpty())
                    {
                        resultLine.Add(operatorsStack.Pop().GetLexeme());
                    }
                }

                result.Add(resultLine);
            }

            result = RemoveEmptyLines(result);

            return result;
        }

        // Возвращает лексему, следующую за лексемой, стоящей не позиции [lineIndex][lexemeIndexInLine]
        private string findNextLexeme(int lineIndex, int lexemeIndexInLine, List<string> lexemesCodesByLines)
        {
            for (int i = 0; i < lexemesCodesByLines.Count; ++i)
            {
                if (i != lineIndex)
                {
                    continue;
                }

                string[] splittedLine = lexemesCodesByLines[i].Split();
                if (lexemeIndexInLine < splittedLine.Length - 1 && splittedLine[lexemeIndexInLine + 1] != "")
                {
                    return serviceTablesContainer.GetLexemeByCode(splittedLine[lexemeIndexInLine + 1]);
                }
                else
                {
                    int curIndex = i + 1;
                    while (curIndex < lexemesCodesByLines.Count
                        && lexemesCodesByLines[curIndex].Split()[0] == "")
                    {
                        curIndex++;
                    }
                    if (curIndex < lexemesCodesByLines.Count)
                    {
                        return serviceTablesContainer.GetLexemeByCode(lexemesCodesByLines[curIndex].Split()[0]);
                    }
                }
            }
            return null;
        }

        // Проверяет, является ли лексема, стоящая на позиции lexemeCodeIndex, функцией, по наличию открывающей скобки после неё
        private bool IsFunction(int _lexemeCodeIndex, string[] _line)
        {
            return _line[_lexemeCodeIndex][0] == 'I'
                && _lexemeCodeIndex < _line.Length - 1
                && serviceTablesContainer.GetLexemeByCode(_line[_lexemeCodeIndex + 1]) == "(";
        }

        // Проверяет, имеет ли функция параметры
        private bool IsFunctionWithoutParameters(int _closingParenthesisIndex, string[] _lineWithLexemesCodes)
        {
            return _closingParenthesisIndex > 0
                && serviceTablesContainer
                .GetLexemeByCode(
                    _lineWithLexemesCodes[_closingParenthesisIndex - 1]
                    ) == "(";
        }

        // Проверяет, является ли лексема с указанным кодом операндом 
        private bool IsOperand(string _lexemeCode)
        {
            if (_lexemeCode[0] == 'I' || _lexemeCode[0] == 'N' || _lexemeCode[0] == 'C' 
                || ServiceTablesContainer.GetInstance().GetLexemeByCode(_lexemeCode) == "true"
                || ServiceTablesContainer.GetInstance().GetLexemeByCode(_lexemeCode) == "false")
            {
                return true;
            }

            return false;
        }

        // Удаляет внутренние списки, которые содержат только один элемент ""
        private List<List<string>> RemoveEmptyLines(List<List<string>> text)
        {
            List<List<string>> result = new List<List<string>>();
            foreach (List<string> line in text)
            {
                if (line.Count == 0)
                {
                    continue;
                }
                result.Add(line);
            }
            return result;
        }
    }
}
