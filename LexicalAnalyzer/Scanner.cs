using _1lab.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1lab
{
    class Scanner
    {
        private bool begOfLineFlag;
        private List<List<string>> lexemes;
        private ServiceTablesContainer serviceTablesContainer;
        private StateMachine stateMachine;
        public static string UNKNOWN_LEXEME = "Unknown lexeme";

        public Scanner()
        {
            lexemes = new List<List<string>>();
            serviceTablesContainer = ServiceTablesContainer.GetInstance();
            stateMachine = new StateMachine(this);
        }

        //Getters

        public Dictionary<string, string> GetDividersTable()
        {
            return serviceTablesContainer.GetDividersTable();
        }

        public Dictionary<string, string> GetIdentifiersTable()
        { 
            return serviceTablesContainer.GetIdentifiersTable();
        }

        public Dictionary<string, string> GetNumConstantsTable()
        {
            return serviceTablesContainer.GetNumConstantsTable();
        }

        public Dictionary<string, string> GetOperatorsTable()
        {
            return serviceTablesContainer.GetOperatorsTable();
        }

        public Dictionary<string, string> GetServiceWordsTable()
        { 
            return serviceTablesContainer.GetServiceWordsTable();
        }

        public Dictionary<string, string> GetStringConstantsTable()
        {
            return serviceTablesContainer.GetStringConstantsTable();
        }


        //Adds the divider to the dividers table
        public void AddDivider(string _div)
        {
            serviceTablesContainer.AddDivider(_div);
        }

        //Adds the identifier to the identifiers table
        public void AddIdentifier(string _id)
        {
            serviceTablesContainer.AddIdentifier(_id);
        }

        //Adds the lexeme to the lexemes list
        public void AddLexeme(string _lexeme)
        {
            lexemes[lexemes.Count-1].Add(_lexeme);
        }

        public void AddNumConstant(string _const)
        {
            serviceTablesContainer.AddNumConstant(_const);
        }

        //Adds the operator to the operators table
        public void AddOperator(string _op)
        {
            serviceTablesContainer.AddOperator(_op);
        }

        //Adds the stirng constant to the string constants table
        public void AddStringConstant(string _const)
        {
            serviceTablesContainer.AddStringConstant(_const);
        }

        public bool AtBeginningOfLine()
        {
            return begOfLineFlag;
        }

        public bool IsDivider(char _ch)
        {
            return serviceTablesContainer.IsDivider(_ch.ToString()) || begOfLineFlag;
        }

        public bool IsOneLiterOp(char _ch)
        {
            return serviceTablesContainer.IsOneLiterOp(_ch.ToString());
        }

        public bool IsOperation(char _ch)
        {
            return serviceTablesContainer.IsOneLiterOp(_ch.ToString());
        }

        public bool IsTwoLiterOp(string _ch)
        {
            return serviceTablesContainer.IsTwoLiterOp(_ch);
        }

        public bool IsTwoLiterOpBeg(char _ch)
        {
            return serviceTablesContainer.IsTwoLiterOpBeg(_ch);
        }

        //Returns the list of lexemes matching the code if it's correct
        //Otherwise returns null
        public List<List<string>> Proccess(List<string> _code)
        {
            lexemes.Clear();
            serviceTablesContainer.ClearIdentifiersTable();
            serviceTablesContainer.ClearNumConstsTable();
            serviceTablesContainer.ClearStringConstantsTable();
            stateMachine.SetState(new InitialState(stateMachine));

            foreach (string line in _code)
            {
                lexemes.Add(new List<string>());

                for (int i = 0; i < line.Length; ++i)
                {
                    if (i == 0)
                        begOfLineFlag = true;
                    else
                        begOfLineFlag = false;

                    stateMachine.NextChar(line[i]);

                    if (stateMachine.GetState() is FailureState)
                    {
                        AddLexeme(UNKNOWN_LEXEME);
                        return lexemes;
                    }
                }

                //Check if the previous line doesn't end with ;
                if (stateMachine.GetBufferContent() != "")
                {
                    stateMachine.NextChar(' ');
                }
            }

            return lexemes;
        }
    }
}
