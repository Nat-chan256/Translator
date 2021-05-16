using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class DigitsPointState : State
    {
        public DigitsPointState(StateMachine _machine) : base(_machine)
        {

        }


        public override void NextChar(char _ch)
        {
            if (IsOperation(_ch) || IsDivider(_ch))
            {
                SemanticProcedure3();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else if (Char.IsDigit(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new FloatNumState(machine));
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
