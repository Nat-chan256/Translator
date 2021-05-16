using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class MultiLineCommentState : State
    {
        public MultiLineCommentState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            machine.AddToBuffer(_ch);
            if (_ch == '*')
                machine.SetState(new MultiLineCommentAstrState(machine));
        }
    }
}
