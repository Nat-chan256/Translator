using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class IdentifierState : State
    {
        public IdentifierState(StateMachine _machine) : base(_machine)
        { }

        public override void NextChar(char _ch)
        {
            if (Char.IsLetter(_ch) || Char.IsDigit(_ch) || _ch == '_'
                || _ch == '$')
                machine.AddToBuffer(_ch);
            else if (IsOperation(_ch) || IsDivider(_ch))
            {
                SemanticProcedure1();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
