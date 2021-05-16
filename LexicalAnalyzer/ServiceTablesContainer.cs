using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1lab
{
    // Класс для хранения всех служебных таблиц.
    // Реализован по паттерну Singleton
    class ServiceTablesContainer
    {
        // Таблица разделителей
        // Ключ - лексема, значение - код лексемы
        private Dictionary<string, string> dividersTable;

        // Таблица идентификаторов
        private Dictionary<string, string> identifiersTable;

        // Таблица числовых констант
        private Dictionary<string, string> numConstantsTable;

        // Таблица операторов
        private Dictionary<string, string> operatorsTable;

        // Таблица приоритетов операций
        private Dictionary<string, int> priority;

        // Таблица служебных слов
        private Dictionary<string, string> serviceWordsTable;

        private Dictionary<string, string> stringConstantsTable;


        private List<string> twoLiterOps;

        // Экземпляр данного класса
        private static ServiceTablesContainer instance;

        private const string dividers = ";,(){} ", oneLiterOps = "/<+*=>%-&|^~!?:[].";

        private const string SERVICE_WORDS_FILE = "..\\..\\..\\Auxillary files\\Service words.txt";

        private ServiceTablesContainer()
        {
            numConstantsTable = new Dictionary<string, string>();
            dividersTable = new Dictionary<string, string>();
            identifiersTable = new Dictionary<string, string>();
            operatorsTable = new Dictionary<string, string>();
            serviceWordsTable = new Dictionary<string, string>();
            stringConstantsTable = new Dictionary<string, string>();

            FillDividersTable();
            FillServiceWordsTable();
            FillTwoLitersTable();
            FillOperatorsTable();
        }

        // Возвращает экземпляр данного класса
        public static ServiceTablesContainer GetInstance()
        {
            if (instance == null)
            {
                instance = new ServiceTablesContainer();
            }
            return instance;
        }

        public Dictionary<string, string> GetDividersTable()
        {
            return dividersTable;
        }

        public Dictionary<string, string> GetIdentifiersTable()
        {
            return identifiersTable;
        }

        // Возвращает лексему по её коду.
        public string GetLexemeByCode(string lexemeCode)
        {
            if (lexemeCode == "")
            {
                return null;
            }
            switch (lexemeCode[0]) // Проверяем первый символ кода лексемы
            {
                case 'I':
                    return identifiersTable.FirstOrDefault(x => x.Value == lexemeCode).Key;

                case 'W':
                    return serviceWordsTable.FirstOrDefault(x => x.Value == lexemeCode).Key;

                case 'O':
                    return operatorsTable.FirstOrDefault(x => x.Value == lexemeCode).Key;

                case 'R':
                    return dividersTable.FirstOrDefault(x => x.Value == lexemeCode).Key;

                case 'N':
                    return numConstantsTable.FirstOrDefault(x => x.Value == lexemeCode).Key;

                case 'C':
                    return stringConstantsTable.FirstOrDefault(x => x.Value == lexemeCode).Key;
            }

            return null;
        }

        public Dictionary<string, string> GetNumConstantsTable()
        {
            return numConstantsTable;
        }

        public Dictionary<string, string> GetOperatorsTable()
        {
            return operatorsTable;
        }

        public Dictionary<string, string> GetServiceWordsTable()
        {
            return serviceWordsTable;
        }

        public Dictionary<string, string> GetStringConstantsTable()
        {
            return stringConstantsTable;
        }

        public void AddDivider(string _div)
        {
            if (dividersTable.ContainsKey(_div)) return;

            dividersTable.Add(_div, 'R' + (dividersTable.Count + 1).ToString());
        }

        public void AddIdentifier(string _id)
        {
            if (identifiersTable.ContainsKey(_id)) return;

            identifiersTable.Add(_id, 'I' + (identifiersTable.Count + 1).ToString());
        }

        public void AddNumConstant(string _const)
        {
            if (numConstantsTable.ContainsKey(_const)) return;

            numConstantsTable.Add(_const, 'N' + (numConstantsTable.Count + 1).ToString());
        }

        public void AddOperator(string _op)
        {
            if (operatorsTable.ContainsKey(_op)) return;

            operatorsTable.Add(_op, 'O' + (operatorsTable.Count + 1).ToString());
        }

        public void AddStringConstant(string _const)
        {
            if (stringConstantsTable.ContainsKey(_const)) return;

            stringConstantsTable.Add(_const, 'C' + (stringConstantsTable.Count + 1).ToString());
        }

        public void ClearIdentifiersTable()
        {
            identifiersTable.Clear();
        }

        public void ClearNumConstsTable()
        {
            numConstantsTable.Clear();
        }

        public void ClearStringConstantsTable()
        {
            stringConstantsTable.Clear();
        }

        private void FillDividersTable()
        {
            foreach (char ch in dividers)
            {
                dividersTable.Add(ch.ToString(), 'R' + (dividersTable.Count + 1).ToString());
            }
        }

        private void FillOperatorsTable()
        {
            //One liter operations
            foreach (char ch in oneLiterOps)
                operatorsTable.Add(ch.ToString(), 'O' + (operatorsTable.Count + 1).ToString());

            //Two liter operations
            foreach (string op in twoLiterOps)
                operatorsTable.Add(op, 'O' + (operatorsTable.Count + 1).ToString());
        }

        private void FillServiceWordsTable()
        {
            StreamReader streamReader = new StreamReader(SERVICE_WORDS_FILE);

            string line = streamReader.ReadLine();
            while (line != null)
            {
                serviceWordsTable.Add(line, 'W' + (serviceWordsTable.Count + 1).ToString());
                line = streamReader.ReadLine();
            }

            streamReader.Close();
        }

        private void FillTwoLitersTable()
        {
            twoLiterOps = new List<string>();
            twoLiterOps.Add("==");
            twoLiterOps.Add("!=");
            twoLiterOps.Add("<=");
            twoLiterOps.Add(">=");
            twoLiterOps.Add("++");
            twoLiterOps.Add("--");
            twoLiterOps.Add("+=");
            twoLiterOps.Add("-=");
            twoLiterOps.Add("*=");
            twoLiterOps.Add("/=");
            twoLiterOps.Add("%=");
            twoLiterOps.Add("&=");
            twoLiterOps.Add("^=");
            twoLiterOps.Add("|=");
            twoLiterOps.Add("**");
            twoLiterOps.Add("<<");
            twoLiterOps.Add(">>");
            twoLiterOps.Add("||");
            twoLiterOps.Add("&&");
        }

        public bool IsDivider(string _div)
        {
            return dividers.Contains(_div);
        }

        public bool IsOneLiterOp(string _div)
        {
            return oneLiterOps.Contains(_div);
        }

        public bool IsTwoLiterOp(string _ch)
        {
            return twoLiterOps.Contains(_ch);
        }

        public bool IsTwoLiterOpBeg(char _ch)
        {
            foreach (string op in twoLiterOps)
                if (_ch == op[0]) return true;
            return false;
        }
    }
}
