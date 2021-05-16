using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class NumEPowState : State
    {
        public NumEPowState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            if (IsDivider(_ch) || IsOperation(_ch))
            {
                SemanticProcedure3();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else if (Char.IsDigit(_ch))
                machine.AddToBuffer(_ch);
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
