using System;
using System.Collections.Generic;
using System.Text;

namespace _1lab.States
{
    class InitialState : State
    {
        public InitialState(StateMachine _machine) : base(_machine)
        {
            
        }

        public override void NextChar(char _ch)
        {
            if (Char.IsLetter(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new LettersState(machine));
            }
            else if (_ch == '$' || _ch == '_')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new IdentifierState(machine));
            }
            else if (_ch == '\'' || _ch == '\"')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new StringConstState(machine));
            }
            else if (Char.IsDigit(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new DigitsState(machine));
            }
            else if (_ch == '.')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new PointState(machine));
            }
            else if (_ch == '/')
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new SlashState(machine));
            }
            else if (IsDivider(_ch))
            {
                machine.AddToBuffer(_ch);
                SemanticProcedure4();
                machine.ClearBuffer();
            }
            else if (IsTwoLiterOpBeg(_ch))
            {
                machine.AddToBuffer(_ch);
                machine.SetState(new TwoLiterOpState(machine));
            }
            else if (IsOneLiterOp(_ch))
            {
                machine.AddToBuffer(_ch);
                SemanticProcedure6();
                machine.ClearBuffer();
            }
            else
            {
                machine.SetState(new FailureState(machine));
            }
        }

    }
}
