using System;
using System.Collections.Generic;
using _1lab;

namespace LexicalAnalyzer
{
    // Синтаксический анализатор программы на языке JavaScript
    class SyntacticalAnalyzer
    {
        private int bracketsLevel;
        // Индексы текущего кода лексемы в матрице lexemeCodes
        private int i, j;
        private string[][] lexemeCodes;
        private string message = "Текст программы корректен";
        // Текущая обрабатываемая лексема (не код)
        private string nxtSymb;

        // Текущий символ - начало аргумента
        private void Argument()
        {
            if (IsIdentifier())
            {
                Scan();
                if (nxtSymb == "[")
                {
                    ArrayElement();
                }
                else if (nxtSymb == "(")
                {
                    FunctionCall();
                }
                else
                {
                    ScanBack();
                }
            }
            else if (IsConstant())
            {
                return;
            }
            else if (nxtSymb == "[")
            {
                Array();
            }
            else
            {
                Error();
            }
        }

        // Текущий символ - [
        private void Array()
        {
            Scan();
            while (nxtSymb != "]")
            {
                Argument();
                Scan();
                if (nxtSymb == ",")
                {
                    Scan();
                }
                else if (nxtSymb != "]")
                {
                    Error();
                }
            }
        }

        // Текущий символ - [
        private void ArrayElement()
        {
            Scan();
            Expression();
            Scan();
            if (nxtSymb != "]")
            {
                Error();
            }
            Scan();
            if (nxtSymb == "[")
            {
                ArrayElement();
            }
            else
            {
                ScanBack();
            }
        }

        // Текущий символ - первая лексема выражения
        private void AssigningExpression()
        {
            if (!IsIdentifier())
            {
                Error();
            }
            Scan();
            if (!IsAssigningOperator())
            {
                Error();
            }
            Scan();
            Expression();
        }

        // Текущий символ - перед началом условия
        private void Condition()
        {
            Scan();
            if (nxtSymb == "!")
            {
                Scan();
            }
            else
            {
                ExpressionWithoutComparisonOperator();
                Scan();
                if (!IsComparisonOperator())
                {
                    Error();
                    return;
                }
                Scan();
                ExpressionWithoutComparisonOperator();
            }

            Scan();
            // Проверка дополнительных условий, объединенных с помощью операторов || или &&
            if (new List<string> { "&&", "||", "&", "|" }.Contains(nxtSymb))
            {
                Condition();
            }
            else
            {
                ScanBack();
            }
        }

        private void ConditionalOperator()
        {
            // Обработка условия
            Scan();
            if (nxtSymb != "(")
            {
                Error();
            }
            Condition();
            Scan();
            if (nxtSymb != ")")
            {
                Error();
            }
            // Обработка тела оператора
            Scan();
            if (nxtSymb == "{")
            {
                Scan();
                Text();
                if (nxtSymb != "}")
                {
                    Error();
                }
            }
            else
            {
                Line();
            }
            // Обработка веток else и else if
            Scan();
            if (nxtSymb == "else")
            {
                Scan();
                if (nxtSymb == "if")
                {
                    ConditionalOperator();
                }
                else if (nxtSymb == "{")
                {
                    Scan();
                    Text();
                }
                else
                {
                    Line();
                }
            }
            else
            {
                ScanBack();
            }
        }

        private void Construction()
        {
            if (nxtSymb == "if")
            {
                ConditionalOperator();
            }
            else if (nxtSymb == "for" || nxtSymb == "while" || nxtSymb == "do")
            {
                CycleOperator();
            }
            else
            {
                Error();
            }
        }

