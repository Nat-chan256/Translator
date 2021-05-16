using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    abstract class State
    {
        protected StateMachine machine;

        public State(StateMachine _machine)
        {
            machine = _machine;
        }

        protected bool IsDivider(char _ch)
        {
            return machine.IsDivider(_ch);
        }

        protected bool IsIdentifierSymbol(char _ch)
        {
            return Char.IsLetter(_ch) || Char.IsDigit(_ch) || _ch == '$' || _ch == '_';
        }

        protected bool IsOneLiterOp(char _ch)
        {
            return machine.IsOneLiterOp(_ch);
        }

        protected bool IsOperation(char _ch)
        {
            return machine.IsOperation(_ch);
        }

        protected bool IsTwoLiterOp(string _ch)
        {
            return machine.IsTwoLiterOp(_ch);
        }

        protected bool IsTwoLiterOpBeg(char _ch)
        {
            return machine.IsTwoLiterOpBeg(_ch);
        }

        //Gets the symbol and changes the state if neccessary
        public abstract void NextChar(char _ch);


        //Semantic procedures

        //Finds the current symbol in the identifiers table
        protected void SemanticProcedure1()
        {
            string curSymbol = machine.GetBufferContent();

            if (!machine.GetIdentifiersTable().ContainsKey(curSymbol))
                machine.AddIdentifier(curSymbol); //Add new identifier to the identifiers table

            machine.AddLexeme(machine.GetIdentifiersTable()[curSymbol]);
        }

        //Finds the current symbol in the service words table
        protected void SemanticProcedure2()
        {
            string curSymbol = machine.GetBufferContent();

            if (machine.GetServiceWordsTable().ContainsKey(curSymbol))
                machine.AddLexeme(machine.GetServiceWordsTable()[curSymbol]);
            else
                SemanticProcedure1();
        }

        //Writes the current symbol to the constants table
        protected void SemanticProcedure3()
        {
            string curSymbol = machine.GetBufferContent();

            if (curSymbol[0] == '\'' || curSymbol[0] == '\"') //If the constant is a line
            {
                machine.AddStringConstant(curSymbol);
                machine.AddLexeme(machine.GetStringConstantsTable()[curSymbol]);
            }
            else //If the constant is a number
            {
                machine.AddNumConstant(curSymbol);
                machine.AddLexeme(machine.GetNumConstantsTable()[curSymbol]);
            }
        }

        //Finds the current symbol in dividers table
        protected void SemanticProcedure4()
        {
            string curSymbol = machine.GetBufferContent();

            if (curSymbol[0] == ' ' || curSymbol[0] == '\t') return;

            if (!machine.GetDividersTable().ContainsKey(curSymbol))
                machine.AddDivider(curSymbol); //Add new operator to the operators table

            machine.AddLexeme(machine.GetDividersTable()[curSymbol]);
        }

        protected void SemanticProcedure5()
        {
            machine.ClearBuffer();
        }

        //Finds the current symbol in operations table
        protected void SemanticProcedure6()
        {
            string curSymbol = machine.GetBufferContent();

            if (!machine.GetOperatorsTable().ContainsKey(curSymbol))
                machine.AddOperator(curSymbol); //Add new operator to the operators table

            machine.AddLexeme(machine.GetOperatorsTable()[curSymbol]);
        }
    }
}
