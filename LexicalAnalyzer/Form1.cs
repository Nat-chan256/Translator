using _1lab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace LexicalAnalyzer
{

    public partial class Form1 : Form
    {
        //Сканнер для формирования списка кодов лексем по исходному коду
        private Scanner scanner;

        // Преобразователь в обратную польскую нотацию
        private RPNConverter rpnConverter;

        // Преобразователь ОПЗ в программу на Basic
        private RPNtoBasicConverter rpnToBasicConverter;

        public Form1()
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Открыть исходный код";
            openFileDialog.DefaultExt = "js";

            sfdSaveSourceCode = new SaveFileDialog();
            sfdSaveSourceCode.Filter = "JavaScript file (*.js;*.jsm;*jsx;*.ts;*.tsx)|*.js;*.jsm;*jsx;*.ts;*.tsx|All files(*.*)|*.*";
            sfdSaveSourceCode.FilterIndex = 2;
            sfdSaveSourceCode.Title = "Сохранить исходный код";
            sfdSaveSourceCode.DefaultExt = "js";

            sfdSaveOutputFile = new SaveFileDialog();
            sfdSaveOutputFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            sfdSaveOutputFile.FilterIndex = 2;
            sfdSaveOutputFile.Title = "Сохранить лексемы";
            sfdSaveOutputFile.DefaultExt = "txt";

            // Инициализация сканера
            scanner = new Scanner();

            // Инициализация преобразователя в польскую нотацию
            rpnConverter = new RPNConverter();

            rpnToBasicConverter = new RPNtoBasicConverter();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Добавление столбцов в таблицы
            dgvIdentifiersTable.Columns.Add("Identifier", "Идентификатор");
            dgvIdentifiersTable.Columns.Add("Lexeme", "Лексема");

            dgvServiceWords.Columns.Add("ServiceWord", "Служебное слово");
            dgvServiceWords.Columns.Add("Lexeme", "Лексема");

            dgvOperators.Columns.Add("Operator", "Оператор");
            dgvOperators.Columns.Add("Lexeme", "Лексема");

            dgvDividers.Columns.Add("Divider", "Разделитель");
            dgvDividers.Columns.Add("Lexeme", "Лексема");

            dgvConstants.Columns.Add("Constant", "Константа");
            dgvConstants.Columns.Add("Lexeme", "Лексема");



            //Заполнение служебных таблиц 
            FillIdentifiersTable();
            FillServiceWordsTable();
            FillOperatorsTable();
            FillDividersTable();
            FillConstantsTable();
        }


        //======================================Вкладка "Лексический анализатор"====================================

        // Загружает исходный код программы в форму
        private void tsiOpenSourceFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader streamReader = new StreamReader(openFileDialog.FileName);
                    rtbSourceCode.Clear();
                    string line = streamReader.ReadLine();
                    while (line != null)
                    {
                        rtbSourceCode.Text += line + "\n";
                        line = streamReader.ReadLine();
                    }
                    streamReader.Close();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        // Сохраняет исхоный код программы в файл
        private void tsmiSaveSourceFile_Click(object sender, EventArgs e)
        {
            if (sfdSaveSourceCode.ShowDialog() == DialogResult.OK)
            {
                if (sfdSaveSourceCode.FileName != "")
                {
                    FileStream fileStream = (FileStream)sfdSaveSourceCode.OpenFile();
                    //Convert the string to the bytes
                    byte[] text = System.Text.Encoding.Default.GetBytes(rtbSourceCode.Text);
                    fileStream.Write(text, 0, text.Length);
                    fileStream.Close();
                }
            }
        }

        //Сохраняет файл с лексемами
        private void tsmiSaveOutputFile_Click(object sender, EventArgs e)
        {
            if (sfdSaveOutputFile.ShowDialog() == DialogResult.OK)
            {
                if (sfdSaveOutputFile.FileName != "")
                {
                    FileStream fileStream = (FileStream)sfdSaveOutputFile.OpenFile();
                    //Convert the string to the bytes
                    byte[] text = System.Text.Encoding.Default.GetBytes(rtbLexemes.Text);
                    fileStream.Write(text, 0, text.Length);
                    fileStream.Close();
                }
            }
        }

        // Обработчик события нажатия кнопки "Выполнить"
        // Выводит список кодов лексем исходного кода на форму
        private void bExecute_Click(object sender, EventArgs e)
        {
            // Делаем кнопки "Выполнить" на всех вкладках доступными
            SetPerformButtonsEnable(true);

            List<string> sourceCode = new List<string>(); //Source code line by line

            //Read from rich textbox
            string[] sourceCodeArr = rtbSourceCode.Text.Split('\n');
            foreach (string line in sourceCodeArr)
                sourceCode.Add(line);

            //Proccess the source code with the scanner to get the lexemes list
            List<List<string>> lexemesList = scanner.Proccess(sourceCode);

            rtbLexemes.Clear();
            foreach (List<string> line in lexemesList)
            {
                foreach (string lexeme in line)
                    rtbLexemes.Text += (lexeme + " ");
                rtbLexemes.Text += "\n";
            }

            //Update identifiers table
            FillIdentifiersTable();
            //Update constants table
            FillConstantsTable();

            // Переносим исходный код во вкладку "Перевод в ОПЗ"
            rtbSourceCodeRPN.Text = rtbSourceCode.Text;

            // Если была обнаружена неизветная лексема - блокируем кнопки "Выполнить" на всех вкладках, кроме первой
            if (ContainsUnknownLexeme(lexemesList))
            {
                SetPerformButtonsEnable(false);
            }
        }


        //======================================Вкладка "Служебные таблицы"====================================

        //Заполнение таблицы констант
        private void FillConstantsTable()
        {
            dgvConstants.Rows.Clear();

            Dictionary<string, string> numConstTable = scanner.GetNumConstantsTable();
            Dictionary<string, string> stringConstTable = scanner.GetStringConstantsTable();

            foreach (string numConstant in numConstTable.Keys)
            {
                dgvConstants.Rows.Add(numConstant, numConstTable[numConstant]);
            }

            foreach (string strConst in stringConstTable.Keys)
            {
                dgvConstants.Rows.Add(strConst, stringConstTable[strConst]);
            }
        }

        //Заполнение таблицы разделителей
        private void FillDividersTable()
        {
            Dictionary<string, string> dividersTable = scanner.GetDividersTable();

            foreach (string divider in dividersTable.Keys)
            {
                dgvDividers.Rows.Add(divider, dividersTable[divider]);
            }
        }

        //Заполенние таблицы идентификаторов
        private void FillIdentifiersTable()
        {
            dgvIdentifiersTable.Rows.Clear();

            Dictionary<string, string> identifiersTable = scanner.GetIdentifiersTable();

            foreach (string identifier in identifiersTable.Keys)
            {
                dgvIdentifiersTable.Rows.Add(identifier, identifiersTable[identifier]);
            }
        }

        //Заполнение таблицы операторов
        private void FillOperatorsTable()
        {
            Dictionary<string, string> operatorsTable = scanner.GetOperatorsTable();

            foreach (string op in operatorsTable.Keys)
            {
                dgvOperators.Rows.Add(op, operatorsTable[op]);
            }
        }

        //Заполнение таблицы служебных слов
        private void FillServiceWordsTable()
        {
            Dictionary<string, string> serviceWordsTable = scanner.GetServiceWordsTable();

            foreach (string serviceWord in serviceWordsTable.Keys)
            {
                dgvServiceWords.Rows.Add(serviceWord, serviceWordsTable[serviceWord]);
            }
        }




        //======================================Вкладка "Перевод в ОПЗ"====================================
        private void bExecuteRPN_Click(object sender, EventArgs e)
        {
            List<string> lexemesCodes = new List<string>(); // Коды лексем исходного кода построчно

            // Чтение из текстбокса строк с кодами лексем
            string[] lexemesCodesLines = rtbLexemes.Text.Split('\n');
            foreach (string line in lexemesCodesLines)
            {
                lexemesCodes.Add(line);
            }

            List<List<String>> rpn = rpnConverter.ConvertToRPN(lexemesCodes);

            rtbRPN.Clear();
            foreach (List<string> line in rpn)
            {
                foreach (string word in line)
                    rtbRPN.Text += (word + " ");
                rtbRPN.Text += "\n";
            }

            // Записываем ОПЗ в следующую вкладку
            rtbRPNBasicTranslationTab.Clear();
            foreach (List<string> line in rpn)
            {
                foreach (string word in line)
                {
                    rtbRPNBasicTranslationTab.Text += word + " ";
                }
                rtbRPNBasicTranslationTab.Text += "\n";
            }
        }


        //======================================Вкладка "Перевод в Basic"====================================
        private void btnExecuteBasicTranslationTab_Click(object sender, EventArgs e)
        {
            // Считываем ОПЗ
            string[] rpnText = rtbRPNBasicTranslationTab.Text.Split('\n');
            List<List<string>> rpn = new List<List<string>>();
            // "Склеиваем" строковые константы, если это необходимо
            bool isString = false;
            string currentStr = "";
            for (int i = 0; i < rpnText.Length; ++i)
            {
                string[] line = rpnText[i].Split();
                List<string> rpnLine = new List<string>();
                for (int j = 0; j < line.Length; ++j)
                {
                    if (line[j].Contains("\"") || line[j].Contains("\'"))
                    {
                        isString = !isString;
                        currentStr += line[j];
                        // Если строка закончилась
                        if (!isString)
                        {
                            // Добавляем её в список элементов ОПЗ
                            rpnLine.Add(currentStr);
                            currentStr = "";
                        }
                        else
                        {
                            currentStr += " ";
                        }
                    }
                    else if (isString)
                    {
                        currentStr += line[j] + " ";
                    }
                    else
                    {
                        rpnLine.Add(line[j]);
                    }
                }
                rpn.Add(rpnLine);
            }

            List<List<string>> basicCode = rpnToBasicConverter.ConvertToBasic(rpn);

            rtbBasic.Clear();
            foreach (List<string> line in basicCode)
            {
                foreach (string word in line)
                    rtbBasic.Text += (word + " ");
                rtbBasic.Text += "\n";
            }
        }

        //======================================Вспомогательные методы====================================

        // Проверяет, есть ли в _listToCheck неизвестная лексема 
        private bool ContainsUnknownLexeme(List<List<string>> _listToCheck)
        { 
            foreach (List<string> innerList in _listToCheck)
            {
                foreach (string str in innerList)
                {
                    if (str == Scanner.UNKNOWN_LEXEME)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Устанавливает значение false свойства Enabled для кнопок "Выполнить" на всех вкладках (кроме первой) 
        private void SetPerformButtonsEnable(bool _value)
        {
            bExecuteRPN.Enabled = _value;
        }

        
    }
}