        // Текущий символ - for, while либо do
        private void CycleOperator()
        {
            string cycleType = nxtSymb;
            if (cycleType == "for" || cycleType == "while")
            {
                Scan();
                if (nxtSymb != "(")
                {
                    Error();
                }
                Scan();
                // Обработка инициализации в цикле for
                if (nxtSymb != ";" && cycleType == "for")
                {
                    Declaration();
                    Scan(); // Пропускаем ;
                }
                else if (nxtSymb == ";")
                {
                    Scan();
                }
                else if (cycleType == "while")
                {
                    ScanBack();
                }
                Condition();
                Scan();
                if (nxtSymb == ";" && cycleType == "for")
                {
                    Scan();
                    if (nxtSymb != ")")
                    {
                        Increment();
                        Scan();
                        if (nxtSymb != ")")
                        {
                            Error();
                        }
                    }
                }
                else if (cycleType == "while" && nxtSymb != ")")
                {
                    Error();
                }
            }
            else if (cycleType != "do")
            {
                Error();
            }

            Scan();
            if (nxtSymb == "{")
            {
                Scan();
                Text();
            }
            else
            {
                Line();
            }
            if (cycleType == "do")
            {
                Scan();
                if (nxtSymb != "while")
                {
                    Error();
                }
                Scan();
                if (nxtSymb != "(")
                {
                    Error();
                }
                Condition();
                Scan();
                if (nxtSymb != ")")
                {
                    Error();
                }
                Scan();
                if (nxtSymb != ";")
                {
                    Error();
                }
            }
        }

        // Текущий символ - let либо var
        private void Declaration()
        {
            do
            {
                Scan();
                if (nxtSymb == ";")
                {
                    break;
                }
                else if (nxtSymb == ",")
                {
                    Scan();
                }
                if (!IsIdentifier())
                {
                    Error();
                }
                Scan();
                if (nxtSymb == "=")
                {
                    Scan();
                    Expression();
                }
                else if (nxtSymb != "," && nxtSymb != ";")
                {
                    Error();
                }
            } while (nxtSymb != ";");
            ScanBack();
        }

        private void Error()
        {
            message = $"Ошибка в строке {i + 1}";
            i = lexemeCodes.Length;
            throw new Error();
        }

