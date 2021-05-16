using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class FloatNumState : State
    {
        public FloatNumState(StateMachine _machine) : base(_machine)
        {

        }


        public override void NextChar(char _ch)
        {
            if (_ch == 'E' || _ch == 'e')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new NumEState(machine));
            }
            else if (IsOperation(_ch) || IsDivider(_ch))
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
