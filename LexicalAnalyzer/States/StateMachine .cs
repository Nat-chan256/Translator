using _1lab.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab
{
    class StateMachine
    {
        private List<char> buffer;
        private State currentState;
        private Scanner scanner;

        public StateMachine(Scanner _scanner)
        {
            scanner = _scanner;
            currentState = new InitialState(this);
            buffer = new List<char>();
        }

        //Getters

        public string GetBufferContent()
        {
            string bufferContent = "";
            foreach (char ch in buffer)
                bufferContent += ch;
            return bufferContent;
        }

        public Dictionary<string, string> GetDividersTable()
        {
            return scanner.GetDividersTable();
        }

        public Dictionary<string, string> GetIdentifiersTable()
        {
            return scanner.GetIdentifiersTable();
        }

        public Dictionary<string, string> GetNumConstantsTable()
        {
            return scanner.GetNumConstantsTable();
        }

        public Dictionary<string, string> GetOperatorsTable()
        {
            return scanner.GetOperatorsTable();
        }

        public Dictionary<string, string> GetServiceWordsTable()
        {
            return scanner.GetServiceWordsTable();
        }

        public State GetState()
        {
            return currentState;
        }


        public Dictionary<string, string> GetStringConstantsTable()
        { 
            return scanner.GetStringConstantsTable();
        }



        public void SetState(State _state)
        {
            currentState = _state;
        }



        public void AddDivider(string _div)
        {
            scanner.AddDivider(_div);
        }

        public void AddIdentifier(string _id)
        {
            scanner.AddIdentifier(_id);
        }

        public void AddLexeme(string _lexeme)
        {
            scanner.AddLexeme(_lexeme);
        }

        public void AddNumConstant(string _const)
        {
            scanner.AddNumConstant(_const);
        }

        public void AddOperator(string _op)
        {
            scanner.AddOperator(_op);
        }

        public void AddStringConstant(string _const)
        {
            scanner.AddStringConstant(_const);
        }

        public void AddToBuffer(char _ch)
        {
            buffer.Add(_ch);
        }

        public bool AtBeginningOfLine()
        {
            return scanner.AtBeginningOfLine();
        }

        public void ClearBuffer()
        {
            buffer.Clear();
        }

        public bool IsDivider(char _ch)
        {
            return scanner.IsDivider(_ch);
        }

        public bool IsOneLiterOp(char _ch)
        {
            return scanner.IsOneLiterOp(_ch);
        }

        public bool IsOperation(char _ch)
        {
            return scanner.IsOperation(_ch);
        }

        public bool IsTwoLiterOp(string _ch)
        {
            return scanner.IsTwoLiterOp(_ch);
        }

        public bool IsTwoLiterOpBeg(char _ch)
        {
            return scanner.IsTwoLiterOpBeg(_ch);
        }

        //Changes the current state if neccessary
        public void NextChar(char _ch)
        {
            currentState.NextChar(_ch);
        }
    }
}