        // Текущий символ - начало выражения
        private void Expression()
        {
            bool endedWithFunctionCall = false;
            while (nxtSymb == "(")
            {
                bracketsLevel++;
                Scan();
            }
            if (IsUnaryOperator())
            {
                Scan();

                if (!IsArgument())
                {
                    Error();
                }

                while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !(bracketsLevel == 0 && nxtSymb == ")"))
                {
                    Scan();
                    if ((new List<string> { ";", "," }).Contains(nxtSymb) || (bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        break;
                    }
                    if ((new List<string> { "(", ")" }).Contains(nxtSymb))
                    {
                        Scan();
                        if (nxtSymb == "(")
                        {
                            bracketsLevel++;
                        }
                        else
                        {
                            bracketsLevel--;
                        }
                    }
                    if (!IsOperator() && !IsComparisonOperator())
                    {
                        Error();
                    }
                    Scan();
                    if (!IsArgument())
                    {
                        Error();
                    }
                }
                ScanBack();
            }
            else if (IsArgument() && nxtSymb != "[")
            {
                Scan();
                if (IsUnaryOperator())
                {
                    while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !(bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        Scan();
                        if ((new List<string> { ";", "," }).Contains(nxtSymb) || (bracketsLevel == 0 && nxtSymb == ")"))
                        {
                            break;
                        }
                        if ((new List<string> { "(", ")" }).Contains(nxtSymb))
                        {
                            Scan();
                            if (nxtSymb == "(")
                            {
                                bracketsLevel++;
                            }
                            else
                            {
                                bracketsLevel--;
                            }
                        }
                        if (!IsOperator() && !IsComparisonOperator())
                        {
                            Error();
                        }
                        Scan();
                        if (!IsArgument())
                        {
                            Error();
                        }
                    }
                    ScanBack();
                }
                else if (nxtSymb == "[") // Обработка элемента массива
                {
                    ArrayElement();
                    while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !(bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        Scan();
                        if ((new List<string> { ";", ",", "]" }).Contains(nxtSymb) || (bracketsLevel == 0 && nxtSymb == ")"))
                        {
                            break;
                        }
                        if ((new List<string> { "(", ")" }).Contains(nxtSymb))
                        {
                            Scan();
                            if (nxtSymb == "(")
                            {
                                bracketsLevel++;
                            }
                            else
                            {
                                bracketsLevel--;
                            }
                        }
                        if (!IsOperator() && !IsComparisonOperator())
                        {
                            Error();
                        }
                        Scan();
                        if (!IsArgument())
                        {
                            Error();
                        }
                        if (IsFunctionCall())
                        {
                            Scan();
                            FunctionCall();
                            endedWithFunctionCall = true;
                        }
                        else
                        {
                            endedWithFunctionCall = false;
                        }
                        if (IsArrayElement())
                        {
                            Scan();
                            ArrayElement();
                        }
                    }
                    if (!endedWithFunctionCall)
                    {
                        ScanBack();
                    }
                }
                else if (nxtSymb == "]")
                {
                    ScanBack();
                    return;
                }
                else if (IsOperator() || IsComparisonOperator())
                {
                    Scan();
                    if (!IsArgument())
                    {
                        Error();
                    }
                    if (IsFunctionCall())
                    {
                        Scan();
                        FunctionCall();
                    }
                    while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !(bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        Scan();
                        if ((new List<string> { ";", "," }).Contains(nxtSymb) || (bracketsLevel == 0 && nxtSymb == ")"))
                        {
                            break;
                        }
                        if (!(IsOperator() || IsComparisonOperator()))
                        {
                            Error();
                        }
                        Scan();
                        if (!IsArgument())
                        {
                            Error();
                        }
                        if (IsFunctionCall())
                        {
                            Scan();
                            FunctionCall();
                        }
                    }
                    ScanBack();
                }
                else if (IsArgument())
                {
                    Error();
                }
                else
                {
                    ScanBack();
                    return;
                }
            }
            else if (nxtSymb == "[")
            {
                Array();
            }
            else
            {
                Error();
            }
        }

