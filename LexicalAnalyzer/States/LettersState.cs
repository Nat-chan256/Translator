using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class LettersState : State
    {
        public LettersState(StateMachine _machine) : base(_machine)
        {

        }


        public override void NextChar(char _ch)
        {
            if (Char.IsLetter(_ch))
                machine.AddToBuffer(_ch);
            else if (Char.IsDigit(_ch) || _ch == '$' || _ch == '_')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new IdentifierState(machine));
            }
            else if (IsDivider(_ch) || IsOperation(_ch))
            {
                SemanticProcedure2();
                machine.ClearBuffer();
                machine.SetState(new InitialState(machine));
                machine.NextChar(_ch);
            }
            else
                machine.SetState(new FailureState(machine));
        }
    }
}
