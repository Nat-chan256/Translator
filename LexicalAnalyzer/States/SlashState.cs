using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class SlashState : State
    {
        public SlashState(StateMachine _machine) : base(_machine)
        {

        }

        public override void NextChar(char _ch)
        {
            if (Char.IsLetter(_ch) || Char.IsDigit(_ch) || _ch == '_' || _ch == '$' || IsDivider(_ch))
            {
                SemanticProcedure6();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else if (_ch == '/')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new OneLineCommentState(machine));
            }
            else if (_ch == '*')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new MultiLineCommentState(machine));
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
