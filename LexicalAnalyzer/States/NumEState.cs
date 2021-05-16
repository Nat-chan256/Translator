using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class NumEState : State
    {
        public NumEState(StateMachine _machine) : base(_machine)
        {

        }


        public override void NextChar(char _ch)
        {
            if (_ch == '+' || _ch == '-')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new NumESignState(machine));
            }
            else if (Char.IsDigit(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new NumEPowState(machine));
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