        // Текущий символ - начало выражения
        private void ExpressionWithoutComparisonOperator()
        {
            while (nxtSymb == "(")
            {
                bracketsLevel++;
                Scan();
            }
            if (IsUnaryOperator())
            {
                Scan();

                if (!IsArgument())
                {
                    Error();
                }
                if (IsFunctionCall())
                {
                    Scan();
                    FunctionCall();
                }

                while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !IsComparisonOperator() && !(bracketsLevel == 0 && nxtSymb == ")"))
                {
                    Scan();
                    if ((new List<string> { ";", "," }).Contains(nxtSymb) || IsComparisonOperator() || (bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        break;
                    }
                    if ((new List<string> { "(", ")" }).Contains(nxtSymb))
                    {
                        Scan();
                        if (nxtSymb == "(")
                        {
                            bracketsLevel++;
                        }
                        else
                        {
                            bracketsLevel--;
                        }
                    }
                    if (!IsOperator())
                    {
                        Error();
                    }
                    Scan();
                    if (!IsArgument())
                    {
                        Error();
                    }
                    if (IsFunctionCall())
                    {
                        Scan();
                        FunctionCall();
                    }
                }
                ScanBack();
            }
            else if (IsArgument() && nxtSymb != "[")
            {
                if (IsFunctionCall())
                {
                    Scan();
                    FunctionCall();
                }
                Scan();
                if (IsUnaryOperator())
                {
                    while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !IsComparisonOperator() && !(bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        Scan();
                        if ((new List<string> { ";", "," }).Contains(nxtSymb) || IsComparisonOperator() || (bracketsLevel == 0 && nxtSymb == ")"))
                        {
                            break;
                        }
                        if ((new List<string> { "(", ")" }).Contains(nxtSymb))
                        {
                            Scan();
                            if (nxtSymb == "(")
                            {
                                bracketsLevel++;
                            }
                            else
                            {
                                bracketsLevel--;
                            }
                        }
                        if (!IsOperator())
                        {
                            Error();
                        }
                        Scan();
                        if (!IsArgument())
                        {
                            Error();
                        }
                        if (IsFunctionCall())
                        {
                            Scan();
                            FunctionCall();
                        }
                    }
                    ScanBack();
                }
                else if (nxtSymb == "[")
                {
                    ArrayElement();
                    while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !IsComparisonOperator() 
                        && !(bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        Scan();
                        if ((new List<string> { ";", "," }).Contains(nxtSymb) || IsComparisonOperator() 
                            || (bracketsLevel == 0 && nxtSymb == ")"))
                        {
                            break;
                        }
                        if ((new List<string> { "(", ")" }).Contains(nxtSymb))
                        {
                            Scan();
                            if (nxtSymb == "(")
                            {
                                bracketsLevel++;
                            }
                            else
                            {
                                bracketsLevel--;
                            }
                        }
                        if (!IsOperator())
                        {
                            Error();
                        }
                        Scan();
                        if (!IsArgument())
                        {
                            Error();
                        }
                        if (IsFunctionCall())
                        {
                            Scan();
                            FunctionCall();
                        }
                    }
                    ScanBack();
                }
                else if (IsOperator())
                {
                    Scan();
                    if (!IsArgument())
                    {
                        Error();
                    }
                    if (IsFunctionCall())
                    {
                        Scan();
                        FunctionCall();
                    }
                    while (!(new List<string> { ";", "," }).Contains(nxtSymb) && !IsComparisonOperator() && !(bracketsLevel == 0 && nxtSymb == ")"))
                    {
                        Scan();
                        if ((new List<string> { ";", "," }).Contains(nxtSymb) || IsComparisonOperator() || (bracketsLevel == 0 && nxtSymb == ")"))
                        {
                            break;
                        }
                        if (!IsOperator())
                        {
                            Error();
                        }
                        Scan();
                        if (!IsArgument())
                        {
                            Error();
                        }
                        if (IsFunctionCall())
                        {
                            Scan();
                            FunctionCall();
                        }
                    }
                    ScanBack();
                }
                else
                {
                    ScanBack();
                    return;
                }
            }
            else if (nxtSymb == "[")
            {
                Array();
            }
            else
            {
                Error();
            }
        }

        private void Function()
        {
            Scan();
            if (!IsIdentifier())
            {
                Error();
            }
            Scan();
            if (nxtSymb != "(")
            {
                Error();
            }
            // Обработка аргументов функции
            while (nxtSymb != ")")
            {
                Scan();
                if (!IsIdentifier())
                {
                    Error();
                }
                Scan();
                if (nxtSymb != ")" && nxtSymb != ",")
                {
                    Error();
                }
            }
            // Обработка тела функции
            Scan();
            if (nxtSymb != "{")
            {
                Error();
            }
            Scan();
            Text();
            if (nxtSymb != "}")
            {
                Error();
            }
        }

        // Текущий символ - (
        private void FunctionCall()
        {
            Scan();
            while (nxtSymb != ")")
            {
                Expression();
                Scan();
                if (nxtSymb == ",")
                {
                    Scan();
                }
            }
        }

        private string GetLexemeByCode(string _lexemeCode)
        {
            return ServiceTablesContainer.GetInstance().GetLexemeByCode(_lexemeCode);
        }

        // Текущий символ - первая лексема инкремента
        private void Increment()
        {
            if ((new List<string> { "++", "--" }).Contains(nxtSymb))
            {
                Scan();
                if (!IsIdentifier())
                {
                    Error();
                }
            }
            else if (IsIdentifier())
            {
                Scan();
                if (!(new List<string> { "++", "--" }).Contains(nxtSymb))
                {
                    Error();
                }
            }
            else
            {
                AssigningExpression();
            }
        }

