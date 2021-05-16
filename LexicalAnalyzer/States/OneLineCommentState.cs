using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class OneLineCommentState : State
    {
        public OneLineCommentState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            if (machine.AtBeginningOfLine())
            {
                SemanticProcedure5();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else
                machine.AddToBuffer(_ch);
        }
    }
}
