using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class DigitsState : State
    {
        public DigitsState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            if (_ch == '.')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new DigitsPointState(machine));
            }
            else if (IsDivider(_ch) || IsOperation(_ch))
            {
                SemanticProcedure3();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else if (_ch == 'E' || _ch == 'e')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new NumEState(machine));
            }
            else if (Char.IsDigit(_ch))
                machine.AddToBuffer(_ch);
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
