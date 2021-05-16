using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class StringConstState : State
    {
        private char quotationMark;

        public StringConstState(StateMachine _machine) : base(_machine)
        {
            try
            {
                quotationMark = machine.GetBufferContent()[0];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Quotation mark not found in buffer. Default value was set.");
                quotationMark = '\"';
            }
        }

        public override void NextChar(char _ch)
        {
            machine.AddToBuffer(_ch);
            if (_ch == quotationMark)
            {
                SemanticProcedure3();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
            }
        }
    }
}
