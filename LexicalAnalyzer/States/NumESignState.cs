using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class NumESignState : State
    {
        public NumESignState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            if (Char.IsDigit(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new NumEPowState(machine));
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