        private bool IsArgument()
        {
            return IsIdentifier() || IsConstant() || nxtSymb == "[";
        }

        private bool IsArgument(string _lexeme)
        {
            string temp = nxtSymb;
            nxtSymb = _lexeme;
            bool result = IsArgument();
            nxtSymb = temp;
            return result;
        }

        // Текущий символ - имя массива
        private bool IsArrayElement()
        {
            Scan();
            bool result = nxtSymb == "[";
            ScanBack();
            return result;
        }

        private bool IsAssigningOperator()
        {
            return (new List<string> { "=", "+=", "-=", "*=", "/=", "&=", "|=", "%=" }).Contains(nxtSymb);
        }

        private bool IsComparisonOperator()
        {
            return (new List<string> { "<", ">", "==", "!=", "<=", ">=" }).Contains(nxtSymb);
        }

        private bool IsConstant()
        {
            return lexemeCodes[i][j][0] == 'N' || lexemeCodes[i][j][0] == 'C';
        }

        // Текущий символ - имя функции
        private bool IsFunctionCall()
        {
            Scan();
            bool result = nxtSymb == "(";
            ScanBack();
            return result;
        }

        private bool IsIdentifier()
        {
            return lexemeCodes[i][j][0] == 'I';
        }

        private bool IsOperator()
        {
            return (ServiceTablesContainer.GetInstance().GetOperatorsTable().ContainsKey(nxtSymb) || nxtSymb == "return") && !IsComparisonOperator();
        }

        private bool IsUnaryOperator()
        {
            return (new List<string> { "++", "--", "return" }).Contains(nxtSymb);
        }

        private bool IsUnaryOperator(string _lexeme)
        {
            string temp = nxtSymb;
            nxtSymb = _lexeme;
            bool result = IsUnaryOperator();
            nxtSymb = temp;
            return result; 
        }

        private bool IsUnaryPostfixOperator()
        {
            return (new List<string> { "++", "--"}).Contains(nxtSymb);
        }

        // Текущий символ - первая лексема строки
        private void Line()
        {
            if (nxtSymb == "let" || nxtSymb == "var")
            {
                Declaration();
            }
            else
            {
                Expression();
            }
            Scan();
            if (nxtSymb != ";")
            {
                Error();
            }
        }

        public string Process(string[] _lexemeCodesByLines)
        {
            lexemeCodes = new string[_lexemeCodesByLines.Length][];
            for (int i = 0; i < lexemeCodes.Length; ++i)
            { 
                lexemeCodes[i] = _lexemeCodesByLines[i].Split();
            }
            i = 0;
            j = -1;
            bracketsLevel = 0;

            Scan();
            Program();

            return message;
        }

        private void Program()
        {
            try
            {
                Text();
            }
            catch(Exception ex)
            {
                return;
            }
        }

        // Подготовка очередного символа программы
        private void Scan()
        {
            j++;
            if (j == lexemeCodes[i].Length)
            {
                i++;
                j = 0;
            }

            nxtSymb = GetLexemeByCode(lexemeCodes[i][j]);
            
            if (nxtSymb == null)
            {
                Scan();
            }
        }

        private void ScanBack()
        {
            j--;
            if (j < 0)
            {
                i--;
                j = lexemeCodes[i].Length - 1;
            }
            nxtSymb = GetLexemeByCode(lexemeCodes[i][j]);
        }

        // Текущий символ - первая лексема текста
        private void Text()
        {
            while (i < lexemeCodes.Length && nxtSymb != "}")
            {
                if (nxtSymb == "function")
                {
                    Function();
                }
                else if (nxtSymb == "if" || nxtSymb == "for" || nxtSymb == "do" || nxtSymb == "while")
                {
                    Construction();
                }
                else
                {
                    Line();
                }

                Scan();
            }
        }
    }
}
