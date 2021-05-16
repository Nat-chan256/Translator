using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class MultiLineCommentAstrState : State
    {
        public MultiLineCommentAstrState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            if (_ch == '/')
            {
                SemanticProcedure5();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
            }
            else
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new MultiLineCommentState(machine));
            }
        }
    }
}
