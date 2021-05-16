using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class FailureState : State
    {
        public FailureState(StateMachine _machine) : base(_machine)
        { }

        public override void NextChar(char _ch)
        {
            throw new NotImplementedException();
        }
    }
}
